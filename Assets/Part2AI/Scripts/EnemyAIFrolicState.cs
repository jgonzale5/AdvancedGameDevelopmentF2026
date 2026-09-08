using UnityEngine;

public class EnemyAIFrolicState : EnemyAIBaseState
{
    public override void OnEnter(EnemyAIManager manager)
    {
        manager.agent.SetDestination(new Vector3(
            Random.Range(manager.transform.position.x - 2, manager.transform.position.x + 2), 
            manager.transform.position.y,
            Random.Range(manager.transform.position.z - 2, manager.transform.position.z + 2)));
    }

    public override void OnExit(EnemyAIManager manager)
    {

    }

    public override void OnUpdate(EnemyAIManager manager)
    {

    }
}
