using UnityEngine;

public class EnemyAIEscapeState : EnemyAIBaseState
{
    //How far should the enemy try to distance themselves
    //from the player
    public readonly float escapeDistance = 10f;
    //The minimum distance for this enemy to consider itself 
    // "at" its target location.
    private readonly float minDistanceToTarget = 0.1f;

    public override void OnEnter(EnemyAIManager manager)
    {
        //Get a reference to the player in the scene
        Transform player = GameObject.
            FindGameObjectWithTag("Player").transform;

        //Get the direction to the player
        Vector3 playerDirection = player.transform.position -
            manager.agent.transform.position;
        //Flatten it so we don't worry about vertical distance
        playerDirection.y = 0;

        //Set the destination in the opposite direction to the player
        //A number of units away equal to the escapeDistance
        manager.agent.SetDestination(
            manager.agent.nextPosition - 
            playerDirection.normalized * escapeDistance
            );
    }

    public override void OnExit(EnemyAIManager manager)
    {

    }

    public override void OnUpdate(EnemyAIManager manager)
    {
        //If the remaining distance is less than the minimum
        if (manager.agent.remainingDistance <= minDistanceToTarget)
        {
            //Set the state to frolic
            manager.SetCurrentState(manager.frolicState);
        }
    }
}
