using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Niantic.Lightship.SharedAR.Networking;
using Niantic.Lightship.SharedAR.Rooms;

public class MPTestJoin : MonoBehaviour
{

    public IRoom room;
    private List<IRoom> rooms;
    private string roomID;
    List<PeerID> ids;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("TestJoin");
    }

    IEnumerator TestJoin() {
        yield return new WaitForSeconds(3);
	RoomManagementService.QueryRoomsByName("testCMAX42",out rooms);
	foreach(IRoom r in rooms) {
	    Debug.Log("RBN ID: " + r.RoomParams.RoomID);
	    roomID = r.RoomParams.RoomID;
	}
	Debug.Log("COROT: " + room);
	RoomManagementService.GetRoom(roomID,out room);
	//RoomManagementService.GetOrCreateRoomForName(new RoomParams(2,"test","test desc","1234",RoomVisibility.Public),out room);	
	Debug.Log("COROT: " + room.RoomParams.RoomID);
	
	room.Initialize();
	room.Join();
	yield return new WaitForSeconds(0.5f);
	ids = room.Networking.PeerIDs;		
	Debug.Log("ID TESTTTT: " + ids);
	Debug.Log("COROT, MY ID: " + room.Networking.SelfPeerID.ToUint32());
	foreach(PeerID id in ids) { //room.Networking.PeerIDs) {
            Debug.Log("COROT ID 1: " + id.ToUint32());
        }	
	//RoomManagementService.DeleteRoom(roomID);
	room.Networking.DataReceived += LoopReceive;
    }

    void LoopReceive(DataReceivedArgs dargs) {
        Debug.Log("GOT IT! TAG: " + dargs.Tag + ", " + dargs.DataLength);
	//MemoryStream ms = dargs.CreateDataReader();
	int[] data = new int[dargs.DataLength / sizeof(int)];
	byte[] rawdata = dargs.CopyData();
	Buffer.BlockCopy(rawdata,0,data,0,sizeof(int));
	Debug.Log("MESSAGE: " + data[0]);
    }
}
