using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceGrabbers : MonoBehaviour
{
    public Transform[] grabberTransforms;
    public ItemSO[] grabFilterItemSOs;
    public bool[] previousMustBeBelts;
    public bool[] nextMustBeBelts;

    private Grabber[] grabbers;

    public void Place()
    {
        grabbers = new Grabber[grabberTransforms.Length];
        for (int i = 0; i < grabberTransforms.Length; i++)
        {
            GridBuildingSystem gridBuildingSystem = GridBuildingSystem.Instance;
            Vector3 position = (grabberTransforms[i].position - transform.position) * gridBuildingSystem.scaling + transform.position;
            Vector2Int placedObjectOrigin = gridBuildingSystem.GetGridPosition(position);

            PlacedObjectTypeSO.Dir dir;
            switch (grabberTransforms[i].eulerAngles.y % 360)
            {
                default:
                case 0: dir = PlacedObjectTypeSO.Dir.Down; break;
                case 90: dir = PlacedObjectTypeSO.Dir.Left; break;
                case 180: dir = PlacedObjectTypeSO.Dir.Up; break;
                case 270: dir = PlacedObjectTypeSO.Dir.Right; break;
            }

            gridBuildingSystem.TryPlaceObject(placedObjectOrigin, GameAssets.i.placedObjectTypeSO_Refs.grabber, dir, out PlacedObject placedObject);
            grabbers[i] = placedObject as Grabber;
            grabbers[i].grabFilterItemSO = grabFilterItemSOs[i];
            grabbers[i].previousMushBeBelt = previousMustBeBelts[i];
            grabbers[i].nextMushBeBelt = nextMustBeBelts[i];
        }
    }

    public void DestorySelf()
    {
        for (int i = 0; i < grabberTransforms.Length; i++)
        {
            Destroy(grabbers[i].gameObject);
        }
    }
}
