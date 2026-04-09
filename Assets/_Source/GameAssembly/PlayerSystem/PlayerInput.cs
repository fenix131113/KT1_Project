using System;
using GameAssembly.ReplaySystem;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.PlayerSystem
{
    public class PlayerInput : IInitializable, IFixedTickable
    {
        [Inject] private InputSystem_Actions _playerInput;
        [Inject] private ReplayController _replayController;

        public event Action OnHoldJump;

        /// <summary>
        /// Expose by itself
        /// </summary>
        public void RegisterJumpHoldInputCallback(Action callback) =>
            OnHoldJump += callback;

        public void UnregisterJumpHoldCallback(Action callback) =>
            OnHoldJump -= callback;

        public void FixedTick()
        {
            if (_replayController.IsPlayback)
                return;

            if (!_playerInput.Player.Jump.IsPressed())
                return;

            OnHoldJump?.Invoke();
            _replayController.RecordCommand(ReplayCommand.JumpHold(_replayController.CurrentTick, true));
        }

        public void Initialize()
        {
            _playerInput.Player.Enable();
            _replayController.OnPlaybackCommand += HandlePlaybackCommand;
        }

        private void HandlePlaybackCommand(ReplayCommand command)
        {
            if (command.type != ReplayCommandType.JUMP_HOLD || !command.boolValue)
                return;

            OnHoldJump?.Invoke();
        }
    }
}
