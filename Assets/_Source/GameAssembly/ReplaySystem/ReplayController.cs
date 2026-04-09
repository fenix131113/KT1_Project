using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace GameAssembly.ReplaySystem
{
    public class ReplayController : IInitializable, IFixedTickable
    {
        private const int CURRENT_REPLAY_VERSION = 1;
        private const string REPLAY_FILE_EXTENSION = ".replay.json";

        [Inject] private ReplayClock _clock;
        [Inject] private IRng _rng;

        private ReplayMode _mode = ReplayMode.IDLE;
        private ReplayData _recordingReplay;
        private ReplayData _playbackReplay;
        private int _playbackCommandIndex;

        public int CurrentTick => _clock.CurrentTick;
        public bool IsRecording => _mode == ReplayMode.RECORDING;
        public bool IsPlayback => _mode == ReplayMode.PLAYBACK;
        public ReplayMode Mode => _mode;

        public event Action<ReplayCommand> OnPlaybackCommand;
        public event Action<ReplayMode> OnModeChanged;

        public void Initialize()
        {
            if (ReplayRuntimeStore.PendingPlaybackReplay == null)
                return;

            var replay = ReplayRuntimeStore.PendingPlaybackReplay;
            ReplayRuntimeStore.PendingPlaybackReplay = null;
            StartPlaybackInternal(replay);
        }

        public void FixedTick()
        {
            if (!IsPlayback || _playbackReplay == null)
                return;

            var commands = _playbackReplay.commands;
            var currentTick = _clock.CurrentTick;

            while (_playbackCommandIndex < commands.Count && commands[_playbackCommandIndex].tick <= currentTick)
            {
                var command = commands[_playbackCommandIndex];
                _playbackCommandIndex++;

                if (command.tick != currentTick)
                    continue;

                OnPlaybackCommand?.Invoke(command);
            }

            if (_playbackCommandIndex < commands.Count || currentTick < _playbackReplay.durationTicks)
                return;

            SetMode(ReplayMode.IDLE);
            Debug.Log($"[Replay] Playback complete. durationTicks={_playbackReplay.durationTicks}.");
        }

        public void StartRecording()
        {
            if (IsRecording)
            {
                Debug.LogWarning("[Replay] record_start ignored. Recording is already active.");
                return;
            }

            if (IsPlayback)
            {
                Debug.LogWarning("[Replay] Cannot start recording while playback is active.");
                return;
            }

            var seed = Environment.TickCount;
            _rng.Reset(seed);
            _clock.Reset();
            _recordingReplay = new ReplayData
            {
                header = new ReplayHeader
                {
                    version = CURRENT_REPLAY_VERSION,
                    tickRate = ReplayClock.TICK_RATE,
                    seed = seed,
                    platform = Application.platform.ToString()
                }
            };

            SetMode(ReplayMode.RECORDING);
            Debug.Log($"[Replay] record_start. seed={seed}, tickRate={ReplayClock.TICK_RATE}.");
        }

        public void StopRecording()
        {
            if (!IsRecording || _recordingReplay == null)
            {
                Debug.LogWarning("[Replay] record_stop ignored. Recording is not active.");
                return;
            }

            _recordingReplay.durationTicks = _clock.CurrentTick;
            ReplayRuntimeStore.LastReplay = CloneReplay(_recordingReplay);

            var commandsCount = _recordingReplay.commands.Count;
            var duration = _recordingReplay.durationTicks;
            var seed = _recordingReplay.header.seed;

            _recordingReplay = null;
            SetMode(ReplayMode.IDLE);
            Debug.Log($"[Replay] record_stop. commands={commandsCount}, durationTicks={duration}, seed={seed}.");
        }

        public void PlayLastReplay()
        {
            if (ReplayRuntimeStore.LastReplay == null)
            {
                Debug.LogWarning("[Replay] replay_play_last ignored. Last replay is empty.");
                return;
            }

            ReplayRuntimeStore.PendingPlaybackReplay = CloneReplay(ReplayRuntimeStore.LastReplay);
            Debug.Log("[Replay] replay_play_last. Reloading scene...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public string SaveLastReplayToFile(string fileName = null)
        {
            if (ReplayRuntimeStore.LastReplay == null)
            {
                Debug.LogWarning("[Replay] save_last ignored. Last replay is empty.");
                return null;
            }

            var replayDirectory = GetReplayDirectoryAbsolutePath();
            Directory.CreateDirectory(replayDirectory);

            var normalizedName = NormalizeReplayFileName(fileName);
            var absolutePath = Path.Combine(replayDirectory, normalizedName);

            var replayCopy = CloneReplay(ReplayRuntimeStore.LastReplay);
            var json = JsonUtility.ToJson(replayCopy, true);
            File.WriteAllText(absolutePath, json);

            Debug.Log($"[Replay] save_last -> {ToRelativeProjectPath(absolutePath)}");
            return absolutePath;
        }

        public bool LoadReplayFromFile(string absolutePath)
        {
            if (string.IsNullOrWhiteSpace(absolutePath))
            {
                Debug.LogWarning("[Replay] load_replay ignored. Path is empty.");
                return false;
            }

            if (!File.Exists(absolutePath))
            {
                Debug.LogError($"[Replay] load_replay failed. File not found: {absolutePath}");
                return false;
            }

            try
            {
                var json = File.ReadAllText(absolutePath);
                var replay = JsonUtility.FromJson<ReplayData>(json);
                replay ??= new ReplayData();
                replay.header ??= new ReplayHeader();
                replay.commands ??= new List<ReplayCommand>();

                if (!ValidateReplay(replay))
                    return false;

                ReplayRuntimeStore.LastReplay = CloneReplay(replay);
                Debug.Log($"[Replay] load_replay <- {ToRelativeProjectPath(absolutePath)}");
                return true;
            }
            catch (Exception exception)
            {
                Debug.LogError($"[Replay] load_replay failed. {exception.Message}");
                return false;
            }
        }

        public bool LoadAndPlayLatestReplayFromFile()
        {
            var absolutePath = GetLatestReplayFilePath();
            if (absolutePath == null)
            {
                Debug.LogWarning("[Replay] load_latest ignored. No replay files found.");
                return false;
            }

            if (!LoadReplayFromFile(absolutePath))
                return false;

            PlayLastReplay();
            return true;
        }

        public bool LoadAndPlayReplayFromFile(string absolutePath)
        {
            if (!LoadReplayFromFile(absolutePath))
                return false;

            PlayLastReplay();
            return true;
        }

        public void RecordCommand(ReplayCommand command)
        {
            if (!IsRecording || _recordingReplay == null)
                return;

            _recordingReplay.commands.Add(command);
        }

        private void StartPlaybackInternal(ReplayData replay)
        {
            if (!ValidateReplay(replay))
                return;

            _playbackReplay = CloneReplay(replay);
            _playbackReplay.commands.Sort((left, right) => left.tick.CompareTo(right.tick));

            _playbackCommandIndex = 0;
            _clock.Reset();
            _rng.Reset(_playbackReplay.header.seed);

            SetMode(ReplayMode.PLAYBACK);
            Debug.Log(
                $"[Replay] Playback started. commands={_playbackReplay.commands.Count}, seed={_playbackReplay.header.seed}.");
        }

        private static ReplayData CloneReplay(ReplayData source)
        {
            if (source == null)
                return null;

            var cloned = new ReplayData
            {
                durationTicks = source.durationTicks,
                header = new ReplayHeader
                {
                    version = source.header.version,
                    tickRate = source.header.tickRate,
                    seed = source.header.seed,
                    platform = source.header.platform
                },
                commands = new List<ReplayCommand>(source.commands.Count)
            };

            for (var i = 0; i < source.commands.Count; i++)
                cloned.commands.Add(source.commands[i]);

            return cloned;
        }

        private static string NormalizeReplayFileName(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
                fileName = $"replay_{DateTime.Now:yyyyMMdd_HHmmss}";

            foreach (var invalidCharacter in Path.GetInvalidFileNameChars())
                fileName = fileName.Replace(invalidCharacter.ToString(), string.Empty);

            if (!fileName.EndsWith(REPLAY_FILE_EXTENSION, StringComparison.OrdinalIgnoreCase))
                fileName += REPLAY_FILE_EXTENSION;

            return fileName;
        }

        private static string GetReplayDirectoryAbsolutePath() =>
            Path.Combine(Application.dataPath, "_Presentation", "Replays");

        private static string GetLatestReplayFilePath()
        {
            var replayDirectory = GetReplayDirectoryAbsolutePath();
            if (!Directory.Exists(replayDirectory))
                return null;

            var files = Directory.GetFiles(replayDirectory, $"*{REPLAY_FILE_EXTENSION}");
            if (files.Length == 0)
                return null;

            Array.Sort(files, (left, right) =>
                File.GetLastWriteTimeUtc(right).CompareTo(File.GetLastWriteTimeUtc(left)));

            return files[0];
        }

        private static string ToRelativeProjectPath(string absolutePath)
        {
            var projectRootPath = Path.GetDirectoryName(Application.dataPath);
            if (string.IsNullOrWhiteSpace(projectRootPath))
                return absolutePath;

            var rootWithSeparator = projectRootPath.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                                    + Path.DirectorySeparatorChar;

            if (!absolutePath.StartsWith(rootWithSeparator, StringComparison.OrdinalIgnoreCase))
                return absolutePath;

            var relativePath = absolutePath[rootWithSeparator.Length..];
            return relativePath.Replace('\\', '/');
        }

        private static bool ValidateReplay(ReplayData replay)
        {
            if (replay?.header == null)
            {
                Debug.LogError("[Replay] Invalid replay: missing header.");
                return false;
            }

            if (replay.header.version != CURRENT_REPLAY_VERSION)
            {
                Debug.LogError(
                    $"[Replay] Unsupported version: {replay.header.version}. Expected {CURRENT_REPLAY_VERSION}.");
                return false;
            }

            if (!string.Equals(replay.header.platform, Application.platform.ToString(), StringComparison.Ordinal))
            {
                Debug.LogError(
                    $"[Replay] Platform mismatch. replay={replay.header.platform}, runtime={Application.platform}.");
                return false;
            }

            if (replay.header.tickRate != ReplayClock.TICK_RATE)
                Debug.LogWarning(
                    $"[Replay] TickRate mismatch. replay={replay.header.tickRate}, runtime={ReplayClock.TICK_RATE}.");

            return true;
        }

        private void SetMode(ReplayMode nextMode)
        {
            if (_mode == nextMode)
                return;

            _mode = nextMode;
            OnModeChanged?.Invoke(nextMode);
        }
    }
}
