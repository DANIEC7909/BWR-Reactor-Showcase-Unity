using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boiler : Buildable
{
    public Transportable WaterInlet;
    public Transportable HeatInlet;
    public Transportable SteamOutlet;
    private void OnEnable()
    {
        GameEvents.Instance.OnFluidTick += Tick;
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnFluidTick -= Tick;
    }

    private void Tick()
    {
       
    }
}
