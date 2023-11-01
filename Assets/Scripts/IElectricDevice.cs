using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IElectricDevice
{
     public int Power { get; set; }
     public ElectricalPole ConnectedPole { get; set; }
}
