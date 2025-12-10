using UnityEngine;

public class RecoveryState_Melee : EnemyState
{
    private Enemy_Melee enemy;

    public RecoveryState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;

        if (enemy == null)
            Debug.LogError("IdleState_Melee: enemyBase is not of type Enemy_Melee");
    }

    public override void Enter()
    {
        base.Enter();

        enemy.agent.isStopped = true;
    }

    public override void Update()
    {
        base.Update();

        enemy.transform.rotation = enemy.FaceTarget(enemy.player.position);

        if (triggerCalled)
        {
            if (enemy.PlayerInAttackRange())
                stateMachine.ChangeState(enemy.attackState);
            else
                stateMachine.ChangeState(enemy.chaseState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
