using UnityEngine;
using Zenject;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Joystick joystick;
    private Rigidbody rigidbody;
    private Vector3 _movement;

    [Inject]
    public void Construct(Joystick joystick)
    {
        this.joystick = joystick;
    }

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 inputDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical).normalized;

        Vector3 moveDirection = transform.TransformDirection(inputDirection);

        _movement = new Vector3(moveDirection.x * moveSpeed, rigidbody.velocity.y, moveDirection.z * moveSpeed);
        rigidbody.velocity = _movement;
    }
}