using UnityEngine;

public class S_ : MonoBehaviour
{
    public Transform cameraPosition;
    
    void Start()
    {
        
    }


    void Update()
    {
        transform.position = cameraPosition.position;
    }
}
