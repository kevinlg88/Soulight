using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float acceleration = 10f;
    [SerializeField] float rotationSpeed = 10f;

    InputSystem_Actions inputActions;
    Rigidbody rb;
    Animator animator;
    Vector3 currentVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        inputActions = new InputSystem_Actions();
        AssignInputs();
    }

    private void AssignInputs()
    {
        inputActions.Player.Enable();
        inputActions.Player.Attack.performed += _ => Attack();
    }

    private void FixedUpdate()
    {
        if (inputActions == null) return;

        Vector2 inputDir = inputActions.Player.Move.ReadValue<Vector2>();
        Move(inputDir);
    }

    private void Move(Vector2 direction)
    {
        Vector3 targetVelocity = new Vector3(direction.x, 0, direction.y) * moveSpeed;

        currentVelocity = Vector3.Lerp(currentVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        rb.linearVelocity = currentVelocity;

        animator.SetBool("isMoving", direction != Vector2.zero);

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.y));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking");
    }
}
