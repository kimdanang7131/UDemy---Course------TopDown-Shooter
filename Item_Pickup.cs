using UnityEngine;

public class Item_Pickup : MonoBehaviour
{
    [SerializeField] private Weapon_Data weaponData;

    void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerWeaponController>()?.PickupWeapon(weaponData);
    }
}
