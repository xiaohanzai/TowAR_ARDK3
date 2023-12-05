using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour
{
    public GameObject prefab;
    NetworkingController networkingController;

    // Start is called before the first frame update
    void Start()
    {
        networkingController = GetComponent<NetworkingController>();
	StartCoroutine("BallTest");
    }

    IEnumerator BallTest() {
        GameObject go;
        yield return new WaitForSeconds(2);
	networkingController.Join("testingRoom57");
	yield return new WaitForSeconds(3);

	go = Instantiate(prefab);
	SendableObject so = go.GetComponent<SendableObject>();
	so.Initialize(-1, networkingController.otherIDs, networkingController.room.Networking.SelfPeerID, networkingController.isHost, networkingController.room.Networking);
	go.transform.localPosition = new Vector3(0,0,1);
	go.GetComponent<MeshRenderer>().enabled = false;
	so.SpawnEverywhere();
	yield return new WaitForSeconds(2);

	
	for(int i = 0; i < 50; i++) {
	    go.transform.Translate(0.1f,0.1f,0);
	    yield return new WaitForSeconds(0.05f);
	}
	for(int i = 0; i < 100; i++) {
	    go.transform.Translate(-0.1f,0,0);
	    yield return new WaitForSeconds(0.05f);
	}
	for(int i = 0; i < 50; i++) {
	    go.transform.Translate(0.1f,-0.1f,0);
	    yield return new WaitForSeconds(0.05f);
	}	

	yield return new WaitForSeconds(15);
	Debug.Log("PLAYER 2 LEAVING ROOM");
	networkingController.LeaveRoom();
    }
}
