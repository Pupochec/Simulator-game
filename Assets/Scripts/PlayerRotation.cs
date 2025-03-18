using UnityEngine;
using Zenject;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float verticalRotationSpeed = 2f;
    [SerializeField] private float mouseSensitivity = 0.2f;

    private Transform cameraTransform;
    private Vector2 inputStartPos;
    private bool isDragging = false;
    private float verticalRotation = 0f;

    [Inject]
    public void Construct([Inject(Id = "CameraTransform")] Transform cameraTransform)
    {
        this.cameraTransform = cameraTransform;
    }

    void Awake()
    {
        cameraTransform.localEulerAngles = Vector3.zero;
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        // Touch
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.position.x >= Screen.width / 2)
            {
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        inputStartPos = touch.position;
                        isDragging = true;
                        break;

                    case TouchPhase.Moved:
                        if (isDragging)
                        {
                            Vector2 inputDelta = touch.position - inputStartPos;
                            RotatePlayer(inputDelta.x, inputDelta.y);
                            inputStartPos = touch.position;
                        }
                        break;

                    case TouchPhase.Ended:
                    case TouchPhase.Canceled:
                        isDragging = false;
                        break;
                }
            }
        }
        // Mouse
        else if (Input.GetMouseButton(0))
        {
            if (Input.mousePosition.x >= Screen.width / 2)
            {
                if (!isDragging)
                {
                    inputStartPos = Input.mousePosition;
                    isDragging = true;
                }
                else
                {
                    Vector2 inputDelta = (Vector2)Input.mousePosition - inputStartPos;
                    RotatePlayer(inputDelta.x * mouseSensitivity, inputDelta.y * mouseSensitivity);
                    inputStartPos = Input.mousePosition;
                }
            }
        }
        else
        {
            isDragging = false;
        }
    }

    private void RotatePlayer(float deltaX, float deltaY)
    {
        float horizontalRotation = deltaX * rotationSpeed * Time.deltaTime;
        transform.Rotate(0, horizontalRotation, 0);

        verticalRotation -= deltaY * verticalRotationSpeed * Time.deltaTime;
        verticalRotation = Mathf.Clamp(verticalRotation, -89f, 89f);

        cameraTransform.localEulerAngles = new Vector3(verticalRotation, 0, 0);
    }
}