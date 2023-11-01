using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public struct ElectricPolePowerGroup
{
    public int EPGroupID;
    public int Power;
    public List<ElectricalPole> ConnectedPoles;
    public ElectricPolePowerGroup(int ID)
    {
        EPGroupID = ID;
        Power = 0;
        ConnectedPoles = new List<ElectricalPole>();
    }
}
