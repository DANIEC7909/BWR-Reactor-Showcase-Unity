using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pump : Transportable
{
    [SerializeField] Animator Anim;
    public int AddativePressure=50;
    public override void Tick()
    {
        base.Tick();
        if (Amount > 0 && Amount<100)
        {
        Anim.SetBool("pump", true);
        }
        else
        {
        Anim.SetBool("pump", false);
        }
        Pressure = AddativePressure;
        if (Outlet)
        {
            Outlet.Pressure = Pressure + AddativePressure;
        }
    }
}
