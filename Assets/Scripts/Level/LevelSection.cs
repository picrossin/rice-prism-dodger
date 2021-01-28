using UnityEngine;

public class LevelSection : MonoBehaviour
{
    [SerializeField] private GameObject exit;
    public GameObject Exit => exit;

    [SerializeField] private bool isRightTurn;
    public bool IsRightTurn => isRightTurn;
    
    [SerializeField] private bool isLeftTurn;
    public bool IsLeftTurn => isLeftTurn;
}
