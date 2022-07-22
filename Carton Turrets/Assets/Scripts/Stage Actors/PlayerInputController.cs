using UnityEngine;
using UnityEngine.InputSystem;


// public static class Input_Control_Events
// {
//     public static event System.Action MoveUpEvent;
//     public static void Invoke_Move_Up_Event(InputAction.CallbackContext context)
//     {
//         MoveUpEvent?.Invoke();
//     }

//     public static event System.Action MoveDownEvent;
//     public static void Invoke_Move_Down_Event(InputAction.CallbackContext context)
//     {
//         MoveDownEvent?.Invoke();
//     }

//     public static event System.Action MoveRightEvent;
//     public static void Invoke_Move_Right_Event(InputAction.CallbackContext context)
//     {
//         MoveRightEvent?.Invoke();
//     }

//     public static event System.Action MoveLeftEvent;
//     public static void Invoke_Move_Left_Event(InputAction.CallbackContext context)
//     {
//         MoveLeftEvent?.Invoke();
//     }

//     // public static event System.Action<Vector3> CameraViewEvent;
//     // public static void Invoke_cameraViewEvent(Vector3 cameraLoc)
//     // {
//     //     CameraViewEvent?.Invoke(cameraLoc);
//     // }
// }

public class PlayerInputController : MonoBehaviour
{
    // public static PlayerInputController singleton;

    // public MainControls PlayerInputActions;

    // private bool _cameraControlsAllowed;
    // private bool _cameraFollowAllowed;
    // public float CameraUpdateDelay;
    // public float CameraUpdateDelayCounter;


    // [Header("Events")]
    // private Vector3 _cameraLocUpdate;
    // public Vector3 CameraLocUpdate
    // {
    //     get{return _cameraLocUpdate;}
    //     set
    //     {
    //         _cameraLocUpdate = value;
    //         Input_Control_Events.Invoke_cameraViewEvent(value);
    //     }
    // }

    private void Awake()
    {
        // if (singleton == null)
        // {
        //     singleton = this;
        //     PlayerInputActions = new MainControls();
        // }
        // else
        // {
        //     Destroy(this.gameObject);
        //     return;
        // }
        // // DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // PlayerInputActions.MainMap.TapDown.started += context => Tap_Down(context);
        // PlayerInputActions.MainMap.TapUp.canceled += context => Tap_Up(context);
        // player_input_actions.PlayerControls.TapDrag.performed += context => Tap_Drag_Started(context);
        // player_input_actions.PlayerControls.TapDrag.canceled += context => Tap_Drag_Ended(context);
        // PlayerInputActions.MainMap.MoveUp.performed += context => Input_Control_Events.Invoke_Move_Up_Event(context);
        // PlayerInputActions.MainMap.MoveDown.performed += context => Input_Control_Events.Invoke_Move_Down_Event(context);
        // PlayerInputActions.MainMap.MoveRight.performed += context => Input_Control_Events.Invoke_Move_Right_Event(context);
        // PlayerInputActions.MainMap.MoveLeft.performed += context => Input_Control_Events.Invoke_Move_Left_Event(context);
    }

//     private void Update() //player controlled camera
//     { 
//         if(!camera_controls_allowed){return;}
//         SlowCameraUpdate();
//         if(drag_started){
//             drag_dist = drag_start -  player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
//             Vector3 cam_move = new Vector3(drag_dist.x, drag_dist.y, 0) * (camera_drag_speed /(10000 / -Camera.main.transform.position.z));
//             Camera.main.transform.position += new Vector3(cam_move.x,cam_move.y, 0);
//             Camera.main.transform.position = new Vector3(Mathf.Clamp(Camera.main.transform.position.x,Global_Vars.min_planet_coord,Global_Vars.max_planet_coord), Mathf.Clamp(Camera.main.transform.position.y,Global_Vars.min_planet_coord,Global_Vars.max_planet_coord), Camera.main.transform.position.z);
//             drag_start = player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
//         }
//         float z = player_input_actions.PlayerControls.Scroll.ReadValue<float>();
//         if (z > 0 && Camera.main.transform.position.z < -camera_max_zoom_in)
//         {
//             Camera.main.transform.position += new Vector3(0, 0, camera_zoom_speed + (-Camera.main.transform.position.z / 10));
//         }
//         else if (z < 0 && Camera.main.transform.position.z > -camera_max_zoom_out)
//         {
//             Camera.main.transform.position -= new Vector3(0, 0, camera_zoom_speed + (-Camera.main.transform.position.z / 10));
//         }
//     }

//     private void LateUpdate() //automatic camera
//     {
//         if(camera_controls_allowed){return;}
//         if(!camera_follow_allowed){return;}
//         SlowCameraUpdate();
//         Camera.main.transform.position = Vector3.MoveTowards
//             (
//                 Camera.main.transform.position,
//                 follow_target != null ? (follow_target.transform.position + desired_camera_pos_offset) : desired_camera_pos,
//                 (Vector3.Distance
//                     (
//                         Camera.main.transform.position,
//                         (follow_target != null ? (follow_target.transform.position + desired_camera_pos_offset) : desired_camera_pos + desired_camera_pos_offset)
//                     ) 
//                     *auto_camera_move_speed)
//             );
//     }

//     private void SlowCameraUpdate()
//     {
//         if(_cameraUpdateDelayCounter >= CameraUpdateDelay)
//         {   
//             _cameraUpdateDelayCounter = 0;
//             CameraLocUpdate = Camera.main.transform.position;
//             return;
//         }
//         _cameraUpdateDelayCounter += Time.deltaTime;
//     }

//     private void Tap_Down(InputAction.CallbackContext context){
//         RaycastHit hit;
//         Ray ray = Camera.main.ScreenPointToRay(player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>());
//         if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<TapableObject_Script>() != null)
//         {
//             tapped_object = hit.transform.gameObject.GetComponent<TapableObject_Script>();
//         }
//         drag_start = player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>();
//         drag_started = true;
//     }

//     private void Tap_Up(InputAction.CallbackContext context){
//         if (drag_started) {drag_started = false;}
//         if(tapped_object == null){return;}
//         RaycastHit hit;
//         Ray ray = Camera.main.ScreenPointToRay(player_input_actions.PlayerControls.TapPOS.ReadValue<Vector2>());
//         if (Physics.Raycast(ray, out hit) && hit.transform.gameObject.GetComponent<TapableObject_Script>() == tapped_object)
//         {
//             hit.transform.gameObject.GetComponent<TapableObject_Script>().Call_Tap_Event();
//             // print("hit "+ hit.transform.name);
//         }
//         tapped_object = null;
//     }

//     // void Update () {
//     //     if (drag_started) {
//     //         drag_start = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera_z_offset);
//     //         drag_start = Camera.main.ScreenToWorldPoint (drag_start);
//     //         Camera.main.transform.position = drag_start;

//     //     } 
//     //     else if (drag_started) {
//     //         var MouseMove = new Vector3(Input.mousePosition.x, Input.mousePosition.y, camera_z_offset);
//     //         MouseMove = Camera.main.ScreenToWorldPoint (MouseMove);
//     //         MouseMove.z = transform.position.z;
//     //         transform.position = transform.position - (MouseMove - drag_start);
//     //     }
// // }

//     public void Toggle_On_Screen_Controls()
//     {
//         if(on_screen_controls_allowed == true)
//         {
//             left_on_screen_controlls_container.SetActive(true);
//             right_on_screen_controlls_container.SetActive(true);
//             on_screen_controls_allowed = true;
//             return;
//             }
//         left_on_screen_controlls_container.SetActive(false);
//         right_on_screen_controlls_container.SetActive(false);
//         on_screen_controls_allowed = false;
//     }

//     public void StartCamShake(float duration, float magnitude)
//     {
//         if(_camShakeCoroutine != null){StopCoroutine(_camShakeCoroutine);}
//         _camShakeCoroutine = Shake_Camera(duration, magnitude);
//         StartCoroutine(_camShakeCoroutine);
//     }

//     public IEnumerator Shake_Camera(float duration, float magnitude)
//     {
//         Vector3 original_pos = transform.localPosition;

//         float elapsed = 0.0f;

//         while(elapsed < duration)
//         {
//             float x = Random.Range(-1f,1f) * magnitude;
//             float y = Random.Range(-1f,1f) * magnitude;

//             Camera.main.transform.position += new Vector3(x,y,original_pos.z);

//             elapsed += Time.deltaTime;

//             yield return null;
//         }
//     }
//     public void KeyPress(string press)
//     {
//         InputAction.CallbackContext nullCtx = new InputAction.CallbackContext();

//         switch (press)
//         {
//             case "W":
//                 Input_Control_Events.Invoke_Move_Up_Event(nullCtx);
//                 return;
//             case "D":
//                 Input_Control_Events.Invoke_Move_Right_Event(nullCtx);
//                 return;
//             case "S":
//                 Input_Control_Events.Invoke_Move_Down_Event(nullCtx);
//                 return;
//             case "A":
//                 Input_Control_Events.Invoke_Move_Left_Event(nullCtx);
//                 return;
//             default:
//                 return;
//         }
//     }

//     private void OnEnable()
//     {
//         player_input_actions.Enable();
//     }

//     private void OnDisable()
//     {
//         player_input_actions.Disable();
//     }

}
