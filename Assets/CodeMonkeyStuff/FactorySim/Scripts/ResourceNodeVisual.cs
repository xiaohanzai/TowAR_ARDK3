using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceNodeVisual : MonoBehaviour {

    private void Awake() {
        //transform.Find("Area").gameObject.SetActive(false);

        Transform[] variantTransformArray = new Transform[] {
            transform.Find("Variant_1"),
            transform.Find("Variant_2"),
            transform.Find("Variant_3"),
            transform.Find("Variant_4")
        };

        foreach (Transform variantTransform in variantTransformArray) {
            variantTransform.gameObject.SetActive(false);
        }

        variantTransformArray[Random.Range(0, variantTransformArray.Length)].gameObject.SetActive(true);
    }

}
