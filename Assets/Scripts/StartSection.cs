using UnityEngine;

public class StartSection : MonoBehaviour
{
    [SerializeField] private StartPieceExit[] exits;
    public StartPieceExit[] Exits => exits;
}