using System.Threading.Tasks;
using UnityEngine;
using Unity.Netcode;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;

public class RelayManager : SingletonPersistent<RelayManager>
{
    public string joinCode {get; private set;}
    private async void Start()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Signed in anonymously!");
        }
    }

    public async Task<string> CreateRelay()
    {
        try
        {
            // Create a Relay allocation for 2 players (host + 1 client)
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(1);

            joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            Debug.Log("Relay Join Code: " + joinCode);

            // Set up transport with Relay
            var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
            transport.SetRelayServerData(
                allocation.RelayServer.IpV4,
                (ushort)allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData
            );

            NetworkManager.Singleton.StartHost();
            return joinCode;
        }
        catch (RelayServiceException e)
        {
            Debug.LogError(e);
            return null;
        }
    }
}