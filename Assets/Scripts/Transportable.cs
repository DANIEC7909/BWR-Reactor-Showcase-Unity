
using UnityEngine;

public enum FluidType
{
    Water, HevyWater, Steam
}
public enum TransportableType
{
    Pipe=0, Pump=1, Tank=2
}
[System.Serializable]
public struct Connection
{
    public Transform InletTransform;
    public Transform OutletTransform;
}
public class Transportable : Buildable
{
    public bool EnableFluidTransport=true; 
    public Transportable Outlet;
    public int Amount = 0;
    public int MaxAmount = 100;
    public int TransportValuePerTick = 5;
    public int Temperature = 0;
    public int Pressure = 0;
    public int PressureTransportTreshold = 10;
    public int PressureTransportTresholdToBroke = 250;
    public int PressureDropValue = 5;
    public FluidType FluidType;
    public Connection Connection;
    public TransportableType TransportableType;

   

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

            if (Amount > 0 && Outlet.Amount + TransportValuePerTick <= Outlet.MaxAmount)
            {
                Amount -= TransportValuePerTick;
                Outlet.Amount += TransportValuePerTick;

            }
            if (TransportableType != TransportableType.Pump&&Outlet.TransportableType!=TransportableType.Pump)
            {
                Outlet.Pressure = Pressure - 5;
            }
        }
    }
}
