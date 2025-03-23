using System.Collections.Generic;
using System.Net.NetworkInformation;
using Unity.VisualScripting;
using UnityEngine;

public class RoomGeneration : MonoBehaviour
{
    [Header("StartRoom")]	
    [SerializeField] GameObject startRoom;
    [SerializeField] Vector3 startRoomPos;
    [SerializeField] bool startRoomActive;
    [Header("Rooms")]	
    [SerializeField] Transform parentTransform;
    [SerializeField] LayerMask unwalkableMask;
    [SerializeField] GameObject[] rooms1x1;
    [SerializeField] GameObject[] rooms1x2;
    [SerializeField] GameObject[] rooms1x3;
    [SerializeField] GameObject[] rooms2x2;
    [SerializeField] GameObject[] rooms2x3;
    [SerializeField] GameObject[] rooms3x3;
    [SerializeField] GameObject[] rooms3x4;
    [SerializeField] bool rooms1x1Active;
    [SerializeField] bool rooms1x2Active;
    [SerializeField] bool rooms1x3Active;
    [SerializeField] bool rooms2x2Active;
    [SerializeField] bool rooms2x3Active;
    [SerializeField] bool rooms3x3Active;
    [SerializeField] bool rooms3x4Active;

    // X ve z değeri grid boyutunu belirler.
    int x, z;
    int nodeDiameter;

    [Header("Pathfinding")]	
    [SerializeField] private int extraEdgeCount;
    Grid grid;
    DelaunayTriangulation delaunayTriangulation;
    Graph graph;
    MST mst;
    DrawMST drawMST;
    Pathfinding pathfinding;

    List<Point> roomsPos = new List<Point>();

    void Awake()
    {
        grid = FindFirstObjectByType<Grid>();
        delaunayTriangulation = FindFirstObjectByType<DelaunayTriangulation>();
        graph = new Graph();
        mst=FindFirstObjectByType<MST>();
        drawMST=FindFirstObjectByType<DrawMST>();
        pathfinding = FindFirstObjectByType<Pathfinding>();
    }
    void Start()
    {
        x = grid.gridSizeX;
        z = grid.gridSizeY;
        nodeDiameter = (int)grid.nodeDiameter;
        CreateRooms();
        PathFunctions();
       
    }
    /// <summary>
    /// Odaları oluşturur.
    /// </summary>
    private void CreateRooms()
    {
        CreateRoom(startRoom,startRoomActive);
        CreateRoom(rooms1x1, rooms1x1.Length, rooms1x1Active);
        CreateRoom(rooms1x2, rooms1x2.Length, rooms1x2Active);
        CreateRoom(rooms1x3, rooms1x3.Length, rooms1x3Active);
        CreateRoom(rooms2x2, rooms2x2.Length, rooms2x2Active);
        CreateRoom(rooms2x3, rooms2x3.Length, rooms2x3Active);
        CreateRoom(rooms3x3, rooms3x3.Length, rooms3x3Active);
        CreateRoom(rooms3x4, rooms3x4.Length, rooms3x4Active);
    }
    /// <summary>
    /// Üçgen oluşturma ve yol bulma işlemleri yapılır.
    /// </summary>
    private void PathFunctions()
    {
        Debug.Log("CreateRooms() başladı.");

        var triangles = delaunayTriangulation.GenerateTriangulation(roomsPos);
        if (triangles == null || triangles.Count == 0)
        {
            Debug.LogError("Delaunay Triangulation başarısız! Triangles listesi boş.");
        }

        graph = graph.ConvertTriangulationToGraph(triangles);
        if (graph == null)
        {
            Debug.LogError("ConvertTriangulationToGraph başarısız! Graph null döndü.");
        }
        if (graph.nodes == null || graph.nodes.Count == 0)
        {
            Debug.LogError("Graph oluşturuldu ama içi boş!");
        }

        Debug.Log($"Graph başarıyla oluşturuldu! {graph.nodes.Count} düğüm var.");

        var mstEdges = mst.GetMST(graph);
        mst.DrawExtraEdges(extraEdgeCount);
        if (mstEdges == null || mstEdges.Count == 0)
        {
            Debug.LogError("MST oluşturuldu ama içi boş!");
        }
        if (drawMST == null)
        {
            Debug.LogError("DrawMST is null!");
        }
        drawMST.DrawEdges(mstEdges);
        pathfinding.FindPathBetweenTwoPoint(mstEdges);
    }
    private void CreateRoom(GameObject room, bool isActive)
    {
         if (isActive)
        {
            Vector3 startRoomWorldPos= grid.CalculateWorldPoint((int)startRoomPos.x, (int)startRoomPos.z);
            GameObject startRoomTemp = Instantiate(room, startRoomWorldPos, Quaternion.identity);
            DrawRoom((int)startRoomPos.x, (int)startRoomPos.z, room);
            roomsPos.Add(new Point(startRoomWorldPos.x, startRoomWorldPos.z));
            OpenDoors(startRoomTemp);
            startRoomTemp.transform.SetParent(parentTransform);
        }
    }
    /// <summary>
    /// Oda oluşturur.
    /// </summary>
    /// <param name="room"></param>
    /// <param name="length"></param>
    /// <param name="isActive"></param>
    private void CreateRoom(GameObject[] room, int length, bool isActive)
    {
        if (isActive)
        {
            for (int i = 0; i < length; i++)
            {
                Vector3 randomPos = GetRandom(room[i]);
                Debug.Log("Random Pos: " + randomPos);
                GameObject roomTemp = Instantiate(room[i], new Vector3(randomPos.x, 0, randomPos.z), Quaternion.identity);
                roomsPos.Add(new Point((int)randomPos.x, (int)randomPos.z));
                OpenDoors(roomTemp);
                roomTemp.transform.SetParent(parentTransform);
            }
        }
    }
    /// <summary>
    /// Grid'de random bir pozisyon bulur ve dünya konumunu gönderir.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    private Vector3 GetRandom(GameObject room)
    {
        int maxAttempts = 100; // Maksimum deneme sayısı
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            int xLocal = (int) room.transform.GetChild(0).GetComponent<Room>().size.x / nodeDiameter;
            int zLocal = (int)room.transform.GetChild(0).GetComponent<Room>().size.z / nodeDiameter;

            int xRandom = Randomize(3, x -2);
            int zRandom = Randomize(3, z - 2);
            Debug.Log("Random x: " + xRandom + " Random z: " + zRandom);
            if (xRandom <= x - xLocal -2 && zRandom <= z - zLocal -2 && CheckRoom(xRandom, zRandom, room))
            {
                Vector3 randomWorldPos = grid.CalculateWorldPoint(xRandom, zRandom);
                Debug.Log("Random World Pos: " + randomWorldPos);
                DrawRoom(xRandom, zRandom, room);
                return new Vector3(randomWorldPos.x, 0, randomWorldPos.z);
            }
            attempts++;
        }
        Debug.LogError("Uygun pozisyon bulunamadı!");
        return Vector3.zero;
    }
    /// <summary>
    /// Odanın kapılarını açar.
    /// </summary>
    /// <param name="room"></param>
    private void OpenDoors(GameObject room)
    {
        int doorCountToOpen = Randomize(1, room.transform.GetChild(0).childCount/2+ 1) ;
        int counter = 0;
        while (counter < doorCountToOpen)
        {
            int randomDoor = Randomize(0, room.transform.GetChild(0).childCount);
            if (room.transform.GetChild(0).GetChild(randomDoor).gameObject.activeSelf)
            {
                counter++;
                room.transform.GetChild(0).GetChild(0).GetChild(randomDoor).GetComponent<Door>().isOpen = true;
                room.transform.GetChild(0).GetChild(0).GetChild(randomDoor).GetChild(0).GetComponent<MeshRenderer>().material.color = Color.green;
            }
        }
    }
    /// <summary>
    /// Random sayı üretir.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    private int Randomize(int x, int y)
    {
        return Random.Range(x, y);
    }
    /// <summary>
    /// Odanın uygun olup olmadığı kontrol edilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    /// <returns></returns>
    private bool CheckRoom(int xRandom, int zRandom, GameObject room)
    {
        return grid.CheckIfRoomFits(xRandom, zRandom, room);
    }
    /// <summary>
    /// Oda çizilir.
    /// </summary>
    /// <param name="xRandom"></param>
    /// <param name="zRandom"></param>
    /// <param name="room"></param>
    private void DrawRoom(int xRandom, int zRandom, GameObject room)
    {
        grid.DrawRoom(xRandom, zRandom, room);
    }
}
