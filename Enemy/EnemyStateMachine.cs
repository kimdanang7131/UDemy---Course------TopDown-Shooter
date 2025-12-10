using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currentState { get; private set; }

    // 게임 시작시 기본 상태 설정
    public void Initialize(EnemyState newState)
    {
        currentState = newState;
        currentState.Enter();
    }

    // 유효상태머신은 항상 상태를 하나 가지고있어야 하므로 
    public void ChangeState(EnemyState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
