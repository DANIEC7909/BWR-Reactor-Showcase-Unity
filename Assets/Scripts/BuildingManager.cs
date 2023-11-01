using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildingManager : MonoBehaviour
{
    Vector2 PointerPosition;
    const int CAMERA_INPUT_ACTIONMAP = 0;
    const string CAMERA_POSITION_BINDING = "PointerPosition";
    const string CAMERA_DESTROY_BLOCK_BINDING = "DestroyBlock";
    const string CAMERA_SUBMIT_BINDING = "Submit";
    const string CAMERA_ROTATE_BINDING = "RotateBlock";
    const string BUILDING_ALLOWED_LAYER = "Ground";
    InputAction PointerPositionAction;
    InputAction SubmitAction;
    InputAction RotateAction;
    InputAction DestroyBlockAction;
    GameObject Ghost;
    Vector3 BuildPointerPostion;
    public List<Buildable> Buildables;
    RaycastHit BuildingRay;
    public List<Buildable> PlacedBuildings;
    public int BuildingID;
    public float ConnectionDistanceTreshold = 5;
    public bool DestroyMode;

    public Material ElectricPoleLineMaterial;

    private void Start()
    {
        GameController.Instance.BuildingManager = this;
        PointerPositionAction = GameController.Instance.Input.actions.actionMaps[CAMERA_INPUT_ACTIONMAP].FindAction(CAMERA_POSITION_BINDING);
        SubmitAction = GameController.Instance.Input.actions.actionMaps[CAMERA_INPUT_ACTIONMAP].FindAction(CAMERA_SUBMIT_BINDING);
        RotateAction = GameController.Instance.Input.actions.actionMaps[CAMERA_INPUT_ACTIONMAP].FindAction(CAMERA_ROTATE_BINDING);
        DestroyBlockAction = GameController.Instance.Input.actions.actionMaps[CAMERA_INPUT_ACTIONMAP].FindAction(CAMERA_DESTROY_BLOCK_BINDING);
        DestroyBlockAction.started += DestroyBlockAction_started;
        DestroyBlockAction.canceled += DestroyBlockAction_canceled;
        SubmitAction.performed += SubmitAction_performed;
        RotateAction.performed += RotateAction_performed;

    }

    private void DestroyBlockAction_canceled(InputAction.CallbackContext obj)
    {
        DestroyMode = false;
    }

    private void DestroyBlockAction_started(InputAction.CallbackContext obj)
    {
        DestroyMode = true;
    }

    private void RotateAction_performed(InputAction.CallbackContext obj)
    {
        if (Ghost)
        {
            Ghost.transform.Rotate(0, 90, 0);
        }
    }

    private void SubmitAction_performed(InputAction.CallbackContext obj)
    {
        //STATE CHECK
        if (DestroyMode)
        {
            DestroyBlock();
        }
        else
        {
            Build();
        }
    }

    private void OnDisable()
    {
        DestroyBlockAction.started -= DestroyBlockAction_started;
        DestroyBlockAction.canceled -= DestroyBlockAction_canceled;
        SubmitAction.performed -= SubmitAction_performed; RotateAction.performed -= RotateAction_performed;
        if (Ghost) Destroy(Ghost);
    }
    public void DestroyBlock()
    {
        Buildable go = BuildingRay.collider.gameObject.GetComponentInParent<Buildable>();
        PlacedBuildings.Remove(go);
        Destroy(go.gameObject);
    }
    public void Build()
    {
        if (Ghost != null)
        {

            Buildable building = Instantiate(Buildables[BuildingID].gameObject, Ghost.transform.position, Ghost.transform.rotation).GetComponent<Buildable>();
            GameEvents.Instance.OnBuildingPlaced_C(building);
            PlacedBuildings.Add(building);
            if (building is Transportable)
            {
                Transportable currentlyPlacedTransportable = (Transportable)building;
                if (currentlyPlacedTransportable)
                {
                    foreach (Buildable bul in PlacedBuildings)
                    {

                        if (!bul)
                        {
                            Debug.LogError("Here we have buildable null so i skip it for now.");
                            continue;
                        }
                        if (!(bul is Transportable))
                        {
                            continue;
                        }
                        Transportable lastPlaced = (Transportable)bul;
                        if (lastPlaced)
                        {
                            if (lastPlaced == currentlyPlacedTransportable)
                            {
                                continue;
                            }
                            /* if (!lastPlaced.Outlet)
                             {*/
                            if (currentlyPlacedTransportable.Connection.InletTransform && lastPlaced.Connection.OutletTransform)
                            {
                                Debug.Log(Vector3.Distance(currentlyPlacedTransportable.Connection.InletTransform.position, lastPlaced.Connection.OutletTransform.position));
                                if (Vector3.Distance(currentlyPlacedTransportable.Connection.InletTransform.position, lastPlaced.Connection.OutletTransform.position) < ConnectionDistanceTreshold)
                                {
                                    lastPlaced.Outlet = currentlyPlacedTransportable;
                                    Debug.Log("Connected!  lastPlaced.Outlet" + Vector3.Distance(currentlyPlacedTransportable.Connection.InletTransform.position, lastPlaced.Connection.OutletTransform.position));
                                }
                            }

                            //}
                            if (currentlyPlacedTransportable.Connection.OutletTransform && lastPlaced.Connection.InletTransform)
                            {
                                if (Vector3.Distance(currentlyPlacedTransportable.Connection.OutletTransform.position, lastPlaced.Connection.InletTransform.position) < ConnectionDistanceTreshold)
                                {
                                    currentlyPlacedTransportable.Outlet = lastPlaced;
                                    // Debug.Log("Connected!  currentlyPlacedTransportable.Outlet" + Vector3.Distance(currentlyPlacedTransportable.Connection.InletTransform.position, lastPlaced.Connection.OutletTransform.position));
                                }
                            }


                        }
                    }
                }
                else
                {
                    return;
                }
            }
            if (building is ElectricalPole)
            {
#if UNITY_EDITOR
                Debug.Log("Buildable is ElectricPole");
#endif

                bool isPoleConnected = false;
                ElectricalPole CurrentlyPlacedEP = (ElectricalPole)building;
                //we have to check distannce to other poles
                foreach (Buildable ep in PlacedBuildings)
                {
                    if (ep is ElectricalPole)
                    {
                        ElectricalPole LastPlacedBuilding = (ElectricalPole)ep;
                        if (Vector3.Distance(CurrentlyPlacedEP.transform.position, ep.transform.position) < CurrentlyPlacedEP.ConnectionDistance && building != ep)
                        //if we are sourronded by any pole take its group
                        {
#if UNITY_EDITOR
                            Debug.Log("Im sourrounded by e poles");
#endif
                            int epgid = 0;
                            if (GameController.Instance.ElectricPolePowerGroups.Count > 0)
                            {
                                LastPlacedBuilding.GetEPGroup().ConnectedPoles.Add(CurrentlyPlacedEP);
                                epgid = LastPlacedBuilding.GetEPGroup().EPGroupID;
                            }
                            else
                            {

                                ElectricPolePowerGroup epg = new ElectricPolePowerGroup(GameController.Instance.ElectricPolePowerGroups.Count);

                                GameController.Instance.ElectricPolePowerGroups.Add(epg);
                                epgid = GameController.Instance.ElectricPolePowerGroups.IndexOf(epg);
                            }
                            CurrentlyPlacedEP.EPGroupID = epgid;

                            isPoleConnected = true;
                            break;
                        }
                    }
                }
                if (isPoleConnected == false)
                //if not make ne group and pass it to pole 
                {
                    Debug.Log("not sourrounded ");
                    ElectricPolePowerGroup epg = new ElectricPolePowerGroup(GameController.Instance.ElectricPolePowerGroups.Count);

                    GameController.Instance.ElectricPolePowerGroups.Add(epg);

                    CurrentlyPlacedEP.EPGroupID = epg.EPGroupID;
                    CurrentlyPlacedEP.GetEPGroup().ConnectedPoles.Add(CurrentlyPlacedEP);


                }

                List<Vector3> pos = new List<Vector3>();

                foreach (ElectricalPole ep in CurrentlyPlacedEP.GetEPGroup().ConnectedPoles)
                {
                    /*CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer.SetPosition(index, ep.transform.position+Vector3.up*2);
                     index++;*/
                    pos.Add(ep.transform.position + Vector3.up * 2);

                }
                if (CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer == null)
                {
                    CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer = CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].gameObject.AddComponent<LineRenderer>();//.LineRenderer.SetPositions(pos.ToArray());
                }                                                                            //CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer.SetVertexCount(pos.ToArray().Length);
                CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer.material = ElectricPoleLineMaterial;
                CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer.SetVertexCount(pos.ToArray().Length);
                CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer.SetWidth(0.2f, 0.2f);
                CurrentlyPlacedEP.GetEPGroup().ConnectedPoles[0].LineRenderer.SetPositions(pos.ToArray());



            }

        }
    }
    public void SelectBuilding(int _BuildingID)
    {
        if (Ghost)
        {
            Destroy(Ghost);
        }
        Ghost = Instantiate(Buildables[_BuildingID].gameObject, PointerPosition, Quaternion.identity);
        //apply Ghost material.
        BuildingID = _BuildingID;
        //remove logic from ghost 
        Buildable buidable = Ghost.GetComponent<Buildable>();
        Ghost.GetComponentInChildren<BoxCollider>().enabled = false;
        if (buidable)
        {
            Destroy(buidable);
        }
        Ghost.name = "Ghost";
    }

    void Update()
    {
        PointerPosition = PointerPositionAction.ReadValue<Vector2>();

        Ray ray = Camera.main.ScreenPointToRay(PointerPosition);

        if (Physics.Raycast(ray, out BuildingRay))
        {
            BuildPointerPostion = new Vector3(Mathf.FloorToInt(BuildingRay.point.x), BuildingRay.point.y, Mathf.FloorToInt(BuildingRay.point.z));
        }

        if (Ghost)
        {
            Ghost.transform.position = new Vector3(BuildPointerPostion.x, 0, BuildPointerPostion.z);
        }
    }
}
