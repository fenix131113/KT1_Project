using System;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.PlayerSystem
{
    public class PlayerInput : IInitializable, ITickable
    {
        [Inject] private InputSystem_Actions _playerInput;

        public event Action OnHoldJump;

        /// <summary>
        /// Expose by itself
        /// </summary>
        public void RegisterJumpHoldInputCallback(Action callback) =>
            OnHoldJump += callback;

        public void UnregisterJumpHoldCallback(Action callback) =>
            OnHoldJump -= callback;

        ~PlayerInput() => Expose();

        public void Tick()
        {
            if (_playerInput.Player.Jump.IsPressed())
                OnHoldJump?.Invoke();
        }

        public void Initialize() => _playerInput.Player.Enable();

        private void Expose() => OnHoldJump = null;
    }
}