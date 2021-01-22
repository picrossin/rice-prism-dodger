using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Obstruction : MonoBehaviour
{
    private enum Movement {Horizontal, Vertical, Rotate};
    
    [SerializeField] private Color color = Color.green;
    [SerializeField] private Movement movementType = Movement.Horizontal;
    [SerializeField] private float speed = 5f;
    [SerializeField] private string wallTag = "Wall";
    [SerializeField] private float rotationRadius = 1.0f;

    private GameObject _rotationParent;
    private Vector3 _initalPosition;
    private Vector3 _movement;

    private void Start()
    {
        _initalPosition = transform.localPosition;
        
        // Set base color
        Material material = GetComponent<Renderer>().material;
        material.SetColor("_BaseColor", color);
        
        // Do some weird math to calculate and set the emission color to make obstruction glow
        float H, S, V;
        Color.RGBToHSV(color, out H, out S, out V);
        Color emissionColor = Color.HSVToRGB(H, S, V / 2.0f);
        emissionColor *= Mathf.Pow(2.0F, 1.25f); // The power value is the intensity of the HDR color
        material.SetColor("_EmissionColor", emissionColor);
        
        // Initial movement setup
        switch (movementType)
        {
            case Movement.Horizontal:
                _movement = Vector3.right;
                break;
            case Movement.Vertical:
                _movement = Vector3.forward;
                break;
            case Movement.Rotate:
                _rotationParent = new GameObject("Rotating Obstruction");
                _rotationParent.transform.position = Vector3.back * rotationRadius + transform.position;
                _rotationParent.transform.parent = transform.parent;
                transform.parent = _rotationParent.transform;
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
                    new Vector3(_initalPosition.x, _initalPosition.y, transform.localPosition.z);
                break;
            case Movement.Rotate:
                transform.localPosition =                     
                    new Vector3(transform.localPosition.x, 0, transform.localPosition.z);
                transform.parent.transform.localRotation = 
                    Quaternion.Euler(Vector3.up * (transform.parent.transform.localRotation.eulerAngles.y + speed));
                break;
        }

        // Scale the movement speed down by 100 so "speed" can be prettier numbers 
        if (movementType != Movement.Rotate)
        {
            transform.localPosition += _movement / 100 * speed;
        }
    }
    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            _movement *= -1;
        }
    }
}
