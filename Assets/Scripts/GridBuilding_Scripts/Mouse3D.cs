using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

public class Mouse3D : MonoBehaviour {

    public static Mouse3D Instance { get; private set; }

    [SerializeField] private LayerMask mouseColliderLayerMask = new LayerMask();

    private Vector2 mousePos;
    private Vector3 mousePos3D;
    private bool updateMousePos3D;

    /* Xiaohan */
    //public TextMeshProUGUI textMeshPro;
    /* Xiaohan */

    private void Awake() {
        Instance = this;
        mousePos = Vector2.zero;
        mousePos3D = Vector3.zero;
    }

    private void Update() {
        /* Xiaohan */
        if (Input.GetMouseButtonDown(0) && !UtilsClass.IsPointerOverUI())
        {
            mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, mouseColliderLayerMask))
            {
                transform.position = raycastHit.point;
            }
            updateMousePos3D = true;
        }
        else
        {
            updateMousePos3D = false;
        }
        /* Xiaohan */
    }

    public static Vector3 GetMouseWorldPosition() => Instance.GetMouseWorldPosition_Instance();

    private Vector3 GetMouseWorldPosition_Instance() {
        if (updateMousePos3D)
        {
            Ray ray = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                //textMeshPro.text = hit.transform.gameObject.name + hit.point.ToString();
                mousePos3D = hit.point;
                return mousePos3D;
            }
            else
            {
                //textMeshPro.text = "no???";
                return Vector3.zero;
            }
        }
        else
        {
            return mousePos3D;
        }
    }

}
