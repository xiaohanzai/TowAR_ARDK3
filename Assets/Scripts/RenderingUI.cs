using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingUI : MonoBehaviour
{
    public Camera camera;
    public RenderTexture[] textures;
    int childIdx;
    // Start is called before the first frame update
    void Start()
    {
	//camera.targetTexture = textures[0];
	
    }

    // Update is called once per frame
    void Update()
    {
	float currTime = Time.time;

	childIdx = 0;
        foreach(RenderTexture texture in textures) {
	    camera.targetTexture = texture;
	    transform.GetChild(childIdx).gameObject.SetActive(true);
	    transform.GetChild(childIdx).transform.rotation = Quaternion.Euler(0,(currTime * 100) % 360,0);
	    camera.Render();
	    transform.GetChild(childIdx).gameObject.SetActive(false);
	    childIdx++;
	}
        
	/*	
	camera.targetTexture = textures[childIdx];
	//transform.GetChild(childIdx).gameObject.SetActive(true);
	transform.GetChild(childIdx).transform.rotation = Quaternion.Euler(0,(currTime * 10) % 360,0);
	camera.Render();
	*/
	
    }

    /*
    void LateUpdate() {
        transform.GetChild(childIdx).gameObject.SetActive(false);
	childIdx = (childIdx + 1) % textures.Length;
	transform.GetChild(childIdx).gameObject.SetActive(true);
    }
    */
}
