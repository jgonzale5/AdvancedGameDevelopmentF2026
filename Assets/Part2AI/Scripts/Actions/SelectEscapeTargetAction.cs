using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SelectEscapeTarget", story: "Set [NewDestination] when [Self] moves away [Distance] away from [Player]", category: "Action", id: "ee893799cebac2339ded49d664792a1b")]
public partial class SelectEscapeTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> Distance;
    [SerializeReference] public BlackboardVariable<GameObject> Player;
    [SerializeReference] public BlackboardVariable<Vector3> NewDestination;

    protected override Status OnStart()
    {
        //Get the direction to the player
        Vector3 playerDirection = Player.Value.transform.position -
            Self.Value.transform.position;
        //Flatten it so we don't worry about vertical distance
        playerDirection.y = 0;

        //Set the destination in the opposite direction to the player
        //A number of units away equal to the escapeDistance
        NewDestination.Value = 
            Player.Value.transform.position -
            playerDirection.normalized * Distance;

        return Status.Success;
    }

}

