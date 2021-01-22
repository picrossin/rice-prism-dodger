using System;
using UnityEngine;

public class TiltMovement : MonoBehaviour
{
    [Tooltip("Amount the board tilts on input, in degrees.")] [SerializeField] [Range(0.0f, 50.0f)]
    private float tiltAmount = 15.0f;

    [Tooltip("The board's rotation smoothing speed.")] [SerializeField] [Range(0.0f, 1.0f)]
    private float smoothSpeed = 0.125f;

    private Vector2 _input;

    private void Start()
    {
        // Turn up timescale so the physics are faster. This means the gameplay is more fast-paced and fun!!
        Time.timeScale = 3.0f;
    }

    private void Update()
    {
        // Get input every frame
        _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        // Apply physics on physics updates
        Quaternion targetRotation = Quaternion.Euler(new Vector3(_input.y, 0, -_input.x) * tiltAmount);
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            smoothSpeed);
    }
}