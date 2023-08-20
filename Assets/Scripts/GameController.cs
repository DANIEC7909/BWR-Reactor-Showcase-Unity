using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public sealed class GameController : Singleton<GameController>
{
    public PlayerInput Input;
    const int SCENE_ID_ONLY_UI = 1;
    const int GENERAL_UI_ACTIONMAP = 1;

    const string BUILDING_TOGGLE_ACTION_KEY = "ToggleBuildingMode";

    public bool BuildingModeState;

    public BuildingManager BuildingManager;

    public InputActionMap GeneralUiActionMap;
    public InputAction BuildingToggleAction;
    private void OnEnable()
    {
        Init(this);
        GameEvents.Instance.OnBuildingModeToggle += Instance_OnBuildingModeToggle;
        GeneralUiActionMap = Input.actions.actionMaps[GENERAL_UI_ACTIONMAP];
        BuildingToggleAction = GeneralUiActionMap.FindAction(BUILDING_TOGGLE_ACTION_KEY);
        GeneralUiActionMap.Enable();
        BuildingToggleAction.performed += BuildingToggleAction_performed;

    }

    private void BuildingToggleAction_performed(InputAction.CallbackContext obj)
    {
        BuildingModeState = !BuildingModeState;
        GameEvents.Instance.OnBuildingModeChanged_C(BuildingModeState);
   
    }

    private void OnDisable()
    {
        GameEvents.Instance.OnBuildingModeToggle -= Instance_OnBuildingModeToggle;
        GeneralUiActionMap.Disable();
        BuildingToggleAction.performed -= BuildingToggleAction_performed;
    }
    private void Instance_OnBuildingModeToggle(bool b)
    {
        BuildingManager.enabled = b;
    }

    void Start()
    {
        Input.GetComponent<PlayerInput>();
        SceneLoadingQueue();
    }
    public void SceneLoadingQueue()
    {
        SceneManager.LoadSceneAsync(SCENE_ID_ONLY_UI, LoadSceneMode.Additive);
        SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive);

    }

    void Update()
    {
    }

    private void FixedUpdate()
    {
        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.OnFluidTick_C();
        }
    }
}
