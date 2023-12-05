using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{
    NetworkingController networkingController;
    public GameObject prefab;
    
    // Start is called before the first frame update
    void Start()
    {
        networkingController = GetComponent<NetworkingController>();
	StartCoroutine("BallTest");
    }

    IEnumerator BallTest() {
        yield return new WaitForSeconds(2);
	networkingController.Join("testingRoom57");
	yield return new WaitForSeconds(1);
	StartCoroutine("CubeTest");
	yield return new WaitForSeconds(20);
	Debug.Log("PLAYER 1 LEAVING ROOM");
	networkingController.LeaveRoom();
    }

    IEnumerator CubeTest() {
        GameObject go;
        go = Instantiate(prefab);
        SendableObject so = go.GetComponent<SendableObject>();
        so.Initialize(0, networkingController.otherIDs, networkingController.room.Networking.SelfPeerID, networkingController.isHost, networkingController.room.Networking);
        go.transform.localPosition = new Vector3(0,-1,1);
        go.GetComponent<MeshRenderer>().enabled = false;
        so.SpawnEverywhere();
        yield return new WaitForSeconds(2);
	for(int i = 0; i < 50; i++) {
            go.transform.Translate(-0.1f,0.1f,0);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
