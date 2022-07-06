using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActor : StageActor
{
    private PiaMainControls PlayerInputActions;
    private InputAction move, placeturret;
    

    private void Awake()
    {
        PlayerInputActions = new PiaMainControls();
    }
    private void OnEnable()
    {
        move = PlayerInputActions.MainMap.PlayerMovement;
        placeturret = PlayerInputActions.MainMap.PlaceTurret;
        move.Enable();

        placeturret.performed += context => PlaceTurret(context);
    }
    private void OnDisable()
    {
        placeturret.performed -= context => PlaceTurret(context);
        move.Disable();
    }


    private void PlaceTurret(InputAction.CallbackContext context)
    {

    }

    private void FixedUpdate()
    {
        Vector2 v = move.ReadValue<Vector2>();
        Debug.Log(v);
    }


    public override void Setup()
    {
        base.Setup();
    }
    public override void Activate()
    {
        base.Activate();
    }
    public override void Die()
    {
        base.Die();
    }
}
