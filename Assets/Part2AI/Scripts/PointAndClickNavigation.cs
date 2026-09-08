using UnityEngine;
//We add the AI library
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class PointAndClickNavigation : MonoBehaviour
{
    //We add a reference to this character's NavMeshAgent
    [SerializeField]
    private NavMeshAgent agent;

    //We call this function to tell the agent to move. Meant to be called when
    //the player clicks.
    public void MoveAgentToClick()
    {
        //We use ScreenPointToRay to get the ray coming from the camera in the direction
        //where the player is clicking
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;

        //We cast a ray in that direction
        if (Physics.Raycast(ray, out hit))
        {
            //We tell the player to go at the point raycasted
            agent.SetDestination(hit.point);
        }
    }
}
