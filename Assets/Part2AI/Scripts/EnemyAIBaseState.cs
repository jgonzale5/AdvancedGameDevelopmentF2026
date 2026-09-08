using UnityEngine;

public abstract class EnemyAIBaseState
{
    //The abstract function for when we enter this state
    public abstract void OnEnter(EnemyAIManager manager);
    //The abstract function for when we exit this state, before entering the next one
    public abstract void OnExit(EnemyAIManager manager);
    //A function to be called on update on the active state
    public abstract void OnUpdate(EnemyAIManager manager);
}
