using UnityEngine;

public class AdvancePlayer_Range : EnemyState
{
    private Enemy_Range enemy;
    private Vector3 playerPos;

    public AdvancePlayer_Range(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Range;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.agent.isStopped = false;
        enemy.agent.speed = enemy.advanceSpeed;

        enemy.visuals.EnableIK(true, false);
    }

    public override void Update()
    {
        base.Update();

        playerPos = enemy.player.transform.position;

        enemy.agent.SetDestination(playerPos);
        enemy.FaceTarget(GetNextPathPoint());

        if (Utility.DistanceToTarget(enemy.transform.position, playerPos) < enemy.advanceStoppingDistance)
        {
            stateMachine.ChangeState(enemy.battleState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

}
