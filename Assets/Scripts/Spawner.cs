using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; 
    
    private void Start()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
