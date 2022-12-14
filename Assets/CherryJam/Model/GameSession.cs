using System.Collections.Generic;
using System.Linq;
using CherryJam.Components.LevelManagement;
using CherryJam.Components.LevelManagement.SpawnPoints;
using CherryJam.Model.Data;
using CherryJam.Model.Definition;
using CherryJam.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CherryJam.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckpoint;
        
        public static GameSession Instance { get; private set; }
        
        public PlayerData Data => _data;

        private PlayerData _save;

        public SpawnPointType NextSpawnPointType { get; set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private List<string> _checkpoints = new List<string>();

        private HashSet<string> permanentlyDestroyed = new HashSet<string>();
        private HashSet<string> markedToBeDestroyed = new HashSet<string>();

        private readonly HashSet<string> _activatedTriggers = new HashSet<string>();

        public void ActivateTrigger(string triggerName)
        {
            _activatedTriggers.Add(triggerName);
        }

        public bool IsTriggerActivated(string triggerName)
        {
            return _activatedTriggers.Contains(triggerName);
        }

        public void Awake()
        {
            var existingSession = GetExistingSession();
            if (existingSession != null)
            {
                existingSession.StartSession(_defaultCheckpoint);
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(_defaultCheckpoint);
            }
        }

        private void StartSession(string defaultCheckpoint)
        {
            SetChecked(defaultCheckpoint);
            
            LoadUIs();
            SpawnHero();
        }

        private void SpawnHero()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            var lastCheckpointId = _checkpoints.Last();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Id == lastCheckpointId)
                {
                    checkpoint.SpawnHero();
                    break;
                }
            }
        }

        private void InitModels()
        {
            _data.Hp.Value = DefsFacade.I.Player.MaxHP;
        }

        private void LoadUIs()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        public void Save()
        {
            _save = _data.Clone();
            permanentlyDestroyed.UnionWith(markedToBeDestroyed);
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            markedToBeDestroyed.Clear();
            
            _trash.Dispose();
            InitModels();
        }

        private GameSession GetExistingSession()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach (var session in sessions)
            {
                if (session != this)
                    return session;
            }

            return null;
        }

        private void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            
            _trash.Dispose();
        }

        public bool IsChecked(string checkpointId)
        {
            return _checkpoints.Contains(checkpointId);
        }

        public void SetChecked(string checkpointId)
        {
            if (!IsChecked(checkpointId))
            {
                Save();
                _checkpoints.Add(checkpointId);
            }
        }

        public bool ObjectHasBeenDestroyed(string id)
        {
            return permanentlyDestroyed.Contains(id);
        }

        public void MarkObjectAsDestroyed(string id)
        {
            markedToBeDestroyed.Add(id);
        }
    }
}