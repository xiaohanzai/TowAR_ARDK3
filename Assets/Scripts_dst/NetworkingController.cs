using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using Niantic.Lightship.SharedAR.Networking;
using Niantic.Lightship.SharedAR.Rooms;
//using Niantic.Lightship.SharedAR.Rooms.RoomManagementService;


public class DataPacket {
    public int objectID;
    public PeerID id;
    public int tag;
    public byte[] rawdata;

    public DataPacket(int objectID, PeerID id, int tag, byte[] rawdata) {
        this.objectID = objectID;
	this.id = id;
	this.tag = tag;
	this.rawdata = rawdata;
    }
}

public class NetworkingController : MonoBehaviour
{
    private readonly object listLock = new object();
    public List<GameObject> registeredPrefabs;
    public IRoom room;
    public bool isHost;
    public List<PeerID> otherIDs;
    public Dictionary<int,SendableObject> instantiatedObjects;
    int nextObjectID;
    //List<Tuple<Action<DataReceivedArgs>,DataReceivedArgs>> processList = new List<Tuple<Action<DataReceivedArgs>,DataReceivedArgs>>();
    List<DataPacket> processDataList = new List<DataPacket>();

    // Start is called before the first frame update
    void Start()
    {
        nextObjectID = -1;
	isHost = false;
	//registeredPrefabs = new List<SendableObject>();
	instantiatedObjects = new Dictionary<int,SendableObject>();	
    }

    void LeftRoom(PeerIDArgs pargs) {
        Debug.Log("SOMEONE LEFT THE ROOM, " + room.Networking.PeerIDs.Count + " REMAINING");
        if (room.Networking.PeerIDs.Count == 0) {
	    Debug.Log("DELETING ROOM NOW!");
	    RoomManagementService.DeleteRoom(room.RoomParams.RoomID);
	}
    }

    public void LeaveRoom() {
        room.Leave();
    }

    public bool Join(string RoomName) {
        string name = "ARDK3_COMP_TOWAR_" + RoomName;
	string roomID = "";
	bool joined = false;
        List<IRoom> rooms = new List<IRoom>();
	RoomManagementService.GetAllRooms(out rooms);
	int roomCounter = 0;
	foreach(IRoom r in rooms) {
	    if (r.RoomParams.Name == name) {
	        roomCounter += 1;
		roomID = r.RoomParams.RoomID;
	    }
	}
	
	if (roomCounter == 0) {
	    RoomParams rp = new RoomParams(10,name,"ARDK3 Comp TOWAR Room","1234",RoomVisibility.Public);
	    RoomManagementService.CreateRoom(rp, out room);
	    room.Initialize();
	    room.Join();
	    joined = true;
	    isHost = true;
	    nextObjectID = 0;
	    Debug.Log("HOST HAS JOINED");
	} else if (roomCounter == 1) {
	    RoomManagementService.GetRoom(roomID,out room);
	    room.Initialize();
	    room.Join();
	    joined = true;
	    Debug.Log("PEER HAS JOINED");	    
	} else {
	    Debug.Log("TOO MANY ROOMS WITH SAME NAME");
	    
	}

	if (joined) {
	    Debug.Log("I JOINED BOIII");
	    //otherIDs = room.Networking.PeerIDs;
	    StartCoroutine("SetRoomLst");
	    room.Networking.DataReceived += DataIncoming;
	    room.Networking.PeerRemoved += LeftRoom;
	}

	return joined;
    }

    IEnumerator SetRoomLst() {
        yield return new WaitForSeconds(0.5f);
	otherIDs = room.Networking.PeerIDs;
    }

    public void ProcessData(DataPacket dp) {
        PeerID id = dp.id;
	int tag = dp.tag;
        byte[] rawdata = dp.rawdata;

	int[] objectID = new int[1];
	Buffer.BlockCopy(rawdata,0,objectID,0,sizeof(int));

	Debug.Log("OBJ ID: " + objectID[0]);

	if (tag == (int) SendableObject.MessageType.Spawn) {
	    Debug.Log("GOT THAT SPAWN! " + id);
	    int[] prefabIdx = new int[1];
    	    Buffer.BlockCopy(rawdata,sizeof(int),prefabIdx,0,sizeof(int));
	    Debug.Log("GOT IT: PREFAB: " + prefabIdx[0]);
	    GameObject go = Instantiate(registeredPrefabs[prefabIdx[0]],transform);
	    SendableObject so = go.GetComponent<SendableObject>();
	    instantiatedObjects[objectID[0]] = so;
	    so.Initialize(objectID[0], otherIDs, id, !isHost, room.Networking);
	    //so.ManuallyAddFuncs();
	} else {
	    Debug.Log("GOT THAT UPDATE! " + id);
	}

	Debug.Log("DOING ITTTTT: " + instantiatedObjects[objectID[0]].funcLst.Count);
	instantiatedObjects[objectID[0]].funcLst[tag](rawdata);
    }


    public void DataIncoming(DataReceivedArgs dargs) {
        Debug.Log("WHOA, I, " + room.Networking.SelfPeerID + ", GOT A MESSAGE: " + dargs.Tag + " FROM " + dargs.PeerID);
        PeerID id = dargs.PeerID;
	if (id.Equals(room.Networking.SelfPeerID)) {
	    return;
	}	
	
	int tag = (int) dargs.Tag;
        byte[] rawdata = dargs.CopyData();
	Debug.Log("DATA THIS LONG: " + rawdata.Length);
	Debug.Log("TAG IS " + tag + " ENUM IS " + (int) SendableObject.MessageType.Spawn);	
	int[] objectID = new int[1];
	Buffer.BlockCopy(rawdata,0,objectID,0,sizeof(int));

	lock(listLock) {
	    processDataList.Add(new DataPacket(objectID[0],id,tag,rawdata));
	}
	/*
	Debug.Log("OBJ ID: " + objectID[0]);

	if (tag == (int) SendableObject.MessageType.Spawn) {
	    Debug.Log("GOT THAT SPAWN!");
	    int[] prefabIdx = new int[1];
    	    Buffer.BlockCopy(rawdata,sizeof(int),prefabIdx,0,sizeof(int));
	    Debug.Log("GOT IT: PREFAB: " + prefabIdx[0]);
	    GameObject go = Instantiate(registeredPrefabs[prefabIdx[0]]);
	    SendableObject so = go.GetComponent<SendableObject>();
	    instantiatedObjects[objectID[0]] = so;
	    so.Initialize(objectID[0], otherIDs, id, !isHost, room.Networking);

	}
	Debug.Log("DOING ITTTTT");
	instantiatedObjects[objectID[0]].funcLst[tag](rawdata);
	*/
    }

    void Update() {

    	lock(listLock) {
            while(processDataList.Count > 0) {
	        Debug.Log("LIST COUNT: " + processDataList.Count);
	        ProcessData(processDataList[0]);
	        processDataList.RemoveAt(0);
	    }
	}

    }
}
