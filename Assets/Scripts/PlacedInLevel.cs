using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacedInLevel : MonoBehaviour
{
    void Start()
    {
        GameController.Instance.BuildingManager.PlacedBuildings.Add(GetComponent<Buildable>());
    }

  
}
