using UnityEngine;
using VInspector.Libs;

public class PlayerMovement : MonoBehaviour
{
    private PlayerControls controls;
    private CharacterController characterController;

    public float walkSpeed;
    public Vector3 movementDirection;

    private Vector2 moveInput;
    private Vector2 aimInput;

    void Awake()
    {
        controls = new PlayerControls();

    }

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        movementDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (movementDirection.magnitude > 0.1f)
        {
            characterController.Move(movementDirection * Time.deltaTime * walkSpeed);
        }
    }

    private void Shoot()
    {
        Debug.Log("SHOOT");
    }

    void OnEnable()
    {
        controls.Enable();

        controls.Character.Fire.performed += context => Shoot();

        controls.Character.Movement.performed += context => moveInput = context.ReadValue<Vector2>();
        controls.Character.Movement.canceled += context => moveInput = Vector2.zero;

        controls.Character.Aim.performed += context => aimInput = context.ReadValue<Vector2>();
        controls.Character.Aim.canceled += context => moveInput = Vector2.zero;
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
