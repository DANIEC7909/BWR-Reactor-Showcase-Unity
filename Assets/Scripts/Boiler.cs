using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;

public class Boiler : Buildable
{
    public Transportable WaterInlet;
    public Transportable HeatInlet;
    public Transportable SteamOutlet;

    
    [SerializeField] int WaterLevelToProduceSteam;
    [SerializeField] int HeatLevelToProduceSteam;

    [SerializeField] int TemperatureToProduceSteam;
    [SerializeField] int ProducedSteamAmount;
    [SerializeField] int PressureAfterProducedSteam;
    private void OnEnable()
    {
        GameEvents.Instance.OnFluidTick += Tick;
        GameController.Instance.BuildingManager.PlacedBuildings.Add(WaterInlet);
        GameController.Instance.BuildingManager.PlacedBuildings.Add(HeatInlet);
        GameController.Instance.BuildingManager.PlacedBuildings.Add(SteamOutlet);
    }
    private void OnDisable()
    {
        GameEvents.Instance.OnFluidTick -= Tick;
    }
    
    private void Tick()
    {
        Profiler.BeginSample("sf.Boiler");
                SteamOutlet.Temperature = (HeatInlet.Temperature - (HeatInlet.Amount * 0.2f));
        if (HeatInlet.Temperature >= TemperatureToProduceSteam &&SteamOutlet.Amount+ ProducedSteamAmount < SteamOutlet.MaxAmount)
        {
            if (  WaterInlet.Amount >= WaterLevelToProduceSteam &&   HeatInlet.Amount>= HeatLevelToProduceSteam)
            {
                WaterInlet.Amount -= WaterLevelToProduceSteam;
                HeatInlet.Amount -= HeatLevelToProduceSteam;
                SteamOutlet.Amount += ProducedSteamAmount;
                SteamOutlet.FluidType = FluidType.Steam;
                SteamOutlet.Pressure = PressureAfterProducedSteam;
            }
        }
        else
        {
            Debug.Log("[Boiler] Temperature is too low");
        }
        Profiler.EndSample();
    }
}
