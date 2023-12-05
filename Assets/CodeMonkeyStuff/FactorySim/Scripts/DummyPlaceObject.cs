using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyPlaceObject : MonoBehaviour {

    [SerializeField] private PlacedObjectTypeSO placedObjectTypeSO;


    private void Start() {
        Vector2Int gridPosition = GridBuildingSystem.Instance.GetGridPosition(transform.position);
        GridBuildingSystem.Instance.TryPlaceObject(gridPosition, placedObjectTypeSO, PlacedObjectTypeSO.Dir.Down);

        Destroy(gameObject);
    }

}
