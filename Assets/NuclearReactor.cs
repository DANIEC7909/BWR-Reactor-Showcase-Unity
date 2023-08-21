using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NuclearReactor : Transportable
{
    public int MeltingTemperatureTreshold;
    public int Fuel;
    public int ControlRodsInProcentage;

    private void OnDrawGizmos()
    {
        if (Amount > 0)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.yellow;
        }
        Handles.Label(transform.position, Amount.ToString() + "/" + Pressure.ToString());
        Gizmos.DrawCube(transform.position + Vector3.up * 5, new Vector3(0.5f, 0.5f, 0.5f));
        if (Outlet)
        {
            if (Outlet.Amount > 0)
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }
            Gizmos.DrawLine(transform.position + Vector3.up * 5, Outlet.transform.position + Vector3.up * 5);
            Gizmos.DrawCube(Outlet.transform.position + Vector3.up * 5, new Vector3(0.5f, 0.5f, 0.5f));
        }
    }

    private void Start()
    {
        DoBasicLoop = true;
        FluidType = FluidType.Heat;
        TransportableType = TransportableType.NuclearReactor;
    }
    public override void Tick()
    {
        base.Tick();
        
    }
}
