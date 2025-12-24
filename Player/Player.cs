using UnityEngine;

public class Player : MonoBehaviour
{
    public Transform playerbody;

    public PlayerControls controls { get; private set; }
    public PlayerAim aim { get; private set; }
    public PlayerMovement movement { get; private set; }
    public PlayerWeaponVisuals weaponVisuals { get; private set; }
    public PlayerWeaponController weapon { get; private set; }
    public PlayerInteraction interaction { get; private set; }

    private void Awake()
    {
        controls = new PlayerControls();
        aim = GetComponent<PlayerAim>();
        movement = GetComponent<PlayerMovement>();
        weaponVisuals = GetComponent<PlayerWeaponVisuals>();
        weapon = GetComponent<PlayerWeaponController>();
        interaction = GetComponent<PlayerInteraction>();
    }

    void OnEnable()
    {
        controls.Enable();
    }

    void OnDisable()
    {
        controls.Disable();
    }
}
