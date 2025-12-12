using UnityEngine;

[CreateAssetMenu(menuName = "Enemy data/Range weapon Data", fileName = "New Weapon Data")]
public class Enemy_RangeWeaponData : ScriptableObject
{
    [Header("Weapon details")]
    public Enemy_RangeWeaponType weaponType;
    public float fireRate = 1f; // bullets per second

    public int minBulletPerAttack = 1;
    public int maxBulletPerAttack = 1;

    public float minWeaponCooldown = 2f;
    public float maxWeaponCooldown = 3f;

    [Header("Bullet details")]
    public float bulletSpeed = 20;
    public float weaponSpread = .1f;

    public int GetBulletsPerAttack() => Random.Range(minBulletPerAttack, maxBulletPerAttack);
    public float GetWeaponCooldown() => Random.Range(minWeaponCooldown, maxWeaponCooldown);

    public Vector3 ApplyWeaponSpread(Vector3 originalDirection)
    {
        float randomizedValue = UnityEngine.Random.Range(-weaponSpread, weaponSpread);
        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

}
