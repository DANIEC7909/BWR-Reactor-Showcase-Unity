using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BuildMenuUI : MonoBehaviour
{
    [SerializeField] Transform Destination;
    [SerializeField] Button ButtonPrefab;
    void OnEnable()
    {
      
     foreach(Buildable buildable in GameController.Instance.BuildingManager.Buildables)
        {
          GameObject go = Instantiate(ButtonPrefab.gameObject, Destination);
            go.GetComponentInChildren<TextMeshProUGUI>().text = buildable.Name;
            go.GetComponent<Button>().onClick.AddListener(() => GameController.Instance.BuildingManager.SelectBuilding(GameController.Instance.BuildingManager.Buildables.IndexOf(buildable)));
        }
    }
    private void OnDisable()
    {
        
    }

}
