using UnityEngine;
using UnityEngine.Networking;
//
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class TestRelay : MonoBehaviour
{
    //So we can set this variable from the input field
    public string joiningCode
    {
        get;

        set;
    }

    //An asynchronous function works similarly to an ienumerator
    async void Start()
    {
        //Initialize unity services
        await UnityServices.InitializeAsync();

        //Add the following events when we sign in to a serviced
        AuthenticationService.Instance.SignedIn += () =>
        {
            Debug.Log("Player " + AuthenticationService.Instance.PlayerId
                + " connected.");
        };

        //You can use this to sign in with another service or account, this lets us
        // skip that stuff, it just makes an ID for the player when they join
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    
    public async void CreateRelay()
    {
        try
        {
            //Allocate the server in the relay services
            Allocation allocation = await RelayService.
                Instance.CreateAllocationAsync(3);

            //Get the code to join the server, we're gonna use this to join it
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(
                allocation.AllocationId);

            Debug.Log(joinCode);

            //Set the details so we can connect
            NetworkManager.Singleton.GetComponent<UnityTransport>().SetHostRelayData(
                allocation.RelayServer.IpV4, //Get the IP of the relay server
                (ushort)allocation.RelayServer.Port, //Get the port on the relay server
                allocation.AllocationIdBytes, //
                allocation.Key, //To access the server
                allocation.ConnectionData //The buffer for the collection data
                );

            //Start the host
            NetworkManager.Singleton.StartHost();
        } 
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }

    //Just so we can call this function with the join button

    public void JoinRelayWithSetPassword()
    {
        JoinRelay(joiningCode);
    }

    //This function will connect us to the server with the given code
    public async void JoinRelay(string joinCode)
    {
        try
        {
            Debug.Log("Joining relay with " + joinCode);
            //Join the allocation in the relay service with this code
            JoinAllocation allocation = 
                await RelayService.Instance.JoinAllocationAsync(joinCode);

            NetworkManager.Singleton.GetComponent<UnityTransport>().SetClientRelayData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData, //Buffer for this client's connection data
                allocation.HostConnectionData //Buffer for the host's data
                );

            NetworkManager.Singleton.StartClient();
        }
        catch (RelayServiceException e)
        {
            Debug.Log(e);
        }
    }
}
