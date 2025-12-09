using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weapon);
    }
}
