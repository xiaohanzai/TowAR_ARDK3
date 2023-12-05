using System.Collections;
using System.Collections.Generic;
using Niantic.Lightship.AR.Semantics;
using UnityEngine;

public class MyPanel : MonoBehaviour
{
	Vector3[] verts;
	Vector3[] norms;
	Vector2[] uvs;
	int[] tries;
	MeshFilter mf;
	//int clickCount;
	//int clickLimit;
	public string matType;

	public Vector3 relativePos;

	public bool canClick;
	private bool isRegrowing;

    public static MyPanel NewPanel(MyPanel og_panel, int sides, float depth, float radius, int limit, string name)
	{
		MyPanel panel = Instantiate(og_panel, new Vector3(0, 0, 0), Quaternion.identity);
		for (int i = 0; i < 3; i++)
		{
			panel.transform.GetChild(i).GetComponent<BoxCollider>().size = new Vector3(2 * radius, 2 * radius * Mathf.Tan(30 * Mathf.PI / 180), depth);
		}
		for (int i = 0; i < 3; i++)
		{
			panel.transform.GetChild(3).GetChild(i).localScale = new Vector3(radius, 1, 1);
		}

		//panel.clickLimit = limit;
		//panel.clickCount = 0;
		panel.gameObject.tag = name;
		panel.matType = name;
		panel.canClick = false;
		panel.isRegrowing = false;

		panel.mf = panel.GetComponent<MeshFilter>();
		panel.verts = new Vector3[2 * (sides + 1)];
		//panel.norms = new Vector3[2*(sides + 1)];	
		panel.tries = new int[3 * (2 * sides + 2 * sides)]; //one triangle per side times 2 for top/bottom, wall formed by each side also has 2 triangles
		panel.verts[sides] = new Vector3(0, 0, 0);
		panel.verts[2 * sides + 1] = new Vector3(0, 0, depth);
		float startAng = Mathf.PI / 2;
		float deltAng = 2 * Mathf.PI / sides;
		for (int i = 0; i < sides; i++)
		{
			panel.verts[i] = new Vector3(radius * Mathf.Cos(startAng), radius * Mathf.Sin(startAng), 0);
			startAng += deltAng;
		}
		int tri_counter = 0;
		for (int i = 0; i < sides; i++)
		{
			panel.tries[tri_counter] = sides;
			panel.tries[tri_counter + 1] = i;
			panel.tries[tri_counter + 2] = (i + 1) % sides;
			tri_counter += 3;
		}

		startAng = Mathf.PI / 2;
		for (int i = 0; i < sides; i++)
		{
			panel.verts[i + sides + 1] = new Vector3(radius * Mathf.Cos(startAng), radius * Mathf.Sin(startAng), depth);
			startAng += deltAng;
		}

		for (int i = 0; i < sides; i++)
		{
			panel.tries[tri_counter] = 2 * sides + 1;
			panel.tries[tri_counter + 1] = (i + 1) % sides + sides + 1;
			panel.tries[tri_counter + 2] = i + sides + 1;
			tri_counter += 3;
		}

		for (int i = 0; i < sides; i++)
		{
			panel.tries[tri_counter] = i;
			panel.tries[tri_counter + 1] = i + sides + 1;
			panel.tries[tri_counter + 2] = (i + 1) % sides;
			tri_counter += 3;

			panel.tries[tri_counter] = (i + 1) % sides;
			panel.tries[tri_counter + 1] = i + sides + 1;
			panel.tries[tri_counter + 2] = (i + 1) % sides + sides + 1;
			tri_counter += 3;
		}


		Mesh mesh = new Mesh();
		//Debug.Log(panel.mf);
		panel.mf.mesh = mesh;
		mesh.vertices = panel.verts;
		//mesh.uv = panel.uvs;
		mesh.triangles = panel.tries;

		return panel;
	}

    private void Update()
    {
		SetVisibility();
    }

    public void PanelClicked()
	{
		if (canClick)
		{
			StartCoroutine(RegrowPanel());
		}
	}

	IEnumerator RegrowPanel()
	{
		isRegrowing = true;

		transform.localScale = new Vector3(0, 0, 0);
		yield return new WaitForSeconds(1f);

		float scaleIncr = 0.01f;
		for (int i = 0; i <= (int)(1.0f / scaleIncr); i++)
		{
			transform.localScale = new Vector3(i * scaleIncr, i * scaleIncr, i * scaleIncr);
			yield return new WaitForSeconds(0.05f);
		}

		isRegrowing = false;
	}

	bool IsVisible()
	{
		Vector3 pos = Camera.main.WorldToViewportPoint(transform.parent.position + relativePos);
		return pos.x > 0 && pos.x < 1 && pos.y > 0 && pos.y < 1 && pos.z > 0;
	}

    void SetVisibility()
    {
		//return;
        if (!IsVisible())
        {
            return;
        }

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.parent.position + relativePos);
        string currSemantic = SinglePlayerSemanticsManager.instance.GetPixelMaterial((int)screenPos.x, (int)screenPos.y);
		//for (int i = 0; i < 3 && currSemantic != matType; i++)
		//{
		//    for (int j = 0; j < 2 && currSemantic != matType; j++)
		//    {
		//        screenPos = Camera.main.WorldToScreenPoint(transform.GetChild(3).GetChild(i).GetChild(j).position);
		//        currSemantic = query.IsPixelMaterial((int)screenPos.x, (int)screenPos.y);
		//    }
		//}

		if (currSemantic != matType)
        {
			GetComponent<MeshRenderer>().enabled = false;
			canClick = false;
		}
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
            if (!isRegrowing && gameObject.layer == 8)
            {
				canClick = true;
			}
            else
            {
				canClick = false;
            }
			//if (matType == "foliage")
        //    {
         //       Vector3 worldPos = DepthManager.instance.GetWorldPosition(screenPos.x, screenPos.y);
         //       transform.position = worldPos;
         //   }
        }
    }
}


