using UnityEditor;
using UnityEngine;

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
        DoBasicLoop = true;
        FluidType = FluidType.Heat;
        TransportableType = TransportableType.NuclearReactor;
    }
    public override void Tick()
    {
        base.Tick();
        if (Temperature < MeltingTemperatureTreshold)
        {
            if (Fuel - HeatCostInFuel >= 0)
            {
                currentTimeToProduceHeat -= Time.deltaTime;
                if (currentTimeToProduceHeat < TimeToProduceHeat)//Action tick when we can do all calculation about energy production.
                {
                    //TODO: Doda� �e jak w okre�lonym czasie ilo�� ciep�a nie jest odbierana na czas zwi�ksza si� samoistnie przyrost temperatury.
                    Temperature += Mathf.Lerp(ControlRodsMaxMultiplier, 0, ControlRodsInProcentage);
                    Fuel -= HeatCostInFuel;
                    if (Temperature >= TemperatureTresholdToProduceHeat)
                    {
                    Amount += 100;
                        Debug.Log("Reactor heated up!");
                    }
                    else
                    {
                        Debug.Log("Reactor is heating UP!...");
                    }
                  
                    currentTimeToProduceHeat = TimeToProduceHeat;
                }
            }
            else
            {
                //not enough fuel.
                Debug.Log("Not enough fuel!");
            }
        }
        else
        {
            //broke
            Debug.Log("Reactor blew up!");
        }
    }
}