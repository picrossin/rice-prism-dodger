using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstruction : MonoBehaviour
{
    private enum Movement {Horizontal, Vertical, Rotate};
    
    [SerializeField] private Color color = Color.green;
    [SerializeField] private Movement movementType = Movement.Horizontal;
    [SerializeField] private float speed = 5f;
    [SerializeField] private string wallTag = "Wall";

    private Rigidbody _rigidbody;
    private Vector3 _initalPosition;
    private Vector3 _movement;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _initalPosition = transform.localPosition;
        
        // Set base color
        Material material = GetComponent<Renderer>().material;
        material.SetColor("_BaseColor", color);
        
        // Do some weird math to calculate and set the emission color
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        Color emissionColor = Color.HSVToRGB(H, S, V / 2.0f);
        float adjustedIntensity = 0.5f;
        emissionColor *= Mathf.Pow(2.0F, adjustedIntensity);
        material.SetColor("_EmissionColor", emissionColor);
        
        // Set initial movement amount
        switch (movementType)
        {
            case Movement.Horizontal:
            case Movement.Rotate:    
                _movement = Vector3.right;
                break;
            case Movement.Vertical:
                _movement = Vector3.forward;
                break;
        }
    }

    private void FixedUpdate()
    {
        // Constrain rotation
        transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, 0);
        
        // Constrain position
        switch (movementType)
        {
            case Movement.Horizontal:
                transform.localPosition = 
                    new Vector3(transform.localPosition.x, _initalPosition.y, _initalPosition.z);
                break;
            case Movement.Vertical:
                transform.localPosition = 
                    new Vector3(_initalPosition.x, transform.localPosition.y, _initalPosition.z);
                break;
        }
        
        _rigidbody.AddRelativeForce(_movement / 2, ForceMode.Impulse);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            _movement *= -1;
        }
    }
}
