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
    }

    // Update is called once per frame
    void Update()
    {
        //We find the vector coming from the eyes and pointing to the player
        playerDirection = player.position - eyeObject.position;

        //We determine if the enemy can see the player
        bool canSee = 
            Vector3.Dot(playerDirection.normalized,
            eyeObject.forward) >= coneAperture
            && 
            playerDirection.magnitude <= visionRange
            &&
            RaycastVisionCone();

        //Execute events for vision
        // if raycasts can be drawn between the enemy eyes and the player
        UpdateSee(canSee);
    }

    /// <summary>
    /// This function determines with raycasts if the enemy can see the player
    /// </summary>
    /// <returns>The result</returns>
    public bool RaycastVisionCone()
    {
        //A hit to get information on the hit
        RaycastHit hit;
        //A way to keep information about the ray itself
        Ray ray = new Ray(eyeObject.position, playerDirection.normalized);
        //We cast the ray in the direction of the player and store the 
        //information in hit
        Physics.Raycast(ray, out hit, visionRange);
        //To debug the ray 
        Debug.DrawRay(ray.origin, ray.direction * visionRange,
            Color.red, 0.2f);

        //Return whether the object hit is the player
        return hit.transform.gameObject.CompareTag("Player");
    }
}
