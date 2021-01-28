using UnityEngine;

public class StartSection : MonoBehaviour
{
    [SerializeField] private StartPieceExit[] exits;
    public StartPieceExit[] Exits => exits;

    [SerializeField] private Spawner playerSpawner;
    public Spawner PlayerSpawner => playerSpawner;
}