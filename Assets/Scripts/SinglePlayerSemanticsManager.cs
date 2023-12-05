using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using TMPro;
using Niantic.Lightship.AR.Semantics;
using UnityEngine.XR.ARFoundation;

public class SinglePlayerSemanticsManager : MonoBehaviour
{
	public static SinglePlayerSemanticsManager instance;

	public ARSemanticSegmentationManager _semanticManager;

	public GameObject[] sprites;
	public string[] spriteNames;
    private Dictionary<string, GameObject> spriteDict;
    
    private string currSemantic;
	private bool okToSpawn;
	private int collectLimit;
	private Regex regex;
	private bool infinResources;
	private bool touchingActive;
   
    //[SerializeField]
    //private TextMeshProUGUI[] spriteCounts;

    //[SerializeField]
    //private TextMeshProUGUI[] spriteMenuCounts;

    //[SerializeField]
    //private RawImage[] overlays;

    //private Texture2D[] masks = new Texture2D[5];

    private void Awake()
    {
		if (instance == null)
		{
			instance = this;
		}
	}

    void Start()
	{
        InitVars();
    }

    private void Update()
    {
        HandleTouch();
	}

    private void InitVars()
    {
        regex = new Regex("([a-z]+)([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        infinResources = false;
        collectLimit = 999;

        currSemantic = "";
        okToSpawn = true;

        spriteDict = new Dictionary<string, GameObject>();
        for (int idx = 0; idx < sprites.Length; idx++)
        {
            spriteDict.Add(spriteNames[idx], sprites[idx]);
        }
    }

    private void HandleTouch()
    {
//#if UNITY_EDITOR
        if (!Input.GetMouseButtonDown(0))
//#else
//            if (Input.touchCount <= 0)
//#endif
        {
            currSemantic = "";
            return;
        }

//#if UNITY_EDITOR
        var touchPos = Input.mousePosition;
        //touchingActive = true;
//#else
//		var touch = Input.touches[0];
//		var touchPos = touch.position;
//		if (touch.phase == TouchPhase.Began)
//        {
//            touchingActive = true;
//        }
//        if (touch.phase == TouchPhase.Ended)
//        {
//            touchingActive = false;
//        }
//#endif

        if (EventSystem.current.currentSelectedGameObject != null)
        {
            return;
        }

        Ray ray = Camera.main.ScreenPointToRay(touchPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100) && hit.transform.gameObject.layer == 8)
        {
            var panel = hit.transform.parent.gameObject.GetComponent<MyPanel>();
            if (panel.canClick)
            {
                panel.PanelClicked();
                currSemantic = panel.matType;
                GameObject.Find("InventoryController").GetComponent<InventoryController_GridBuilding>().ResourceCollected(panel.gameObject);
                if (okToSpawn)
                {
                    okToSpawn = false;
                    StartCoroutine(SpawnSprite(touchPos, spriteDict[currSemantic]));
                }
            }
        }
    }

    public string GetPixelMaterial(int x, int y)
	{
		if (!_semanticManager.subsystem.running)
		{
			return "";
		}

		var channelsNamesInPixel = _semanticManager.GetChannelNamesAt(x, y);
		if (channelsNamesInPixel.Count > 0)
		{
			string currSemantic = channelsNamesInPixel[0];
			foreach (var name in spriteNames)
			{
				if (currSemantic == name)
				{
					return currSemantic;
				}
			}
		}
		return "";
	}

    //public void Reset()
    //{
    //    // TODO: inventory?
    //    //for (int idx = 0; idx < sprites.Length; idx++)
    //    //{
    //    //	inventory[idx] = 0;
    //    //}
    //    //for (int idx = 0; idx < spriteCounts.Length; idx++)
    //    //{
    //    //	spriteCounts[idx].text = inventory[idx].ToString();
    //    //	if (spriteMenuCounts[idx] != null)
    //    //	{
    //    //		spriteMenuCounts[idx].text = inventory[idx].ToString();
    //    //	}
    //    //}
    //    infinResources = false;
    //    currSemantic = "";
    //    okToSpawn = true;
    //    touchingActive = false;
    //}

    //public int GetAmount(string name)
    //{
    //	MatchCollection matches = regex.Matches(name);
    //	GroupCollection groups = matches[0].Groups;

    //	string semName = groups[1].Value;
    //	int numNeeded = Int32.Parse(groups[2].Value);
    //	int semIdx = spriteDict[semName];

    //	return infinResources ? -1 : inventory[semIdx];
    //}

    //public void SetInfiniteResources(bool status)
    //{
    //	infinResources = status;
    //	if (infinResources)
    //	{
    //		for (int idx = 0; idx < spriteCounts.Length; idx++)
    //		{
    //			spriteCounts[idx].text = "\u221E";
    //			if (spriteMenuCounts[idx] != null)
    //			{
    //				spriteMenuCounts[idx].text = "\u221E";
    //			}
    //		}
    //	}
    //	else
    //	{
    //		for (int idx = 0; idx < spriteCounts.Length; idx++)
    //		{
    //			spriteCounts[idx].text = inventory[idx].ToString();
    //			if (spriteMenuCounts[idx] != null)
    //			{
    //				spriteMenuCounts[idx].text = inventory[idx].ToString();
    //			}
    //		}
    //	}
    //}

    //public int GetCost(int idx)
    //{
    //	return controller.GetCost(idx);
    //}

    //public int GetPoints(int idx)
    //{
    //	return controller.GetPoints(idx);
    //}



    //public bool QueryInventory(string name, bool decrement)
    //{
    //	MatchCollection matches = regex.Matches(name);
    //	GroupCollection groups = matches[0].Groups;

    //	string semName = groups[1].Value;
    //	Debug.Log("NUM IDX: " + Int32.Parse(groups[2].Value));
    //	int numNeeded = controller.GetCost(Int32.Parse(groups[2].Value));
    //	int semIdx = spriteDict[semName];

    //	if (infinResources)
    //	{
    //		return true;
    //	}
    //	Debug.Log("NUM NEEDED: " + numNeeded + ", AMOUNT LEFT: " + inventory[semIdx]);
    //	if (numNeeded <= inventory[semIdx])
    //	{
    //		Debug.Log("NUM ABOUT TO DECREMENT");
    //		if (decrement)
    //		{
    //			inventory[semIdx] = inventory[semIdx] - numNeeded;
    //			spriteCounts[semIdx].text = inventory[semIdx].ToString();
    //			if (spriteMenuCounts[semIdx] != null)
    //			{
    //				spriteMenuCounts[semIdx].text = inventory[semIdx].ToString();
    //			}
    //		}
    //		return true;
    //	}

    //	return false;
    //}

    //public void FreeWood(int amount)
    //{
    //	int semIdx = spriteDict["foliage"];
    //	inventory[semIdx] += amount;
    //	spriteCounts[semIdx].text = inventory[semIdx].ToString();
    //	if (spriteMenuCounts[semIdx] != null)
    //	{
    //		spriteMenuCounts[semIdx].text = inventory[semIdx].ToString();
    //	}
    //}

    //public void FreeRock(int amount)
    //{
    //	int semIdx = spriteDict["ground"];
    //	inventory[semIdx] += amount;
    //	spriteCounts[semIdx].text = inventory[semIdx].ToString();
    //	if (spriteMenuCounts[semIdx] != null)
    //	{
    //		spriteMenuCounts[semIdx].text = inventory[semIdx].ToString();
    //	}
    //}

    IEnumerator SpawnSprite(Vector2 touch, GameObject sprite)
    {
        Vector3 pos = new Vector3(touch.x, touch.y, 0.5f);
        pos = Camera.main.ScreenToWorldPoint(pos);
        GameObject currSprite = Instantiate(sprite, pos, Quaternion.identity);
        currSprite.transform.SetParent(Camera.main.transform);
        currSprite.transform.localRotation = Quaternion.Euler(0, 0, 0);
        currSprite.transform.localScale = new Vector3(0.02F, 0.02F, 0.02F);
        int idx;
        float yvel = 0.01F;
        float xvel = 0.02F;
        AudioManager.Instance.PlayGather();
        yield return new WaitForSeconds(0.05F);
        for (idx = 0; idx < 10; idx++)
        {
            if (idx == 5)
            {
                okToSpawn = true;
            }
            currSprite.transform.Translate(xvel, yvel, -0.025F, Space.Self);
            xvel -= 0.001F;
            yvel -= 0.005F;
            yield return new WaitForSeconds(0.02F);
        }
        Destroy(currSprite);
    }
}