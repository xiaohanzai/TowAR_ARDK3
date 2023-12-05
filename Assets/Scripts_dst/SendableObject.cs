using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Niantic.Lightship.SharedAR.Networking;
using Niantic.Lightship.SharedAR.Rooms;


public abstract class SendableObject : MonoBehaviour
{

    public enum MessageType : uint {
         Spawn = 0,
         Update = 1,
    }

    protected int registerIdx;
    protected int objectID;
    protected List<PeerID> ids;
    protected PeerID owner;
    protected bool isHost;
    protected INetworking networking;
    public List<Action<byte[]>> funcLst = new List<Action<byte[]>>();

    protected void Start() {
        //if (funcLst.Count == 0) {
        //    funcLst.Add(SpawnReceived);
        //    funcLst.Add(UpdateReceived);
	//}
    }

    public void ManuallyAddFuncs() {
        funcLst.Add(SpawnReceived);
        funcLst.Add(UpdateReceived);
    }

    public static ushort[] DataToHalf(Vector3 pos, Quaternion quat, int size) {
        ushort[] retVal = new ushort[size];

        retVal[0] = Mathf.FloatToHalf(pos.x);
        retVal[1] = Mathf.FloatToHalf(pos.y);
        retVal[2] = Mathf.FloatToHalf(pos.z);
        retVal[3] = Mathf.FloatToHalf(quat.x);	
        retVal[4] = Mathf.FloatToHalf(quat.y);
        retVal[5] = Mathf.FloatToHalf(quat.z);
        retVal[6] = Mathf.FloatToHalf(quat.w);

        return retVal;
    }

    public static float[] HalfToData(ushort[] data) {
        float[] retVal = new float[7];

        retVal[0] = Mathf.HalfToFloat(data[0]);
        retVal[1] = Mathf.HalfToFloat(data[1]);
        retVal[2] = Mathf.HalfToFloat(data[2]);
        retVal[3] = Mathf.HalfToFloat(data[3]);
        retVal[4] = Mathf.HalfToFloat(data[4]);
        retVal[5] = Mathf.HalfToFloat(data[5]);
        retVal[5] = Mathf.HalfToFloat(data[6]);	

        return retVal;
    }

    public static byte[] FloatArrayToByteArray(float[] srcArr,int start, int length) {
        byte[] trgArr = new byte[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,trgArr.Length);

	return trgArr;
    }

    public static byte[] HalfArrayToByteArray(ushort[] srcArr,int start, int length) {
        byte[] trgArr = new byte[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,trgArr.Length);

	return trgArr;
    }

    public static float[] ByteArrayToFloatArray(byte[] srcArr, int start, int length) {
        float[] trgArr = new float[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,srcArr.Length - start); // always use byte array's length

	return trgArr;
    }

    public static ushort[] ByteArrayToHalfArray(byte[] srcArr, int start, int length) {
        ushort[] trgArr = new ushort[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,srcArr.Length - start); // always use byte array's length

	return trgArr;
    }

    public static byte[] IntArrayToByteArray(int[] srcArr, int start, int length) {
        byte[] trgArr = new byte[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,trgArr.Length);

	return trgArr;
    }

    public static byte[] UShortArrayToByteArray(ushort[] srcArr, int start, int length) {
        byte[] trgArr = new byte[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,trgArr.Length);

	return trgArr;
    }

    public static int[] ByteArrayToIntArray(byte[] srcArr, int start, int length) {
        int[] trgArr = new int[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,srcArr.Length-start); // always use byte array's length

	return trgArr;
    }

    public static ushort[] ByteArrayToUShortArray(byte[] srcArr, int start, int length) {
        ushort[] trgArr = new ushort[length];
        Buffer.BlockCopy(srcArr,start,trgArr,0,srcArr.Length-start); // always use byte array's length

	return trgArr;
    }

    // Update object with appropriate info
    // objectID: assign a string that is unique for each object
    // owner: PeerID for who technically spawned the object
    // isHost: is the owner the host?
    // networking: networking object from the session room, object needed for sending data
    public void Initialize(int objectID, List<PeerID> ids, PeerID owner, bool isHost, INetworking networking) {
        this.objectID = objectID;
	this.ids = ids;
        this.owner = owner;
	this.isHost = isHost;
	this.networking = networking;
	ManuallyAddFuncs();
	Init();
    }

    public void SpawnEverywhere(){
        byte[] rawdata = GatherData();
	MessageSend(MessageType.Spawn, rawdata);    
    }

    public void UpdateEveryone() {
        byte[] rawdata = GatherData();
	MessageSend(MessageType.Update, rawdata);    
    }
    
    public void MessageSend(MessageType tag, byte[] rawdata) {
        Debug.Log(networking.SelfPeerID + " sending message " + tag + " of size " + rawdata.Length + " to " + ids.Count + " peers.");
	networking.SendData(ids, (uint)tag, rawdata);
    }

    protected void Update() {
        if (HasChanged() && owner.Equals(networking.SelfPeerID)) {
	   Debug.Log("UPDATING OBJECT FROM OWNER " + owner + " BY " + networking.SelfPeerID);
	   UpdateEveryone();
	}
    }

    public abstract void Init();

    public abstract bool HasChanged();

    public abstract byte[] GatherData();

    public abstract void SpawnReceived(byte[] rawdata);

    public abstract void UpdateReceived(byte[] rawdata);
}
