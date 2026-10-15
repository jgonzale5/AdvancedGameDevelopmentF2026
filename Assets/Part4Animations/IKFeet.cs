using UnityEngine;

public class IKFeet : MonoBehaviour
{
    //A reference to the animator handling the animations
    [SerializeField]
    private Animator animator;
    //The name of the parameter in the animator controller that holds the weight of the left foot in this point of the animation
    [SerializeField]
    private string leftIKWeightParamName = "LeftFootIKWeight";
    //The name of the parameter on the animator controller that holds the weight of the right foot in this point of the animation
    [SerializeField]
    private string rightIKWeightParamName = "RightFootIKWeight";
    //The distance above the bone of this feet
    [SerializeField]
    private float ankleOffset = 1.0f;
    //
    [SerializeField]
    private float raycastOffset = 1.0f;
    //The length of the raycast we'll be using to detect objects on the way
    [SerializeField]
    private float rayLength = 1.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //We get the animator when the game starts
        animator = GetComponent<Animator>();
    }

    //This function will be called every frame that the IKs are set to refresh
    private void OnAnimatorIK(int layerIndex)
    {
        //We call the footIK function for each foot
        FootIK(AvatarIKGoal.LeftFoot, animator.GetFloat(leftIKWeightParamName));
        FootIK(AvatarIKGoal.RightFoot, animator.GetFloat(rightIKWeightParamName));
    }

    //This function receives an enumerator that it can use to locate a bone on the skeleton
    //The weight will be useful later.
    private void FootIK(AvatarIKGoal goal, float weight)
    {
        //Tell the IK to take priority over the animation
        animator.SetIKPositionWeight(goal, weight);
        animator.SetIKRotationWeight(goal, weight);

        //Then we calculate a ray from the position of the goal we selected, pointing down...
        Ray ray = new Ray(animator.GetIKPosition(goal) + raycastOffset * Vector3.up,
            Vector3.down);

        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * (ankleOffset + rayLength), Color.red, 0.1f);

        //And we cast that ray. 
        //Ankle offset will tell us the distance between the ankle and the bottom of the foot.
        //So the final length of the ray, will be equal to the ankleOffset + the ray length
        if (Physics.Raycast(ray, out hit, ankleOffset + rayLength))
        {
            //Move the foot to the surface touched
            Vector3 footPos = hit.point;
            //Move it up a little to match the offset
            footPos.y += ankleOffset;


            Debug.DrawRay(hit.point, hit.normal * rayLength, Color.blue, 0.1f);


            //Set the position of the bone to the one calculated
            animator.SetIKPosition(goal, footPos);
            //Set the rotation of the bone...
            animator.SetIKRotation(goal,
                Quaternion.LookRotation(transform.forward, //To be pointing forward in relation to the body
                hit.normal)); //And for its up vector to be equal to the normal vector of the surface
        }
    }

}
