using UnityEngine;

public class ThirdPersonMovemnt : MonoBehaviour
{
    // Walking
    [Header("Walking")]
    public float walkSpeed = 5f;
    public float walkRotationSpeed = 100f;

    // Running
    [Header("Running")]
    public float runSpeed = 10f;
    public float runRotationSpeed = 200f;

    // Jumping
    [Header("Jumping")]
    public float jumpSpeed;

    [HideInInspector] public CharacterController characterController;
    private float ySpeed;
    private float originalStepOffset;

    [Header("Test")]
    [HideInInspector] public float currentSpeed;
    [HideInInspector] public float currentRotationSpeed;

    [HideInInspector] public Vector3 movementDirection;

    [HideInInspector] public TPDash dashScript;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        originalStepOffset = characterController.stepOffset;

        dashScript = GetComponent<TPDash>();
    }

    private void Update()
    {
        InputSystem();

        // Dash Function
        if (Input.GetMouseButtonDown(0)) { dashScript.Dfunction(); }
    }

    void InputSystem()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        MovementSystem(movementDirection);
        ChangeSpeed();
    }

    void ChangeSpeed()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = runSpeed;
            currentRotationSpeed = runRotationSpeed;
        }
        else
        {
            currentSpeed = walkSpeed;
            currentRotationSpeed = walkRotationSpeed;
        }
    }

    void MovementSystem(Vector3 movementDirection)
    {
        #region Move System
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * currentSpeed;

        var cameraTransform = Camera.main.transform;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (characterController.isGrounded)
        {
            characterController.stepOffset = originalStepOffset;
            ySpeed = -0.5f;

            if (Input.GetButtonDown("Jump"))
            {
                ySpeed = jumpSpeed;
            }
        }
        else
        {
            characterController.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * magnitude;
        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, currentRotationSpeed * Time.deltaTime);
        }
        #endregion
    }

    private Vector3 AdjustVelocityToSlope(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, 0.2f))
        {
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if (adjustedVelocity.y < 0)
            {
                return adjustedVelocity;
            }
        }

        return velocity;
    }
}
