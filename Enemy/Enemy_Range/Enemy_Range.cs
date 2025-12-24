using System.Collections.Generic;
using UnityEngine;


public enum CoverPerk { Unavailable, CanTakeCover, CanTakeAndChangeCover }

public class Enemy_Range : Enemy
{
    [Header("Perk")]
    public CoverPerk coverPerk;
    public CoverPerk lastCoverPerk { get; private set; }

    [Header("Advance perk")]
    public float advanceSpeed;
    public float advanceStoppingDistance;

    [Header("Cover System")]
    public float safeDistance;
    public CoverPoint currentCoverPoint;

    [Header("Weapon details")]
    public Enemy_RangeWeaponType weaponType;
    public Enemy_RangeWeaponData weaponData;

    [Space]
    public Transform weaponHolder;
    public Transform gunPoint;
    public GameObject bulletPrefab;

    [Header("Aim details")]
    public float slowAim = 4;
    public float fastAim = 20;
    public Transform aim;
    public Transform playersBody;
    public LayerMask whatToIgnore;

    [SerializeField] List<Enemy_RangeWeaponData> availableWeaponData;

    public IdleState_Range idleState { get; private set; }
    public MoveState_Range moveState { get; private set; }
    public BattleState_Range battleState { get; private set; }
    public RunToCoverState_Range runToCoverState { get; private set; }
    public AdvancePlayer_Range advancePlayerState { get; private set; }

    public LayerMask playerLayerMask;
    protected override void Awake()
    {
        base.Awake();

        idleState = new IdleState_Range(this, stateMachine, "Idle");
        moveState = new MoveState_Range(this, stateMachine, "Move");
        battleState = new BattleState_Range(this, stateMachine, "Battle");
        runToCoverState = new RunToCoverState_Range(this, stateMachine, "Run");
        advancePlayerState = new AdvancePlayer_Range(this, stateMachine, "Advance");
    }

    protected override void Start()
    {
        base.Start();

        playersBody = player.GetComponent<Player>().playerbody;
        aim.parent = null;

        stateMachine.Initialize(idleState);
        visuals.SetupLook();
        SetupWeapon();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
    }

    #region Cover System

    public bool CanGetCover()
    {
        if (coverPerk == CoverPerk.Unavailable)
            return false;

        currentCoverPoint = AttemptToFindCover()?.GetComponent<CoverPoint>();

        if (currentCoverPoint != null)
        {
            currentCoverPoint.SetOccupied(true);
            lastCoverPerk = coverPerk;
            coverPerk = CoverPerk.Unavailable;
            return true;
        }

        Debug.Log("No valid cover found");
        return false;
    }

    private Transform AttemptToFindCover()
    {
        List<CoverPoint> collectedCoverPoints = new List<CoverPoint>();

        foreach (Cover cover in CollectNearByCovers())
        {
            collectedCoverPoints.AddRange(cover.GetValidCoverPoints(transform));
        }

        CoverPoint furthestCoverPoint = null;
        float furtherestDistance = 0;

        foreach (CoverPoint coverPoint in collectedCoverPoints)
        {
            float currentDistance = Utility.DistanceToTarget(transform.position, coverPoint.transform.position);
            if (currentDistance > furtherestDistance)
            {
                furtherestDistance = currentDistance;
                furthestCoverPoint = coverPoint;
            }
        }

        if (furthestCoverPoint != null)
        {
            currentCoverPoint?.SetOccupied(false);
            return furthestCoverPoint.transform;
        }
        return null;
    }

    private List<Cover> CollectNearByCovers()
    {
        float coverRadiusCheck = 30;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, coverRadiusCheck);
        List<Cover> collectedCovers = new List<Cover>();

        foreach (Collider collider in hitColliders)
        {
            Cover cover = collider.gameObject.GetComponent<Cover>();
            if (cover != null && collectedCovers.Contains(cover) == false)
                collectedCovers.Add(cover);
        }

        return collectedCovers;
    }

    #endregion

    public void FireSingleBullet()
    {
        anim.SetTrigger("Shoot");

        Vector3 bulletsDirection = (aim.position - gunPoint.position).normalized;

        GameObject newBullet = ObjectPool.instance.GetObject(bulletPrefab);
        newBullet.transform.position = gunPoint.position;
        newBullet.transform.rotation = Quaternion.LookRotation(gunPoint.forward);

        newBullet.GetComponent<Enemy_Bullet>().BulletSetup(100, 50);

        Rigidbody rbNewBullet = newBullet.GetComponent<Rigidbody>();

        Vector3 bulletDirectionWithSpread = weaponData.ApplyWeaponSpread(bulletsDirection);

        rbNewBullet.mass = 20 / weaponData.bulletSpeed;
        rbNewBullet.linearVelocity = bulletDirectionWithSpread * weaponData.bulletSpeed;
    }

    public override void EnterBattleMode()
    {
        if (inBattleMode)
            return;

        base.EnterBattleMode();

        if (CanGetCover())
            stateMachine.ChangeState(runToCoverState);
        else
            stateMachine.ChangeState(battleState);
    }

    private void SetupWeapon()
    {
        List<Enemy_RangeWeaponData> filteredData = new List<Enemy_RangeWeaponData>();

        foreach (var weaponData in availableWeaponData)
        {
            if (weaponData.weaponType == weaponType)
                filteredData.Add(weaponData);
        }

        if (filteredData.Count > 0)
        {
            int random = Random.Range(0, filteredData.Count);
            weaponData = filteredData[random];
        }
        else
            Debug.LogWarning("No weapon data available for weapon type: " + weaponType);


        gunPoint = visuals.currentWeaponModel.GetComponent<Enemy_RangeWeaponModel>().gunPoint;
    }


    #region Enemy's Aim
    public void UpdateAimPosition()
    {
        float aimSpeed = AimOnPlayer() ? fastAim : slowAim;
        aim.position = Vector3.MoveTowards(aim.position, playersBody.position, aimSpeed * Time.deltaTime);
    }

    public bool AimOnPlayer()
    {
        float distanceAimToPlayer = Utility.DistanceToTarget(aim.position, playersBody.position);
        return distanceAimToPlayer < 2;
    }


    public bool IsSeenPlayer()
    {
        Vector3 myPosition = transform.position + Vector3.up;
        Vector3 directionToPlayer = playersBody.position - myPosition;

        if (Physics.Raycast(myPosition, directionToPlayer, out RaycastHit hit, Mathf.Infinity, ~whatToIgnore))
        {
            if (hit.transform == player)
            {
                Debug.Log(hit.transform.gameObject.name);
                UpdateAimPosition();
                return true;
            }
        }

        return false;
    }

    #endregion

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        // Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + transform.forward * 1);

        // Gizmos.color = Color.yellow;
        // Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;
        // Gizmos.DrawLine(transform.position + Vector3.up, transform.position + Vector3.up + directionToPlayer * 100);
    }
}



// float shortestDistance = float.MaxValue;

// foreach (CoverPoint coverPoint in collectedCoverPoints)
// {
//     float currentDistance = Utility.DistanceToTarget(transform.position, coverPoint.transform.position);
//     if (currentDistance < shortestDistance)
//     {
//         shortestDistance = currentDistance;
//         closestCoverPoint = coverPoint;
//     }
// }