using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrow : MonoBehaviour
{
    public float growTime = 2f;
    public float startScale = 0f;
    public float endScale = 1f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GrowObjectOverTime(this.transform, startScale, endScale, growTime));

    }

	// Coroutine to scale an object over time 
	IEnumerator GrowObjectOverTime(Transform objTransform, float startScale, float endScale, float growTime)
	{
		float elapsedTime = 0f;

		while (elapsedTime < growTime)
		{
			float scale = Mathf.Lerp(startScale, endScale, elapsedTime / growTime);
			objTransform.localScale = new Vector3(scale, scale, scale);

			elapsedTime += Time.deltaTime;
			yield return null;
		}

		// Ensure the final scale is precisely the endScale
		objTransform.localScale = new Vector3(endScale, endScale, endScale);
	}

}
