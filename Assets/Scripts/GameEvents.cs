using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameEvents : Singleton<GameEvents>
{
    void Awake()
    {
        Init(this);
    }
    public delegate void FluidTick();
    public event FluidTick OnFluidTick;
    public void OnFluidTick_C()
    {
        OnFluidTick?.Invoke();
    }
    public delegate void BuildingPlaced(Buildable bb);
    public event BuildingPlaced OnBuildingPlaced;
    public void OnBuildingPlaced_C(Buildable bb)
    {
        OnBuildingPlaced?.Invoke(bb);
    }
    public delegate void BuildingMode(bool b);
    public event BuildingMode OnBuildingModeToggle;
    public void OnBuildingModeChanged_C(bool b)
    {
        OnBuildingModeToggle?.Invoke(b);
    }
}
