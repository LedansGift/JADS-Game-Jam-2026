using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, Controls.IPlayerActions
{
    private bool gameStarted = false;
    public static InputManager Instance { get; private set; }
    public static bool disableInputs = false;

    public Vector2 MovementValue { get; private set; }
    public Vector2 MousePosition { get; private set; }
    public Action OnAttackEvent;
    public Action OnAttackReleaseEvent;
    public Action OnRepairEvent;
    public Action OnBuildEvent;
    public Action OnCycleLeftEvent;
    public Action OnCycleRightEvent;
    public Action OnMenuEvent;

    private Controls controls;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        controls = new Controls();
        controls.Player.SetCallbacks(this);
        controls.Player.Enable();

        disableInputs = true;

        MousePosition = Vector2.zero;
    }

    private void OnEnable()
    {
        PauseManager.OnPauseGame += ToggleDisableInputs;
        //LevelManager.OnGameEndLight += GameEnd;
    }

    private void OnDisable()
    {
        PauseManager.OnPauseGame -= ToggleDisableInputs;
        //LevelManager.OnGameEndLight -= GameEnd;
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            MousePosition = Vector2.zero;
            return;
        }

        MousePosition = context.ReadValue<Vector2>();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            MovementValue = Vector2.zero;
            return;
        }

        MovementValue = context.ReadValue<Vector2>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
        else if (context.canceled)
        {
            OnAttackReleaseEvent?.Invoke();
        }
    }

    public void OnRepair(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnRepairEvent?.Invoke();
        }
    }

    public void OnBuild(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnBuildEvent?.Invoke();
        }
    }

    public void OnCycleLeft(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnCycleLeftEvent?.Invoke();
        }
    }

    public void OnCycleRight(InputAction.CallbackContext context)
    {
        if (disableInputs)
        {
            return;
        }

        if (context.performed)
        {
            OnCycleRightEvent?.Invoke();
        }
    }

    public void OnMenu(InputAction.CallbackContext context)
    {
        if (!gameStarted)
        {
            return;
        }

        if (context.performed)
        {
            OnMenuEvent?.Invoke();
        }
    }

    private void ToggleDisableInputs(object sender, bool toggle)
    {
        if (!gameStarted)
        {
            return;
        }

        disableInputs = toggle;
    }

    public void GameStart()
    {
        gameStarted = true;
        disableInputs = false;
    }

    public void GamePause()
    {
        gameStarted = false;
        disableInputs = true;
    }
}
