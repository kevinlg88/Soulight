using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float acceleration = 10f;    // quanto maior, mais rápido atinge targetVelocity
    [SerializeField] float rotationSpeed = 10f;    // suavidade da rotação
    [SerializeField] bool movementRelativeToPlayer = true; // <--- sua necessidade
    [SerializeField] bool rotateToMovement = true;

    InputSystem_Actions inputActions;
    Rigidbody rb;
    Animator animator;

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
        Vector3 localInput = new Vector3(direction.x, 0f, direction.y);

        Vector3 worldDirection = movementRelativeToPlayer
            ? transform.TransformDirection(localInput)         
            : new Vector3(direction.x, 0f, direction.y);      

        float inputMagnitude = Mathf.Clamp01(localInput.magnitude);
        if (worldDirection.sqrMagnitude > 0.0001f)
            worldDirection = worldDirection.normalized * inputMagnitude;

        Vector3 targetVelocity = worldDirection * moveSpeed;

        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);

        animator.SetBool("isMoving", inputMagnitude > 0.01f);

        if (rotateToMovement && worldDirection.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(worldDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        }
    }

    private void Attack()
    {
        Debug.Log("Attacking");
    }
}
