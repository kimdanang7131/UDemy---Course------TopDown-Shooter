using Unity.VisualScripting;
using UnityEngine;

public class BattleState_Range : EnemyState
{
    private Enemy_Range enemy;

    private float lastTimeShot = -10;
    private int bulletsShot = 0;

    private int bulletsPerAttack;
    private float weaponCooldown;


    private float coverCheckTimer = .5f;

    public BattleState_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();

        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();

        enemy.visuals.EnableIK(true, true);
        enemy.agent.isStopped = true;
    }

    public override void Update()
    {
        base.Update();

        ChangeCoverIfShould();

        enemy.FaceTarget(enemy.player.position);

        if (WeaponOutOfBullets())
        {
            if (WeaponOnCooldown())
                AttemptToResetWeapon();

            return;
        }

        if (CanShoot())
        {
            Shoot();
        }
    }

    private void ChangeCoverIfShould()
    {
        coverCheckTimer -= Time.deltaTime;

        if (coverCheckTimer < 0)
        {
            coverCheckTimer = .5f;

            if (IsPlayerInClearSight())
            {
                if (enemy.CanGetCover())
                    stateMachine.ChangeState(enemy.runToCoverState);
            }
        }

    }
    #region Cover system region

    private bool IsPlayerInClearSight()
    {
        Vector3 enemyEyesPos = enemy.transform.position + new Vector3(0, 0.5f, 0);
        Vector3 playerTargetPos = enemy.player.transform.position + new Vector3(0, 0.5f, 0);
        Vector3 directionToPlayer = (playerTargetPos - enemyEyesPos).normalized;
        float maxDistance = Vector3.Distance(enemyEyesPos, playerTargetPos);

        if (Physics.Raycast(enemyEyesPos, directionToPlayer, out RaycastHit hit, maxDistance))
        {
            return hit.collider.gameObject.GetComponentInParent<Player>() != null;

        }

        Debug.Log("No Hit");
        return false;
    }

    #endregion


    #region Weapon region
    // 초기화
    private void AttemptToResetWeapon()
    {
        bulletsShot = 0;

        bulletsPerAttack = enemy.weaponData.GetBulletsPerAttack();
        weaponCooldown = enemy.weaponData.GetWeaponCooldown();
    }

    private bool WeaponOnCooldown() => Time.time > lastTimeShot + weaponCooldown;

    private bool WeaponOutOfBullets() => bulletsShot >= bulletsPerAttack;

    private bool CanShoot() => Time.time > lastTimeShot + (1 / enemy.weaponData.fireRate);

    private void Shoot()
    {
        enemy.FireSingleBullet();
        lastTimeShot = Time.time;
        bulletsShot++;
    }

    public override void Exit()
    {
        base.Exit();
        enemy.visuals.EnableIK(false, false);
        enemy.agent.isStopped = false;
    }

    #endregion
}

