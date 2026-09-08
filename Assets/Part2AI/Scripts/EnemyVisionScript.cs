using UnityEngine;
using UnityEngine.InputSystem.XR;

public class EnemyVisionScript : MonoBehaviour
{
    //The maximum distance that this enemy can see
    [SerializeField]
    private float visionRange = 3.0f;
    //We'll use dot product to determine if the player is in the cone
    // of vision for this enemy. 
    [SerializeField, Range(-1, 1)]
    private float coneAperture = 0;
    //The transform that the enemy is "seeing" out of.
    [SerializeField]
    private Transform eyeObject;
    //A reference to the player so we always know if they're in range.
    private Transform player;
    //
    private Vector3 playerDirection;
    //This delegate function will let us reuse this script
    // by changing what function it calls when it sees something.
    public delegate void SetCanSee(bool to);
    public SetCanSee UpdateSee;

    //
    private void Start()
    {
        //At the start, look for the object with the Player tag
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //We find the vector coming from the eyes and pointing to the player
        playerDirection = player.position - eyeObject.position;
    }

    // Update is called once per frame
    void Update()
    {
        //We determine if the enemy can see the player
        bool canSee = 
            Vector3.Dot(playerDirection.normalized,
            eyeObject.forward) >= coneAperture
            && 
            playerDirection.magnitude <= visionRange;

        //
        UpdateSee(canSee);
    }
}
