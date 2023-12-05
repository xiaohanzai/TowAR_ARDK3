using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialDome : MonoBehaviour
{
	 HashSet<MyPanel> wallPanels;
	 HashSet<MyPanel> floorPanels;
	public MyPanel panelPrefab;
	public float panelSize;
	public float panelDepth;
	public float panelAngle;
	public int numRows;
	float panelRadius;
	public float panelOpacity;
	public Material groundMat;
	public Material foliageMat;

	/*JDM */
	//public GameObject winObjectWallPrefab;
	//public GameObject winObjectFloorPrefab;
	//public GameObject loseObjectWallPrefab;
	//public GameObject loseObjectFloorPrefab;
	//public float growTime = 2f;
	//public float startScale = 0f;
	//public float endScale = .1f;
	/*JDM */

	int CalcNumPanels()
	{
		//float SA = 4*Mathf.PI*roughRadius*roughRadius;
		//float theta = Mathf.Asin(panelRadius / roughRadius);
		//float subSA = 2*Mathf.PI*roughRadius*roughRadius*(1 - Mathf.Cos(theta));
		//int n = (int)(SA / subSA);

		//refinedRadius = Mathf.Sqrt(subSA / (4*Mathf.PI));

		return (int)(360 / panelAngle);
	}

	void Start()
	{
		SetUpBlocks();
		DomeOff();
	}

	void Update()
	{

	}

	void SetUpBlocks()
	{
		MyPanel testObj;
		wallPanels = new HashSet<MyPanel>();
		floorPanels = new HashSet<MyPanel>();
		Color currColor;
		int num = CalcNumPanels();
		int panelCounter = 0;
		//wallPanels = new MyPanel[num*Mathf.Max(1,numRows)];
		panelRadius = panelSize / (Mathf.Tan((panelAngle / 2) * Mathf.PI / 180));
		//panelRadius += panelDepth;
		for (int j = 0; j < Mathf.Max(1, numRows); j++)
		{
			float angleShift = panelAngle / 2;
			for (int i = 0; i < num; i++)
			{
				//wallPanels[j*i + i] = MyPanel.newPanel(prefab,6,panelDepth,panelSize);
				testObj = MyPanel.NewPanel(panelPrefab, 6, panelDepth, panelSize, 100, "foliage");
				//wallPanels[j*i + i].transform.position = new Vector3(panelRadius*Mathf.Sin((i*panelAngle + angleShift*(j % 2)) * Mathf.PI / 180),
				testObj.transform.position = new Vector3(panelRadius * Mathf.Sin((i * panelAngle + angleShift * (j % 2)) * Mathf.PI / 180),
													 j * (panelSize * 1.75f),
								   panelRadius * Mathf.Cos((i * panelAngle + angleShift * (j % 2)) * Mathf.PI / 180));
				//wallPanels[j*i + i].transform.LookAt(transform);
				//wallPanels[j*i + i].transform.SetParent(transform);
				testObj.transform.LookAt(transform);
				testObj.transform.rotation = Quaternion.Euler(0, testObj.transform.rotation.eulerAngles.y, 0);
				testObj.transform.SetParent(transform.GetChild(0));
				testObj.GetComponent<MeshRenderer>().material = foliageMat;

				currColor = testObj.GetComponent<MeshRenderer>().material.color;
				//currColor.a = 0.3f; //Mathf.Min(1f,Mathf.Max(0f,panelOpacity));
				testObj.GetComponent<MeshRenderer>().material.color = new Color(currColor.r, currColor.g, currColor.b, panelOpacity);
				testObj.GetComponent<MeshRenderer>().enabled = false;
				testObj.name = "Foliage_" + (panelCounter).ToString();
				testObj.GetComponent<MyPanel>().relativePos = testObj.transform.localPosition;
				panelCounter += 1;
				wallPanels.Add(testObj);
			}
		}
		panelCounter = 0;
		float groundRadius = panelRadius + 2 * panelSize;
		int numGroundRows = (int)Mathf.Ceil(groundRadius / (2 * panelSize)) + 1;
		int numGrndPanels = 0;
		//floorPanels = new MyPanel[4*numGroundRows*numGroundRows];
		for (int j = -numGroundRows; j < numGroundRows; j++)
		{
			float xShift = 2 * panelSize;
			for (int i = -numGroundRows; i < numGroundRows; i++)
			{
				float x = 2 * panelSize * i * 2f + xShift * (j % 2);
				float y = 2 * panelSize * j * 1.75f;
				if (Mathf.Sqrt(x * x + y * y) < groundRadius)
				{
					numGrndPanels += 1;
					testObj = MyPanel.NewPanel(panelPrefab, 6, panelDepth, 2 * panelSize, 100, "ground");
					testObj.transform.rotation = Quaternion.Euler(-90, 0, 0);
					testObj.transform.position = new Vector3(x, 0, y);
					testObj.transform.SetParent(transform.GetChild(1));
					testObj.GetComponent<MeshRenderer>().material = groundMat;
					currColor = testObj.GetComponent<MeshRenderer>().material.color;
					//currColor.a = 0.3f; //Mathf.Min(1f,Mathf.Max(0f,panelOpacity));
					testObj.GetComponent<MeshRenderer>().material.color = new Color(currColor.r, currColor.g, currColor.b, panelOpacity);
					testObj.GetComponent<MeshRenderer>().enabled = false;
					testObj.name = "Ground_" + (panelCounter).ToString();
					testObj.GetComponent<MyPanel>().relativePos = testObj.transform.localPosition;
					panelCounter += 1;
					floorPanels.Add(testObj);
				}
			}
		}

		transform.position = new Vector3(0, -1f, 0);
	}

	public void DomeOn()
	{
		foreach (MyPanel p in wallPanels)
		{
			if (p.gameObject.activeSelf)
			{
				p.gameObject.layer = 8;
			}
		}
		foreach (MyPanel p in floorPanels)
		{
			if (p.gameObject.activeSelf)
			{
				p.gameObject.layer = 8;
			}
		}
	}

	public void DomeOff()
	{
		foreach (MyPanel p in wallPanels)
		{
			if (p.gameObject.activeSelf)
			{
				p.gameObject.layer = 9;
			}
		}
		foreach (MyPanel p in floorPanels)
		{
			if (p.gameObject.activeSelf)
			{
				p.gameObject.layer = 9;
			}
		}
	}

	public void AdjustForPlatform()
	{
		Ray ray;
		RaycastHit hit;
		foreach (MyPanel p in floorPanels)
		{
			ray = new Ray(p.transform.position, Vector3.up);
			if (Physics.Raycast(ray, out hit))
			{
				if (hit.transform.root.name.Contains("Platform"))
				{
					p.gameObject.SetActive(false);
				}
			}
		}
	}

	public void SetPosition(Vector3 pos)
    {
		transform.position = pos;
    }

	//adds win objects onto the panels
	//public void SpawnWinObjectOnPanel(Transform panelTransform, bool isWallPanel)
	//{
		//if (isWallPanel && winObjectWallPrefab != null)
		//{
		//	GameObject winObject = Instantiate(winObjectWallPrefab, panelTransform.position + Vector3.up * 2.0f, Quaternion.identity);
		//	winObject.transform.SetParent(panelTransform);
		//	winObject.name = "WinObject_Wall";
		//	StartCoroutine(GrowObjectOverTime(winObject.transform, startScale, endScale, growTime));
		//}
		//else if (!isWallPanel && winObjectFloorPrefab != null)
	//	{
		//	GameObject winObject = Instantiate(winObjectFloorPrefab, panelTransform.position + Vector3.up * 2.0f, Quaternion.identity);
		//	winObject.transform.SetParent(panelTransform);
		//	winObject.name = "WinObject_Floor";
	//		StartCoroutine(GrowObjectOverTime(winObject.transform, startScale, endScale, growTime));
	//	}
	//	else
	//	{
	//		Debug.LogError("Prefab not assigned for winObjectPrefab in the inspector.");
	//	}
	//}


	//public void SpawnLoseObjectOnPanel(Transform panelTransform, bool isWallPanel)
	//{
	//	if (isWallPanel && loseObjectWallPrefab != null)
	//	{
	//		GameObject loseObject = Instantiate(loseObjectWallPrefab, panelTransform.position + Vector3.up * 2.0f, Quaternion.identity);
	//		loseObject.transform.SetParent(panelTransform);
	//		loseObject.name = "LoseObject_Wall";
	//		StartCoroutine(GrowObjectOverTime(loseObject.transform, startScale, endScale, growTime));
	//	}
	//	else if (!isWallPanel && loseObjectFloorPrefab != null)
	//	{
	//		GameObject loseObject = Instantiate(loseObjectFloorPrefab, panelTransform.position + Vector3.up * 2.0f, Quaternion.identity);
	//		loseObject.transform.SetParent(panelTransform);
	//		loseObject.name = "LoseObject_Floor";
	//		StartCoroutine(GrowObjectOverTime(loseObject.transform, startScale, endScale, growTime));
	//	}
	//	else
	//	{
	//		Debug.LogError("Prefab not assigned for loseObjectPrefab in the inspector.");
	//	}
	//}

	// Coroutine to scale an object over time ***NOT CURRENTLY IN USE
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