using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    public int id;
    public float x, y;
    public List<Edge> edges = new List<Edge>();

    public GraphNode(int id, float x, float y)
    {
        this.id = id;
        this.x = x;
        this.y = y;
    }
}

public class Edge
{
    public GraphNode nodeA, nodeB;
    public float weight;

    public Edge(GraphNode a, GraphNode b, float weight)
    {
        nodeA = a;
        nodeB = b;
        this.weight = weight;
    }
}
public class Graph
{
    public List<GraphNode> nodes = new List<GraphNode>();
    public List<Edge> edges = new List<Edge>();

    public void AddEdge(GraphNode a, GraphNode b)
    {
        float weight = Vector2.Distance(new Vector2(a.x, a.y), new Vector2(b.x, b.y));
        Edge edge = new Edge(a, b, weight);
        edges.Add(edge);

        a.edges.Add(edge);
        b.edges.Add(edge);
    }

    /* // Delaunay Üçgenlerinden MST Grafiği Üretme
    public Graph ConvertTriangulationToGraph(List<Triangle> triangles)
    {
        Graph graph = new Graph();
        Dictionary<Vector2, GraphNode> nodeMap = new Dictionary<Vector2, GraphNode>();

        foreach (var triangle in triangles)
        {
            Vector2[] points = {
            new Vector2(triangle.p1.x, triangle.p1.y),
            new Vector2(triangle.p2.x, triangle.p2.y),
            new Vector2(triangle.p3.x, triangle.p3.y)
        };

            GraphNode[] nodes = new GraphNode[3];

            for (int i = 0; i < 3; i++)
            {

                if (!nodeMap.ContainsKey(points[i]))
                {
                    GraphNode newNode = new GraphNode(nodeMap.Count, points[i].x, points[i].y);
                    nodeMap[points[i]] = newNode;
                    graph.nodes.Add(newNode);
                }
                nodes[i] = nodeMap[points[i]];
            }

            // Kenarları ekle
            graph.AddEdge(nodes[0], nodes[1]);
            graph.AddEdge(nodes[1], nodes[2]);
            graph.AddEdge(nodes[2], nodes[0]);
        }

        return graph;
    } */
    // Delaunay Üçgenlerinden MST Grafiği Üretme
    public Graph ConvertTriangulationToGraph(List<Triangle> triangles)
    {
        Graph graph = new Graph();
        Dictionary<Vector2, GraphNode> nodeMap = new Dictionary<Vector2, GraphNode>();

        // Büyük üçgenin noktaları
        Vector2 p1 = new Vector2(-60, -60);
        Vector2 p2 = new Vector2(-60, -180);
        Vector2 p3 = new Vector2(180, -60);

        foreach (var triangle in triangles)
        {
            Vector2[] points = {
                new Vector2(triangle.p1.x, triangle.p1.y),
                new Vector2(triangle.p2.x, triangle.p2.y),
                new Vector2(triangle.p3.x, triangle.p3.y)
            };

            GraphNode[] nodes = new GraphNode[3];

            for (int i = 0; i < 3; i++)
            {
                // Büyük üçgenin noktalarını kontrol et ve ekleme
                if (points[i] == p1 || points[i] == p2  ||points[i] == p3)
                    continue;

                // Yeni düğüm ekleme
                if (!nodeMap.ContainsKey(points[i]))
                {
                    GraphNode newNode = new GraphNode(nodeMap.Count, points[i].x, points[i].y);
                    nodeMap[points[i]] = newNode;
                    graph.nodes.Add(newNode);
                }
                nodes[i] = nodeMap[points[i]];
            }

            // Eğer geçerli üçgenin tüm noktaları mevcutsa, kenarları ekle
            if (nodes[0] != null && nodes[1] != null) graph.AddEdge(nodes[0], nodes[1]);
            if (nodes[1] != null && nodes[2] != null) graph.AddEdge(nodes[1], nodes[2]);
            if (nodes[2] != null && nodes[0] != null) graph.AddEdge(nodes[2], nodes[0]);
        }

        return graph;
    }
}

