using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;

public class NuclearReactor : Transportable
{
    [SerializeField] int MeltingTemperatureTreshold = 500;

    [SerializeField] float ControlRodsInProcentage;
    public int Fuel;
    [SerializeField] float TimeToProduceHeat;
    [SerializeField] float currentTimeToProduceHeat;
    [SerializeField] int HeatCostInFuel;
    [SerializeField] int TemperatureTresholdToProduceHeat;
    [SerializeField] float ControlRodsMaxMultiplier = 0.5f;
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
      //  DoBasicLoop = true;
        FluidType = FluidType.Heat;
        TransportableType = TransportableType.NuclearReactor;
    }
    public override void Tick()
    {
        Profiler.BeginSample("sf.NuclearReactor");
        base.Tick();
        if (Temperature < MeltingTemperatureTreshold)
        {
            if (Fuel - HeatCostInFuel >= 0)
            {
                currentTimeToProduceHeat -= Time.deltaTime;
                if (currentTimeToProduceHeat < TimeToProduceHeat)//Action tick when we can do all calculation about energy production.
                {
                    //TODO: Dodaæ ¿e jak w okreœlonym czasie iloœæ ciep³a nie jest odbierana na czas zwiêksza siê samoistnie przyrost temperatury.
                    if (ControlRodsInProcentage >=1&& Temperature>0)
                    {
                        Temperature -= (Time.deltaTime*0.5f);
                    }
                    else
                    {
                        Temperature += Mathf.Lerp(ControlRodsMaxMultiplier, 0, ControlRodsInProcentage);
                    }
                    Fuel -= HeatCostInFuel;
                    if (Temperature >= TemperatureTresholdToProduceHeat)
                    {
                    Amount += (int)(Temperature/HeatCostInFuel);
#if UNITY_EDITOR
                         Debug.Log("Reactor heated up!"+ (int)(Temperature / HeatCostInFuel));
#endif
                    }
                    else
                    {
#if UNITY_EDITOR
                            Debug.Log("Reactor is heating UP!...");
#endif
                    }
                  
                    currentTimeToProduceHeat = TimeToProduceHeat;
                }
            }
            else
            {
                if (Temperature>0)
                {
                    Temperature -= (Time.deltaTime * 0.5f);
                }
                //not enough fuel.
               // Debug.Log("Not enough fuel!");
            }
        }
        else
        {
            //broke
            Debug.Log("Reactor blew up!");
        }
        Profiler.EndSample();
    }
}
