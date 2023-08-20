using UnityEngine;
using UnityEngine.InputSystem;
public class GameCamera : Singleton<GameCamera>
{
    public PlayerInput Input;
    const int CAMERA_ACTIONMAP = 0;
    const string MOVE_ACTION_KEY = "Move";
    const string SPRINT_ACTION_KEY = "Sprint";
    const string ROLL_ACTION_KEY = "Roll";
    public float SprintSpeed = 10;
    public float NormalSpeed = 5;
    InputActionMap CameraActionMap;
    InputAction moveAction;
    InputAction sprintAction;
    InputAction rollAction;
    public Camera Camera;

    void Start()
    {
        Init(this);
        Input = GameController.Instance.Input;

        CameraActionMap = Input.actions.actionMaps[CAMERA_ACTIONMAP];
        CameraActionMap.Enable();

        if (!Camera)
        {
            Camera = GetComponentInChildren<Camera>();
        }
        
        moveAction = CameraActionMap.FindAction(MOVE_ACTION_KEY);
        sprintAction = CameraActionMap.FindAction(SPRINT_ACTION_KEY);
        rollAction = CameraActionMap.FindAction(ROLL_ACTION_KEY);

    }


    private void OnDisable()
    {

        CameraActionMap.Disable();
    }

    void FixedUpdate()
    {

        Vector2 Axis = moveAction.ReadValue<Vector2>();
        transform.position += (transform.forward * Axis.y + transform.right * Axis.x) * (sprintAction.IsPressed() ? SprintSpeed : NormalSpeed);//new Vector3((Axis.x * Time.deltaTime * (sprintAction.IsPressed() ? SprintSpeed : NormalSpeed)), 0, (Axis.y * Time.deltaTime * (sprintAction.IsPressed() ? SprintSpeed : NormalSpeed)));

        transform.Rotate(new Vector3(0, rollAction.ReadValue<float>(), 0));


    }              
}
