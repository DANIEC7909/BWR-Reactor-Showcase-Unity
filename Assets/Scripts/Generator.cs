using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : Transportable
{
    public int PressureTreshold;
    public int TemperatureTreshold;
    public int ProducedPower;
    public int SteamCostPerTick=2;
    public override void Tick()
    {
        if(Pressure> PressureTreshold&&Amount>SteamCostPerTick && Temperature>TemperatureTreshold&&FluidType==FluidType.Steam)
        {
            Amount -= SteamCostPerTick;
            ProducedPower += SteamCostPerTick;
        }
    }
}
