using UnityEngine;

public class DeadState_Melee : EnemyState
{
    Enemy_Melee enemy;
    private Enemy_Ragdoll ragdoll;

    // private bool interactionDisabled;

    public DeadState_Melee(Enemy enemyBase, EnemyStateMachine stateMachine, string animBoolName) : base(enemyBase, stateMachine, animBoolName)
    {
        enemy = enemyBase as Enemy_Melee;

        if (enemy == null)
            Debug.LogError("DeadState_Melee: enemyBase is not of type Enemy_Melee");

        ragdoll = enemy.GetComponent<Enemy_Ragdoll>();
    }

    public override void Enter()
    {
        base.Enter();

        // interactionDisabled = false;

        enemy.anim.enabled = false;
        enemy.agent.isStopped = true;

        ragdoll.RagdollActive(true);

        stateTimer = 1.5f;
    }

    public override void Update()
    {
        base.Update();

        // if (stateTimer < 0 && interactionDisabled == false)
        // {
        //     interactionDisabled = true;
        //     ragdoll.RagdollActive(false);
        //     ragdoll.CollidersActive(false);
        // }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
