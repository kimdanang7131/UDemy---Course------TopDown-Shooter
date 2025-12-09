using System;
using JetBrains.Annotations;
using UnityEngine;

public enum WeaponType
{
    Pistol,
    Revolver,
    AutoRifle,
    Shotgun,
    Rifle
}

public enum ShootType
{
    Single,
    Auto,
}

[Serializable] // 클래스를 인스펙터에서 볼 수 있게 함
public class Weapon
{
    public WeaponType weaponType;

    [Header("Shooting specifics")]
    public ShootType shootType;
    public int bulletsPerShot;
    public float defaultFireRate;
    public float fireRate = 1; // 발사 속도
    private float lastShootTime;

    [Header("Burst fire")]
    public bool burstAvailable;
    public bool burstActive;

    public int burstBulletsPerShot;
    public float burstFireRate;
    public float burstFireDelay = .1f;

    [Header("Magazine details")]
    public int bulletsInMagazine;
    public int magazineCapacity;
    public int totalReserveAmmo;

    [Range(1, 3)]
    public float reloadSpeed = 1f; // 재장전 속도 - 애니메이션
    [Range(1, 3)]
    public float equipmentSpeed = 1f; // 장비 속도 - 애니메이션
    [Range(2, 12)]
    public float gunDistance = 4f;
    [Range(3, 8)]
    public float cameraDistance = 6;

    [Header("Spread")]
    public float baseSpread = 1;
    public float maximumSpread = 3;
    private float currentSpread = 2;

    public float spreadIncreaseRate = 0.15f;

    private float lastSpreadUpdateTime;
    private float spreadCooldown = 1;

    #region Spread Methods

    public Vector3 ApplySpread(Vector3 originalDirection)
    {
        UpdateSpread();

        float randomizedValue = UnityEngine.Random.Range(-currentSpread, currentSpread);

        Quaternion spreadRotation = Quaternion.Euler(randomizedValue, randomizedValue, randomizedValue);

        return spreadRotation * originalDirection;
    }

    private void UpdateSpread()
    {
        if (Time.time > lastSpreadUpdateTime + spreadCooldown)
            currentSpread = baseSpread;
        else
            IncreaseSpread();

        lastSpreadUpdateTime = Time.time;
    }

    private void IncreaseSpread()
    {
        currentSpread = Mathf.Clamp(currentSpread + spreadIncreaseRate, baseSpread, maximumSpread);
    }

    #endregion

    #region Burst Methods

    public bool BurstActivated()
    {
        if (weaponType == WeaponType.Shotgun)
        {
            burstFireDelay = 0;
            return true;
        }

        return false;
    }

    public void ToggleBurst()
    {
        if (burstAvailable == false)
            return;

        burstActive = !burstActive;

        if (burstActive)
        {
            bulletsPerShot = burstBulletsPerShot;
            fireRate = burstFireRate;
        }
        else
        {
            bulletsPerShot = 1;
            fireRate = defaultFireRate;
        }
    }

    #endregion

    public bool CanShoot() => HaveEnoughBullets() && ReadyToFire();

    private bool ReadyToFire()
    {
        if (Time.time > lastShootTime + (1 / fireRate))
        {
            lastShootTime = Time.time;
            return true;
        }

        return false;
    }

    #region Reload Methods

    private bool HaveEnoughBullets() => bulletsInMagazine > 0;

    public bool CanReload()
    {
        // 탄창 가득 찼는지 확인
        if (bulletsInMagazine == magazineCapacity)
            return false;

        // 남은 탄약이 있는지 확인
        if (totalReserveAmmo > 0)
            return true;

        return false;
    }

    public void RefillBullets()
    {
        // 남아있는 총알 남겨둘시
        // totalReserveAmmo += bulletsInMagazine;

        int bulletToReload = magazineCapacity;

        // 충전할 탄약보다, 남은 탄약이 적을 때 -> totalReserveAmmo 남아있는 총 탄약량
        if (bulletToReload > totalReserveAmmo)
            bulletToReload = totalReserveAmmo;

        totalReserveAmmo -= bulletToReload;
        bulletsInMagazine = bulletToReload;

        if (totalReserveAmmo < 0)
            totalReserveAmmo = 0;
    }
    #endregion
}
