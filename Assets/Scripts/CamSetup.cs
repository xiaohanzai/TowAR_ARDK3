using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSetup : MonoBehaviour
{
    public Transform obj;
    public Camera camera;
    private RenderingUI render;
    //private bool renderBool;

    // Start is called before the first frame update
    void Start()
    {
	//renderBool = false;
        transform.LookAt(obj.position + new Vector3(0,0.1f,0));
	//transform.Rotate(0,0,180,Space.Self);
	camera.transform.gameObject.SetActive(false);
	render = obj.gameObject.GetComponent<RenderingUI>();
    }

    public void Reset() {
        render.enabled = false;
    }

    public void SwitchRendering(bool renderBool) {
        //renderBool = !renderBool;
	render.enabled = renderBool;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
