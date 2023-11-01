using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectricalPole : Buildable
{
    public int EPGroupID=-1;
    public int ConnectionDistance = 15;
  //  public List<ElectricalPole> ConnectedPoles = new List<ElectricalPole>();
    public List<IElectricDevice> electricDevices;
    public LineRenderer LineRenderer;
    public ElectricPolePowerGroup GetEPGroup()
    {
#if UNITY_EDITOR
        if (EPGroupID == -1)
        {
            
            Debug.LogError("EP Group not Found.");
        }
#endif
            return GameController.Instance.ElectricPolePowerGroups[EPGroupID];
    }
    public bool IsAbleToGetPowerFromGrid(int amount)
    { 
        return (GameController.Instance.ElectricPolePowerGroups[EPGroupID].Power - amount > 0) ? true : false;
    }




}
