using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] GameObject firstViewCamera;
    [SerializeField] GameObject secondViewCamera;
    [SerializeField] GameObject thirdViewCamera;

    [Header("Other Functions")]
    [SerializeField] InputController inputC;
    [SerializeField] GameObject PhotoSystem;
    [SerializeField] GameObject ProjectileSystem;
    [SerializeField] PlayerHealth healthSystem;

    [Space]
    [SerializeField] float spawnRadius = 10;
    public static PlayerController main;
    Vector3 knockback;
    CharacterController CC;
    public enum State
    { movement, damage, croutch, photo, projectile, none }

    [Space]
    public State currentState;

    public enum Area
    { hideable, nonhidable }

    public Area area;

    void Awake()
    {
        currentState = State.movement;
        if (main) Destroy(gameObject);
        main = this;
        CC = GetComponent<CharacterController>();

        // ThirdPersonMovement
        originalStepOffset = CC.stepOffset;
        // Dash
        currentReload = reloadTime;
    }

    void Update()
    {
        // Movement Control State
        switch (currentState)
        {
            case State.movement:
                MovementFunction();
                break;

            case State.damage:
                DamagePlayerFunction();
                break;

            case State.croutch:
                break;

            case State.photo:
                PhotoFunction();
                break;

            case State.projectile:
                ProjectileFunction();
                break;

            case State.none:
                break;
        }

        KnockbackFunction();
        ReloadDash();
        FootStepsFunction();
    }

    #region Damged&Knockback
    public void DamagePlayerFunction()
    {
        healthSystem.Damage();
    }

    void KnockbackFunction()
    {
        if (knockback.magnitude > 0.05)
        {
            CC.Move(knockback);
            knockback *= 0.95f;
        }
    }

    public void ApplyKnockback(Vector3 amount)
    {
        knockback += amount;
    }
    #endregion

    #region StateChange
    void MovementFunction()
    {
        TPMovement();

        PhotoSystem.SetActive(false);
        ProjectileSystem.SetActive(false);
        firstViewCamera.SetActive(false);
        thirdViewCamera.SetActive(true);

        // Change States
        if (Input.GetKeyDown(inputC.CSCameraPhoto)) { currentState = State.photo; }
        if (Input.GetKeyDown(inputC.CSProjectile)) { currentState = State.projectile; }
    }

    void PhotoFunction()
    {
        FPMovement();

        PhotoSystem.SetActive(true);
        ProjectileSystem.SetActive(false);
        firstViewCamera.SetActive(true);
        thirdViewCamera.SetActive(false);

        // Change States
        if (Input.GetKeyDown(inputC.CSCameraPhoto)) { currentState = State.movement; }
        if (Input.GetKeyDown(inputC.CSProjectile)) { currentState = State.projectile; }
    }

    void ProjectileFunction()
    {
        FPMovement();

        PhotoSystem.SetActive(false);
        ProjectileSystem.SetActive(true);
        firstViewCamera.SetActive(true);
        thirdViewCamera.SetActive(false);

        if (Input.GetKeyDown(inputC.CSProjectile)) { currentState = State.movement; }
        if (Input.GetKeyDown(inputC.CSCameraPhoto)) { currentState = State.photo; }
    }
    #endregion

    #region DetectionMonster
    void CroutchFunction()
    {
        AreaFunction();
    }

    public int GetSurroundingMonsters()
    {
        int count = 0;
        var cols = Physics.OverlapSphere(transform.position, spawnRadius);
        foreach (var c in cols)
        {
            if (c.CompareTag("Enemy"))
            {
                count++;
            }
        }
        return 0;
    }

    void AreaFunction()
    {
        switch (area)
        {
            case Area.hideable:
                break;
            case Area.nonhidable:
                break;
        }
    }
    #endregion

    #region FirstPersonMovement
    [Header("FirstPersonMovement")]
    public float speed = 12f;
    public float gravity = -9.81f;
    Vector3 velocity;

    void FPMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        CC.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;

        CC.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region ThirdPersonMovement
    [Header("ThirdPersonMovement")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 100f;

    private float ySpeed;
    private float originalStepOffset;

    float currentSpeed;
    float currentRotationSpeed;

    Vector3 movementDirection;

    void TPMovement()
    {
        InputSystem();

        // Dash Function
        if (Input.GetKeyDown(inputC.dash)) { Dfunction(); }
    }

    void InputSystem()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        movementDirection = new Vector3(horizontalInput, 0, verticalInput);

        MovementSystem(movementDirection);

        currentSpeed = moveSpeed;
        currentRotationSpeed = rotationSpeed;
    }

    void MovementSystem(Vector3 movementDirection)
    {
        #region Move System
        float magnitude = Mathf.Clamp01(movementDirection.magnitude) * currentSpeed;

        var cameraTransform = Camera.main.transform;
        movementDirection = Quaternion.AngleAxis(cameraTransform.rotation.eulerAngles.y, Vector3.up) * movementDirection;

        movementDirection.Normalize();

        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (CC.isGrounded)
        {
            CC.stepOffset = originalStepOffset;
            ySpeed = -0.5f;
        }
        else
        {
            CC.stepOffset = 0;
        }

        Vector3 velocity = movementDirection * magnitude;
        velocity = AdjustVelocityToSlope(velocity);
        velocity.y += ySpeed;

        CC.Move(velocity * Time.deltaTime);

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

    //----------------------------Dash----------------------------------------

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public float reloadTime = 0.5f;
    public float currentReload;
    bool boolReload;

    void ReloadDash()
    {
        if (boolReload)
        {
            currentReload -= Time.deltaTime;
        }

        if (currentReload <= 0.0f)
        {
            boolReload = false;
        }
    }

    void Dfunction()
    {
        if (!boolReload)
        {
            StartCoroutine(Dash());
            currentReload = reloadTime;
            boolReload = true;
        }
    }

    IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            CC.Move(movementDirection * dashSpeed * Time.deltaTime);

            yield return null;
        }
    }
    #endregion

    #region Footsteps
    [Header("Footsteps")]
    public GameObject footsteps;
    public GameObject nWFootsteps;
    public GameObject waterRipple;
    public bool isGrounded;

    [TagSelector]
    public string puddleTag;

    [TagSelector]
    public string groundTag;

    public enum AreaFootSteps
    { puddle, ground, none }

    public AreaFootSteps steps;

    void FootStepsFunction()
    {
        float MvX = Input.GetAxis("Horizontal");
        float MvY = Input.GetAxis("Vertical");
        Vector3 Mv = new Vector3(MvX, 0, MvY);

        RaycastHit hit;

        if (Physics.Raycast(transform.position, Vector3.down * 1.15f, out hit))
        {
            isGrounded = true;

            if (hit.transform.tag == puddleTag)
            {
                steps = AreaFootSteps.puddle;
            }
            else if (hit.transform.tag == groundTag)
            {
                steps = AreaFootSteps.ground;
            }
            else
            {
                steps = AreaFootSteps.none;
            }
        }

        if (steps == AreaFootSteps.puddle)
        {
            waterRipple.SetActive(isGrounded);
        }
        if (steps != AreaFootSteps.puddle)
        {
            waterRipple.SetActive(false);
        }
        if (steps == AreaFootSteps.ground)
        {
            if (Mv != Vector3.zero)
            {
                footsteps.SetActive(isGrounded);
                nWFootsteps.SetActive(false);
            }
            else
            {
                footsteps.SetActive(false);
                nWFootsteps.SetActive(true);
            }
        }
        if (steps != AreaFootSteps.ground)
        {
            footsteps.SetActive(false);
            nWFootsteps.SetActive(false);
        }
    }
    #endregion
}
