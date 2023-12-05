using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Niantic.Lightship.SharedAR.Networking;
using Niantic.Lightship.SharedAR.Rooms;
//using Niantic.Lightship.SharedAR.Rooms.RoomManagementService;


public class MPTest : MonoBehaviour
{
    public IRoom room;
    private List<IRoom> rooms;
    bool otherIDReceived;
    PeerID otherID;

    // Start is called before the first frame update
    void Start()
    {
        otherIDReceived = false;
        Debug.Log(room);
	RoomParams rp = new RoomParams(2,"testCMAX42","test desc","1234",RoomVisibility.Public);
	//RoomParams rp = new RoomParams("testROOMID",RoomVisibility.Public);

        RoomManagementService.CreateRoom(rp,out room);
        //RoomManagementService.CreateRoom(new RoomParams(2,"test","test desc","1234",RoomVisibility.Public),out room);	
        //RoomManagementService.GetOrCreateRoomForName(new RoomParams(2,"test","test desc","1234",RoomVisibility.Public),out room);	
	room.Initialize();
	room.Networking.PeerAdded += GetOther;
	room.Networking.NetworkEvent += Boop;	
	room.Join();
	Debug.Log("self id: " + room.Networking.SelfPeerID);
	Debug.Log("printing peer ids");
	foreach(PeerID id in room.Networking.PeerIDs) {
	    Debug.Log("ID: " + id);
	}
	Debug.Log("Done printing peer ids");
	RoomManagementService.GetAllRooms(out rooms);
	int counter = 0;
	string myid = "";
	foreach(IRoom r in rooms) {
	    if (r.RoomParams.Name == "testCMAX2") {
	       myid = r.RoomParams.RoomID;
	    }
	    Debug.Log("ROOM ID " + counter + ": " + r.RoomParams.RoomID);
	    //Debug.Log("ROOM PW " + counter + ": " + r.RoomParams.Passcode);
	    counter += 1;
	}
	Debug.Log("MY ROOM ID: " + room.RoomParams.RoomID);
	StartCoroutine("LoopSend");
    }

    void Boop(NetworkEventArgs nargs) {
        Debug.Log("BEEP: " + nargs.networkEvent);
	if (nargs.networkEvent == NetworkEvents.ArdkShutdown) {
	    Debug.Log("Room deleted");
	    RoomManagementService.DeleteRoom(room.RoomParams.RoomID);
	}
    }

    void GetOther(PeerIDArgs pargs) {
        //Debug.Log("OTHERID: " + pargs.PeerID.Equals(room.Networking.SelfPeerID));
	otherID = pargs.PeerID;
	otherIDReceived = true;
    }

    IEnumerator LoopSend() {
        while(!otherIDReceived) {
	    yield return new WaitForSeconds(0.5f);
	}

	Debug.Log("MAIN, MY ID: " + room.Networking.SelfPeerID.ToUint32());
	foreach(PeerID id in room.Networking.PeerIDs) {
	    Debug.Log("MAIN, ID: " + id.ToUint32());
	}

	List<PeerID> ids = new List<PeerID>();
	ids.Add(otherID);
	int counter = 0;
	byte[] rawdata = new byte[sizeof(int)];
	int[] data = new int[1];
	while(true) {
	    data[0] = counter;
	    Buffer.BlockCopy(data,0,rawdata,0,sizeof(int));
	    room.Networking.SendData(ids, 7, rawdata);
	    counter += 1;
	    yield return new WaitForSeconds(5);
	}
    }

    IEnumerator tester() {
        yield return new WaitForSeconds(4);
	RoomManagementService.DeleteRoom(room.RoomParams.RoomID);
	Debug.Log("Room DELETE: " + room.RoomParams.RoomID);
	yield return new WaitForSeconds(2);
	int counter = 0;
	foreach(IRoom r in rooms) {
            Debug.Log("ROOM ID " + counter + ": " + r.RoomParams.RoomID);
            //Debug.Log("ROOM PW " + counter + ": " + r.RoomParams.Passcode);
            counter += 1;
        }
    }

}
