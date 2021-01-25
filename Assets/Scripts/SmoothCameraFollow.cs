using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [Tooltip("The camera's follow smoothing speed.")] [SerializeField] [Range(0.0f, 1.0f)]
    private float smoothSpeed = 0.125f;

    [Tooltip("The camera's offset position from the player.")] [SerializeField]
    private Vector3 _offset = new Vector3(0f, 10f, 0f);

    [Tooltip("Amount the board tilts on input, in degrees.")] [SerializeField] [Range(0.0f, 50.0f)]
    private float tiltAmount = 15.0f;
    
    private GameObject _playerObject;
    private Player _player;

    private void FixedUpdate()
    {
        if (_playerObject == null)
        {
            _playerObject = GameObject.FindGameObjectWithTag(playerTag);
            if (_playerObject != null)
            {
                _player = _playerObject.GetComponent<Player>();
            }
        }
        else
        {
            // Follow player
            Vector3 targetPosition = _playerObject.transform.position + _offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
            
            // Rotate on input to simulate a labyrinth
            Quaternion targetRotation = Quaternion.Euler(90f, 0f, 0f) * 
                                        Quaternion.AngleAxis(_player.PlayerInput.x * tiltAmount, Vector3.up) *
                                        Quaternion.AngleAxis(-_player.PlayerInput.y * tiltAmount, Vector3.right);
            
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                smoothSpeed);
        }
    }
}