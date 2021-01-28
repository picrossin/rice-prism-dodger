using UnityEngine;

public class Obstruction : MonoBehaviour
{
    public enum Movement
    {
        Horizontal,
        Vertical,
        Rotate
    };

    [SerializeField] private Movement movementType = Movement.Horizontal;
    public Movement MovementType => movementType;

    [SerializeField] private string wallTag = "Wall";

    private Color _color = Color.green;
    private float _speed = 5f;
    private float _rotationRadius = 1.0f;

    private GameObject _rotationParent;
    private Vector3 _initalPosition;
    private Vector3 _movement;
    private float _distanceToGround;

    private void Start()
    {
        _initalPosition = transform.localPosition;
        _distanceToGround = GetComponent<Collider>().bounds.extents.y;
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
                    Quaternion.Euler(Vector3.up * (transform.parent.transform.localRotation.eulerAngles.y + _speed));
                break;
        }

        // Scale the movement speed down by 100 so "speed" can be prettier numbers 
        if (movementType != Movement.Rotate)
        {
            transform.localPosition += _movement / 100 * _speed;
        }

        if (!OnGround())
        {
            _movement *= -1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(wallTag))
        {
            _movement *= -1;
        }
    }

    public void SetData(ObstructionData data)
    {
        ColorUtility.TryParseHtmlString(data.color, out Color parsedColor);
        _color = parsedColor;
        _speed = data.speed;
        _rotationRadius = data.rotationRadius;

        SetupObstruction();
    }

    private void SetupObstruction()
    {
        // Set base color
        Material material = GetComponent<Renderer>().material;
        material.SetColor("_BaseColor", _color);

        // Do some weird math to calculate and set the emission color to make obstruction glow
        float H, S, V;
        Color.RGBToHSV(_color, out H, out S, out V);
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
                _rotationParent.transform.position = transform.position;
                _rotationParent.transform.parent = transform.parent;
                _rotationParent.transform.localPosition += Vector3.back * _rotationRadius;
                transform.parent = _rotationParent.transform;
                break;
        }
    }

    private bool OnGround()
    {
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            _distanceToGround + 0.7f);
    }
}