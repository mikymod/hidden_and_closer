using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace HNC
{
    [CreateAssetMenu(fileName = "InputHandler", menuName = "HNC/InputHandler")]
    public class InputHandler : ScriptableObject, GameInput.IPlayerActions, GameInput.ICompanionActions, GameInput.IUIActions
    {
        public event UnityAction<Vector2> move = delegate { };
        public event UnityAction<Vector2> look = delegate { };
        public event UnityAction aimStarted = delegate { };
        public event UnityAction aimCanceled = delegate { };
        public event UnityAction crouchStarted = delegate { };
        public event UnityAction crouchCanceled = delegate { };
        public event UnityAction fireStarted = delegate { };
        public event UnityAction fireCanceled = delegate { };

        public event UnityAction<Vector2> companionMove = delegate { };
        public event UnityAction<Vector2> companionLook = delegate { };
        public event UnityAction companionSwitch = delegate { };
        public event UnityAction playerSwitch = delegate { };

        public event UnityAction pause = delegate { };

        private GameInput gameInput;

        private void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new GameInput();
                gameInput.Player.SetCallbacks(this);
                gameInput.Companion.SetCallbacks(this);
                gameInput.UI.SetCallbacks(this);
            }
        }

        private void OnDisable() => DisableAllInput();

        public void DisableAllInput()
        {
            gameInput.Player.Disable();
            gameInput.Companion.Disable();
            gameInput.UI.Disable();
        }

        public void EnablePlayerInput()
        {
            gameInput.Player.Enable();
            gameInput.Companion.Disable();
            gameInput.UI.Disable();
        }

        public void EnableCompanionInput()
        {
            gameInput.Player.Disable();
            gameInput.Companion.Enable();
            gameInput.UI.Disable();
        }

        public void EnableUIInput()
        {
            gameInput.Player.Disable();
            gameInput.Companion.Disable();
            gameInput.UI.Enable();
        }

        public void OnAim(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                aimStarted?.Invoke();
            }

            if (context.canceled)
            {
                aimCanceled?.Invoke();
            }
        }

        public void OnCrouch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                crouchStarted?.Invoke();
            }

            if (context.canceled)
            {
                crouchCanceled?.Invoke();
            }
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                fireStarted?.Invoke();
            }

            if (context.canceled)
            {
                fireCanceled?.Invoke();
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            look?.Invoke(value);
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            move?.Invoke(value);
        }

        public void OnCompanionLook(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            companionLook?.Invoke(value);
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                pause?.Invoke();
            }
        }

        public void OnCompanionMove(InputAction.CallbackContext context)
        {
            Vector2 value = context.ReadValue<Vector2>();
            companionMove?.Invoke(value);
        }
        public void OnCompanionSwitch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                companionSwitch?.Invoke();
            }
        }
        public void OnPlayerSwitch(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                playerSwitch?.Invoke();
            }
        }
        public void OnNavigate(InputAction.CallbackContext context) { }

        public void OnSubmit(InputAction.CallbackContext context) { }

        public void OnCancel(InputAction.CallbackContext context) { }

        public void OnPoint(InputAction.CallbackContext context) { }

        public void OnClick(InputAction.CallbackContext context) { }

        public void OnScrollWheel(InputAction.CallbackContext context) { }

        public void OnMiddleClick(InputAction.CallbackContext context) { }

        public void OnRightClick(InputAction.CallbackContext context) { }

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) { }

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) { }
    }
}

