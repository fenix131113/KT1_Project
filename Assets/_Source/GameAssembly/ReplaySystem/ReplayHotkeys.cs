using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.ReplaySystem
{
    public class ReplayHotkeys : IInitializable, ITickable
    {
        [Inject] private ReplayController _replayController;

        public void Initialize() =>
            Debug.Log("[Replay] Hotkeys: F5=record_start, F6=record_stop, F7=replay_play_last, F8=save_last, F9=load_latest_and_play.");

        public void Tick()
        {
            var keyboard = Keyboard.current;
            if (keyboard == null)
                return;

            if (keyboard.f5Key.wasPressedThisFrame)
                _replayController.StartRecording();

            if (keyboard.f6Key.wasPressedThisFrame)
                _replayController.StopRecording();

            if (keyboard.f7Key.wasPressedThisFrame)
                _replayController.PlayLastReplay();

            if (keyboard.f8Key.wasPressedThisFrame)
                _replayController.SaveLastReplayToFile();

            if (keyboard.f9Key.wasPressedThisFrame)
                _replayController.LoadAndPlayLatestReplayFromFile();
        }
    }
}
