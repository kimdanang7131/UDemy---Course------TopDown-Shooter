using UnityEngine;

[CreateAssetMenu(menuName = "Weapon System/Weapon Data", fileName = "New Weapon Data")]
public class Weapon_Data : ScriptableObject
{
    public string weaponName;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Header("Regular shot")]
    public ShootType shootType;
    public int bulletsPerShot = 1;
    public float fireRate;

    [Header("Burst shot")]
    public bool burstAvailable;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;

    [Header("Weapon Spread")]
    public float baseSpread;
    public float maxSpread;

    public float spreadIncreaseRate = 0.15f;

    [Header("Weapon generics")]
    public WeaponType weaponType;
    [Range(1, 3)]
    public float reloadSpeed = 1f;
    [Range(1, 3)]
    public float equipmentSpeed = 1f;
    [Range(4, 8)]
    public float gunDistance = 4f;
    [Range(4, 8)]
    public float cameraDistance = 4f;
}
