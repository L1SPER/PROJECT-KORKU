using UnityEngine;

public class Door : MonoBehaviour
{
    //[SerializeField] public GameObject doorMesh ;
    [SerializeField] public bool isOpen;
    [SerializeField] public bool isIntersect;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Door"))
        {
            isIntersect = true;
            //Diger  objeyi  de yesil  olcak
            Debug.Log("Kapılar birbirine girdi.");
            Debug.Log("Obje ismi:"+other.name);
        }
    }
}
