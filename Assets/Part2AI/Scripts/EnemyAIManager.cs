using UnityEngine;
//Add the AI library
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    //The agent that controls the navigation of this enemy
    public NavMeshAgent agent;

    ////State
    //The state that this enemy is currently in
    private EnemyAIBaseState currentState;
    //The idle state for this enemy
    private EnemyAIIdleState idleState = new();
    //The frolic state for this enemy
    private EnemyAIFrolicState frolicState = new();
    //The dead state for this enemy
    private EnemyAIDeadState deadState = new();

    //Blackboard
    public bool isDead = false;
    public bool reachedDestination = false;
    public bool isBored = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set the current state to frolic
        currentState = frolicState;
        //Tell the current state to enter
        currentState.OnEnter(this);
    }

    // Update is called once per frame
    void Update()
    {
        //On update the call the update function of the current state
        currentState.OnUpdate(this);
    }
}
