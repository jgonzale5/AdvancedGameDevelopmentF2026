using UnityEngine;

public class EnemyAIFrolicState : EnemyAIBaseState
{
    //The minimum distance for this enemy to consider itself 
    // "at" its target location.
    private readonly float minDistanceToTarget = 0.1f;
    //
    private readonly float randomDestinationRadius = 2f;

    public override void OnEnter(EnemyAIManager manager)
    {
        //Set a new random destination for this enemy
        manager.agent.SetDestination(new Vector3(
            Random.Range(manager.transform.position.x - randomDestinationRadius, 
                        manager.transform.position.x + randomDestinationRadius), 
            manager.transform.position.y,
            Random.Range(manager.transform.position.z - randomDestinationRadius, 
                        manager.transform.position.z + randomDestinationRadius)));
    }

    public override void OnExit(EnemyAIManager manager)
    {

    }

    public override void OnUpdate(EnemyAIManager manager)
    {
        
        //If the distance between this agent and its destination is
        // the minimum, we change to idle
        if (manager.agent.remainingDistance <= minDistanceToTarget )
        {
            manager.SetCurrentState(manager.idleState);
        }
    }
}
