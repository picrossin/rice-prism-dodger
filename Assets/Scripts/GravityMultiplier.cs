using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class GravityMultiplier : MonoBehaviour
{
    [Tooltip("Multiplies the gravity of the rigidbody of this GameObject by the specified multiplier.")]
    [SerializeField] [Range(1.0f, 50.0f)]
    private float gravityMultiplier = 2f;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.AddForce(Physics.gravity * (gravityMultiplier - 1f), ForceMode.Acceleration);
    }
}