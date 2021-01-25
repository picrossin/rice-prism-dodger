using System;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] [Range(0.0f, 50.0f)] private float speed = 1.0f;
    [SerializeField] private string finishTag = "Finish";

    private Vector2 _playerInput;
    public Vector2 PlayerInput => _playerInput;

    private Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = 0.0f;
    }

    private void Update()
    {
        _playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3 (_playerInput.x, 0, _playerInput.y);
        _rigidbody.AddForce(movement * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(finishTag))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}