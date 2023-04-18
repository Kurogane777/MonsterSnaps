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
    [SerializeField] GameObject PhotoSystem;
    [SerializeField] GameObject ProjectileSystem;
    [SerializeField] PlayerHealth healthSystem;

    [Space]
    [SerializeField] FirstPersonMovemnt _fPMove;
    [SerializeField] SecondPersonMovemnt _sPMove;
    [SerializeField] ThirdPersonMovemnt _tPMove;
    [Space]
    [SerializeField] float spawnRadius = 10;
    public static PlayerController main;
    Vector3 knockback;
    CharacterController CC;
    public enum State
    { movement, damage, croutch, photo, projectile }

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
        }
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
    void MovementFunction()
    {
        _fPMove.enabled = false;
        _sPMove.enabled = false;
        _tPMove.enabled = true;

        PhotoSystem.SetActive(false);
        ProjectileSystem.SetActive(false);
        firstViewCamera.SetActive(false);
        thirdViewCamera.SetActive(true);

        // Change States
        if (Input.GetKeyDown(KeyCode.R)) { currentState = State.photo; }
        if (Input.GetKeyDown(KeyCode.E)) { currentState = State.projectile; }
    }

    void CroutchFunction()
    {
        AreaFunction();


    }

    public void DamagePlayerFunction()
    {
        healthSystem.Damage();
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

    void PhotoFunction()
    {
        _fPMove.enabled = true;
        _sPMove.enabled = false;
        _tPMove.enabled = false;

        PhotoSystem.SetActive(true);
        ProjectileSystem.SetActive(false);
        firstViewCamera.SetActive(true);
        thirdViewCamera.SetActive(false);

        // Change States
        if (Input.GetKeyDown(KeyCode.R)) { currentState = State.movement; }
        if (Input.GetKeyDown(KeyCode.E)) { currentState = State.projectile; }
    }

    void ProjectileFunction()
    {
        _fPMove.enabled = true;
        _sPMove.enabled = false;
        _tPMove.enabled = false;

        PhotoSystem.SetActive(false);
        ProjectileSystem.SetActive(true);
        firstViewCamera.SetActive(true);
        thirdViewCamera.SetActive(false);

        if (Input.GetKeyDown(KeyCode.E)) { currentState = State.movement; }
        if (Input.GetKeyDown(KeyCode.R)) { currentState = State.photo; }
    }
}
