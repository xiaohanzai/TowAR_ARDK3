using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RpcTest : NetworkBehaviour
{

    public override void OnNetworkSpawn() {
        if (!IsServer && IsOwner) {
	    TestServerRpc(0, NetworkObjectId);
	}
    }

    [ClientRpc]
    void TestClientRpc(int value, ulong sourceNetworkObjectId) {
        Debug.Log($"Cient Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
	if (IsOwner) {
	    TestServerRpc(value + 1, sourceNetworkObjectId);
	}
    }

    [ServerRpc]
    void TestServerRpc(int value, ulong sourceNetworkObjectId) {
        Debug.Log($"Server Received the RPC #{value} on NetworkObject #{sourceNetworkObjectId}");
	TestClientRpc(value, sourceNetworkObjectId);
    }
}
