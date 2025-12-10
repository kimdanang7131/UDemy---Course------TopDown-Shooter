using UnityEngine;

public class ChaseState_Melee : EnemyState
{
    private Enemy_Melee enemy;
    private float lastTimeUpdateDestination;

    public ChaseState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;

        if (enemy == null)
            Debug.LogError("ChaseState_Melee: enemyBase is not of type Enemy_Melee");
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.speed = enemy.chaseSpeed;
        enemy.agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.PlayerInAttackRange())
            stateMachine.ChangeState(enemy.attackState);


        enemy.transform.rotation = enemy.FaceTarget(GetNextPathPoint());

        if (CanUpdateDestinaion())
            enemy.agent.destination = enemy.player.position;

    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanUpdateDestinaion()
    {
        if (Time.time > lastTimeUpdateDestination + .25f)
        {
            lastTimeUpdateDestination = Time.time;
            return true;
        }

        return false;
    }
}
