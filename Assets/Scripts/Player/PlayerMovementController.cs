using UnityEngine;

/// <summary>
/// Responsible only for player's movement
/// </summary>
public class PlayerMovementController : MonoBehaviour
{
    public float movementSpeed = 10f;
    public float damping = 10f;
    public float rotationSpeed = 15f;
    [SerializeField] private Animator animator;

    private Camera mainCamera;
    private Rigidbody rb;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(horizontalInput, 0f, verticalInput);
        movement.Normalize();

        if (movement != Vector3.zero)
        {
            rb.velocity = movement * movementSpeed;
            animator.SetBool("Run", true);
        }
        else
        {
            rb.velocity = Vector3.zero;
            animator.SetBool("Run", false);
        }

        Vector3 currentVelocity = rb.velocity;
        Vector3 oppositeForce = -currentVelocity * damping;
        rb.AddForce(oppositeForce, ForceMode.Acceleration);

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 direction = hit.point - transform.position;
            direction.y = 0f;

            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}