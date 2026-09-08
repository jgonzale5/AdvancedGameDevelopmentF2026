using UnityEngine;

public class EnemyAIIdleState : EnemyAIBaseState
{
    //The amount of seconds this enemy will be idle before
    // automatically changing states
    //It's read only so we cannot modify it.
    private readonly float idleTime = 2f;
    //This keeps track of how long we've been idle.
    private float idleTimeCounter = 0f;

    public override void OnEnter(EnemyAIManager manager)
    {
        //When this state starts, reset the counter
        idleTimeCounter = 0f;
    }

    public override void OnExit(EnemyAIManager manager)
    {

    }

    public override void OnUpdate(EnemyAIManager manager)
    {
        //Every update, increase the counter
        idleTimeCounter += Time.deltaTime;

        //If enough time has passed, change the state to frolic
        if (idleTimeCounter >= idleTime)
        {
            manager.SetCurrentState(manager.frolicState);
        }
    }
}
