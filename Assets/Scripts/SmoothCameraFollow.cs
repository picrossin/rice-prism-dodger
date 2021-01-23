using UnityEngine;

public class SmoothCameraFollow : MonoBehaviour
{
    [SerializeField] private string playerTag = "Player";

    [Tooltip("The camera's follow smoothing speed.")] [SerializeField] [Range(0.0f, 1.0f)]
    private float smoothSpeed = 0.125f;

    [Tooltip("The camera's offset position from the player.")] [SerializeField]
    private Vector3 _offset = new Vector3(0f, 10f, 0f);

    private GameObject _player;

    private void FixedUpdate()
    {
        if (_player == null)
        {
            _player = GameObject.FindGameObjectWithTag(playerTag);
        }
        else
        {
            Vector3 targetPosition = _player.transform.position + _offset;
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed);
        }
    }
}