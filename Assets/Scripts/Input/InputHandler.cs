using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace HNC {
    [CreateAssetMenu(fileName = "InputHandler", menuName = "HNC/InputHandler")]
    public class InputHandler : ScriptableObject, GameInput.IPlayerActions, GameInput.ICompanionActions, GameInput.IUIActions {
        public event UnityAction<Vector2> move = delegate { };
        public event UnityAction<Vector2> look = delegate { };
        public event UnityAction aimStarted = delegate { };
        public event UnityAction aimCanceled = delegate { };
        public event UnityAction crouchStarted = delegate { };
        public event UnityAction crouchCanceled = delegate { };
        public event UnityAction fireStarted = delegate { };
        public event UnityAction fireCanceled = delegate { };
        public event UnityAction interactStarted = delegate { };
        public event UnityAction interactCanceled = delegate { };

        public event UnityAction<Vector2> companionMove = delegate { };
        public event UnityAction<Vector2> companionLook = delegate { };
        public event UnityAction companionSwitch = delegate { };
        public event UnityAction playerSwitch = delegate { };

        public event UnityAction pause = delegate { };

        private GameInput gameInput;

        private bool useGamepad;
        private bool aimPressed = false;
        private bool sendEvent = false;

        private void OnEnable() {
            if (gameInput == null) {
                gameInput = new GameInput();
                gameInput.Player.SetCallbacks(this);
                gameInput.Companion.SetCallbacks(this);
                gameInput.UI.SetCallbacks(this);
            }
        }

        private void OnDisable() => DisableAllInput();

        public void DisableAllInput() {
            gameInput.Player.Disable();
            gameInput.Companion.Disable();
            gameInput.UI.Disable();
        }

        public void EnablePlayerInput() {
            if (gameInput.UI.enabled && sendEvent) {
                if (aimPressed) {
                    aimStarted?.Invoke();
                } else {
                    aimCanceled?.Invoke();
                }
                sendEvent = false;
            }
            gameInput.Player.Enable();
            gameInput.Companion.Disable();
            gameInput.UI.Disable();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void EnableCompanionInput() {
            gameInput.Player.Disable();
            gameInput.Companion.Enable();
            gameInput.UI.Disable();

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public void EnableUIInput() {
            gameInput.Player.Disable();
            gameInput.Companion.Disable();
            gameInput.UI.Enable();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public IEnumerator StartRumble(float time, float intensity) {
            if (useGamepad) {
                Gamepad.current.SetMotorSpeeds(intensity, intensity);
                yield return new WaitForSeconds(time);
                Gamepad.current.SetMotorSpeeds(0, 0);
            }
        }

        private void CameFromGamepade(InputAction.CallbackContext context) => useGamepad = Gamepad.current != null && Gamepad.current.allControls.Contains(context.control);

        public void OnAim(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.performed) {
                if (gameInput.Player.enabled) {
                    if (!aimPressed) {
                        aimPressed = true;
                        aimStarted?.Invoke();
                    } else {
                        aimPressed = false;
                        aimCanceled?.Invoke();
                    }
                } else if (gameInput.UI.enabled) {
                    if (!aimPressed) {
                        aimPressed = true;
                    } else {
                        aimPressed = false;
                    }
                    sendEvent = true;
                }
            }
        }

        public void OnCrouch(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.started) {
                crouchStarted?.Invoke();
            }

            if (context.canceled) {
                crouchCanceled?.Invoke();
            }
        }

        public void OnFire(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.started) {
                fireStarted?.Invoke();
            }

            if (context.canceled) {
                fireCanceled?.Invoke();
            }
        }

        public void OnInteract(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.started) {
                interactStarted?.Invoke();
            }

            if (context.canceled) {
                interactCanceled?.Invoke();
            }
        }

        public void OnLook(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            Vector2 value = context.ReadValue<Vector2>();
            look?.Invoke(value);
        }

        public void OnMoveX(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            Vector2 value = context.ReadValue<Vector2>();
            move?.Invoke(value);
        }

        public void OnCompanionLook(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            Vector2 value = context.ReadValue<Vector2>();
            companionLook?.Invoke(value);
        }

        public void OnPause(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.started) {
                pause?.Invoke();
            }
        }

        public void OnCompanionMove(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            Vector2 value = context.ReadValue<Vector2>();
            companionMove?.Invoke(value);
        }
        public void OnCompanionSwitch(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.started) {
                companionSwitch?.Invoke();
            }
        }
        public void OnPlayerSwitch(InputAction.CallbackContext context) {
            CameFromGamepade(context);

            if (context.started) {
                playerSwitch?.Invoke();
            }
        }

        public void OnNavigate(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnSubmit(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnCancel(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnPoint(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnClick(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnScrollWheel(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnMiddleClick(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnRightClick(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnTrackedDevicePosition(InputAction.CallbackContext context) => CameFromGamepade(context);

        public void OnTrackedDeviceOrientation(InputAction.CallbackContext context) => CameFromGamepade(context);
    }
}

