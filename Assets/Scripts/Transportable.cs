
using UnityEngine;

public enum FluidType
{
    None = 0 , Water = 1, HevyWater = 2, Steam = 3, Heat = 4
}
public enum TransportableType
{
    Pipe = 0, Pump = 1, Tank = 2, NuclearReactor = 3
}
[System.Serializable]
public struct Connection
{
    public Transform InletTransform;
    public Transform OutletTransform;
}
public class Transportable : Buildable
{
    public bool EnableFluidTransport = true;
    public Transportable Outlet;
    public int Amount = 0;
    public int MaxAmount = 100;
    public int TransportValuePerTick = 5;
    public float Temperature = 0;
    public int Pressure = 0;
    public int PressureTransportTreshold = 10;
    public int PressureTransportTresholdToBroke = 250;
    public int PressureDropValue = 5;
    public FluidType FluidType;
    public Connection Connection;
    public TransportableType TransportableType;
    public int TemperatureDrop = 1;
    /// <summary>
    /// This field declare is transportable have to care about transportation or doo basic stuff.
    /// </summary>
    protected bool DoBasicLoop;
    /// <summary>
    /// Power that this object requires to work propairly. If power is not required leave it as 0.
    /// </summary>
    public int Power;

    private void OnEnable()
    {
        GameEvents.Instance.OnFluidTick += Instance_OnFluidTick;
    }



    private void OnDisable()
    {
        GameEvents.Instance.OnFluidTick -= Instance_OnFluidTick;
    }

    private void Instance_OnFluidTick()
    {
        Tick();
    }
    public virtual void Broke()
    {
#if UNITY_EDITOR

        Debug.Log("Pipeline Is Broken !");
#endif
    }
    public virtual void Tick()
    {
        if (EnableFluidTransport)
        {
            if (!Outlet)
                return;
            if (Pressure < PressureTransportTreshold)
                return;
            if (Pressure > PressureTransportTresholdToBroke)
            {
                Broke();
                return;
            }
            if (FluidType == Outlet.FluidType || Outlet.FluidType == FluidType.None)
            {

                if (!DoBasicLoop) return;

                if (Amount > 0 && Outlet.Amount + TransportValuePerTick <= Outlet.MaxAmount)
                {
                    Amount -= TransportValuePerTick;
                    Outlet.Amount += TransportValuePerTick;

                }
                //Here we basically calculate temperature over distance and we check is temp is not less than level temperature.
                Outlet.Temperature = Temperature > GameController.Instance.CurrentEnviormentTemperature ? Temperature - TemperatureDrop : GameController.Instance.CurrentEnviormentTemperature;
                
                if (TransportableType != TransportableType.Pump && Outlet.TransportableType != TransportableType.Pump)
                {
                    Outlet.Pressure = Pressure - 5;
                }

            }
            else
            {
                //wrong fluid type is connected to pipe
            }
        }
    }
}
