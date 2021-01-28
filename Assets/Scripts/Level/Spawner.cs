using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject objectToSpawn; 
    
    private void Start()
    {
        Spawn();
    }

    public void Spawn()
    {
        Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
}
