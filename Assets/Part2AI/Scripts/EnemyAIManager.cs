using UnityEngine;
//Add the AI library
using UnityEngine.AI;

public class EnemyAIManager : MonoBehaviour
{
    //The agent that controls the navigation of this enemy
    public NavMeshAgent agent;
    //A reference to the vision script that detects the player
    public EnemyVisionScript visionScript;

    ////State
    //The state that this enemy is currently in
    private EnemyAIBaseState currentState;
    //The idle state for this enemy
    public EnemyAIIdleState idleState = new();
    //The frolic state for this enemy
    public EnemyAIFrolicState frolicState = new();
    //The dead state for this enemy
    public EnemyAIDeadState deadState = new();

    //Blackboard
    public bool isDead = false;
    public bool reachedDestination = false;
    public bool isBored = false;
    public bool canSeePlayer = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set the current state to frolic
        currentState = frolicState;
        //Tell the current state to enter
        currentState.OnEnter(this);
    }

    private void OnEnable()
    {
        //If there's a vision script to be referenced, tell it to
        // call SetCanSee to keep this script up to date.
        if (visionScript != null)
            visionScript.UpdateSee += SetCanSee;
    }

    private void OnDisable()
    {
        //If there's a vision script to be referenced, tell it to
        // stop calling SetCanSee if this script is disabled
        if (visionScript != null)
            visionScript.UpdateSee -= SetCanSee;
    }

    // Update is called once per frame
    void Update()
    {
        //On update the call the update function of the current state
        currentState.OnUpdate(this);
    }

    //States call this function to change the state of the manager
    public void SetCurrentState(EnemyAIBaseState to)
    {
        //Before changing states, call the OnExit function
        currentState.OnExit(this);
        //Set the new current state
        currentState = to;
        //Execute the OnEnter function of the new current state
        currentState.OnEnter(this);
        Debug.Log("New State: " + currentState.ToString());
    }

    //This function sets the blackboard variable that determines whether
    // this enemy can see the player
    public void SetCanSee(bool to)
    {
        canSeePlayer = to;

        if (canSeePlayer)
            GetComponent<MeshRenderer>().material.color = Color.red;
        else
            GetComponent<MeshRenderer>().material.color = Color.blue;
    }
}
