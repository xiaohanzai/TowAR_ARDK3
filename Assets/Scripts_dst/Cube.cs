using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : SendableObject
{
    Vector3 prevPos;
    Quaternion prevRot;
    // Start is called before the first frame update
    public override void Init()
    {
        registerIdx = 1;
        base.Start();
	prevPos = transform.position;
	prevRot = transform.rotation;
    }

    public override bool HasChanged() {
         bool retval = false;
         if (transform.position != prevPos || transform.rotation != prevRot) {
	     prevPos = transform.position;
	     prevRot = transform.rotation;
	     retval = true;
	 }

	 return retval;
    }

    public override byte[] GatherData() {
	ushort[] data = DataToHalf(transform.position,transform.rotation,7*sizeof(ushort));    
        byte[] rawdata = new byte[2*sizeof(int) + 7 * sizeof(ushort)];
	int[] currArr = new int[1];
	currArr[0] = objectID;
	Buffer.BlockCopy(currArr,0,rawdata,0,sizeof(int));
	currArr[0] = registerIdx;
	Debug.Log("CUBE IDX: " + registerIdx);
	Buffer.BlockCopy(currArr,0,rawdata,sizeof(int),sizeof(int));	
	Buffer.BlockCopy(data,0,rawdata,2*sizeof(int),7*sizeof(ushort));	

	return rawdata;
    }

    public override void SpawnReceived(byte[] rawdata) {
        ushort[] data = new ushort[7];
	Buffer.BlockCopy(rawdata,2*sizeof(int),data,0,7*sizeof(ushort));
	float[] fulldata = HalfToData(data);
	Vector3 pos = new Vector3(fulldata[0],fulldata[1],fulldata[2]);
	Quaternion quat = new Quaternion(fulldata[3],fulldata[4],fulldata[5],fulldata[6]);
	transform.position = pos;
	transform.rotation = quat;
    }

    public override void UpdateReceived(byte[] rawdata) {
        ushort[] data = new ushort[7];
	Buffer.BlockCopy(rawdata,2*sizeof(int),data,0,7*sizeof(ushort));
	float[] fulldata = HalfToData(data);
	Vector3 pos = new Vector3(fulldata[0],fulldata[1],fulldata[2]);
	Quaternion quat = new Quaternion(fulldata[3],fulldata[4],fulldata[5],fulldata[6]);
	transform.position = pos;
	transform.rotation = quat;
    }
}
