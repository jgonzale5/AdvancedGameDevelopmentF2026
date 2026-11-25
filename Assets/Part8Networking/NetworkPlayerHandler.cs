using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class NetworkPlayerHandler : NetworkBehaviour
{
    //Network variables HAVE to be value types. So nothing that is passed by reference.
    //That includes: Classes, arrays, lists, and strings
    //Structures are fine, but you have to write them a little differently
    public NetworkVariable<FixedString32Bytes> playerName
        = new NetworkVariable<FixedString32Bytes>("null", 
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    public NetworkVariable<PlayerInfo> playerInformation
        = new NetworkVariable<PlayerInfo>(new PlayerInfo(),
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Owner);

    public struct PlayerInfo : INetworkSerializable
    {
        public FixedString32Bytes name;
        public int rank;

        public void SetName(FixedString32Bytes to)
        {
            name = to;
        }

        

        //The interface necessary to serialize a custom struct
        public void NetworkSerialize<T>(BufferSerializer<T> serializer) 
            where T : IReaderWriter
        {
            serializer.SerializeValue(ref name);
            serializer.SerializeValue(ref rank);
        }
    }

    public TextMeshPro nameDisplay;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        Debug.Log("Hi! I'm " + NetworkObject.OwnerClientId);

        //Whenever the network variable value changes, we change the player name
        playerName.OnValueChanged +=
            (FixedString32Bytes oldValue, FixedString32Bytes newValue) =>
            {
                Debug.Log("test");
                //nameDisplay.text = playerName.Value.ToString();
                nameDisplay.text = playerName.Value.ToString();
            };

        //If this is the object of the local player, they update their name
        if (IsLocalPlayer)
        {
            playerName.Value = PlayerNamingScript.thisPlayerName;
        }
        ////Whenever the network variable value changes, we change the player name
        //playerInformation.OnValueChanged +=
        //    (PlayerInfo oldValue, PlayerInfo newValue) =>
        //    {
        //        Debug.Log("test");
        //        //nameDisplay.text = playerName.Value.ToString();
        //        nameDisplay.text = playerInformation.Value.name.ToString();
        //    };

        ////If this is the object of the local player, they update their name
        //if (IsLocalPlayer)
        //{
        //    //playerName.Value = PlayerNamingScript.thisPlayerName;
        //    playerInformation.Value.SetName(PlayerNamingScript.thisPlayerName);
        //}

    }


    [ClientRpc]
    public void DebugClientRpc(string to)
    {

        Debug.Log("Name updated to " + to);
    }
}
