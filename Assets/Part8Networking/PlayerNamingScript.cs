using Unity.Netcode;
using UnityEngine;

public class PlayerNamingScript : NetworkBehaviour
{
    //The name of this player
    public static string thisPlayerName;

    //Set the name of the player to the value sent
    public void SetName(string to)
    {
        thisPlayerName = to;

        //Set the player name to the value received
        thisPlayerName = to;

        //Look for players and only tell the local one to change their name
        var players = FindObjectsByType<NetworkPlayerHandler>();

        foreach (var p in players)
        {
            if (p.IsLocalPlayer)
            {
                p.playerInformation.Value.SetName(to);
                p.DebugClientRpc(p.OwnerClientId.ToString());
                return;
            }
        }
    }

}
