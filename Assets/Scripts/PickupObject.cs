using UnityEngine;
using Zenject;

public class PickupObject : MonoBehaviour
{
    [SerializeField] private float pickupSpeed = 5f;
    [SerializeField] private float throwForce = 10f;
    [SerializeField] private float maxPickupDistance = 3f;

    private Transform handPosition;
    private GameObject buttonDrop;
    private GameObject selectedObject;
    private bool isPickingUp = false;

    [Inject]
    public void Construct(GameObject ButtonDrop, [Inject(Id = "HandPosition")] Transform handPosition)
    {
        this.buttonDrop = ButtonDrop;
        this.handPosition = handPosition;
    }

    void Update()
    {
        HandleInput();

        if (isPickingUp && selectedObject != null)
        {
            MoveObjectToHand();
        }
    }

    private void HandleInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TryPickupObject(Input.mousePosition);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                TryPickupObject(touch.position);
            }
        }
    }

    private void TryPickupObject(Vector2 screenPosition)
    {
        if (selectedObject != null) return;

        Ray ray = Camera.main.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Pickable"))
            {
                float distance = Vector3.Distance(transform.position, hit.collider.transform.position);
                if (distance <= maxPickupDistance)
                {
                    selectedObject = hit.collider.gameObject;
                    isPickingUp = true;
                    buttonDrop.SetActive(true);

                    Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                    }
                }
            }
        }
    }

    private void MoveObjectToHand()
    {
        selectedObject.transform.position = Vector3.Lerp(selectedObject.transform.position, handPosition.position, pickupSpeed * Time.deltaTime);

        selectedObject.transform.rotation = Quaternion.Lerp(selectedObject.transform.rotation, handPosition.rotation, pickupSpeed * Time.deltaTime);

        if (Vector3.Distance(selectedObject.transform.position, handPosition.position) < 0.1f)
        {
            isPickingUp = false;

            selectedObject.transform.SetParent(handPosition);
        }
    }

    public void ThrowObject()
    {
        if (selectedObject == null) return;

        buttonDrop.SetActive(false);

        Rigidbody rb = selectedObject.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        selectedObject.transform.SetParent(null);

        if (rb != null)
        {
            rb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        }

        selectedObject = null;
    }
}