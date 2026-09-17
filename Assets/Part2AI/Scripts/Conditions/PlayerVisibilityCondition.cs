using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "Player Visibility", story: "[Agent] can see [Player] in [coneAperture] and [visionRange]", category: "Conditions", id: "991374082dcfd47110645bdf23397bb9")]
public partial class PlayerVisibilityCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<float> coneAperture;
    [SerializeReference] public BlackboardVariable<float> visionRange;

    public override bool IsTrue()
    {
        //We find the vector coming from the eyes and pointing to the player
        Vector3 playerDirection = Player.Value.transform.position
            - Agent.Value.transform.position;

        //We determine if the enemy can see the player
        bool canSee =
            Vector3.Dot(playerDirection.normalized,
            Agent.Value.transform.forward) >= coneAperture.Value
            &&
            playerDirection.magnitude <= visionRange.Value
            //&&
            //RaycastVisionCone()
            ;
       // Debug.Log(canSee.ToString());

        return canSee;
    }

    public override void OnStart()
    {
        //If the player hasn't been assigned, find the player and assign it
        if (Player == null || Player.Value == null)
        {
            Player.Value = GameObject.FindGameObjectWithTag("Player");
        }
        //Debug.Log(coneAperture.Value + " " + visionRange.Value);
    }

    public override void OnEnd()
    {
    }
}
