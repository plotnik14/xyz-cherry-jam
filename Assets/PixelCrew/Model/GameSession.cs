﻿using System.Collections.Generic;
using System.Linq;
using PixelCrew.Components.LevelManagement;
using PixelCrew.Model.Data;
using PixelCrew.Model.Models;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private string _defaultCheckpoint;
        
        public PlayerData Data => _data;

        private PlayerData _save;

        public QuickInventoryModel QuickInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        private List<string> _checkpoints = new List<string>();

        private HashSet<string> permanentlyDestroyed = new HashSet<string>();
        private HashSet<string> markedToBeDestroyed = new HashSet<string>();

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
                StartSession(_defaultCheckpoint);
            }
        }

        private void StartSession(string defaultCheckpoint)
        {
            SetChecked(defaultCheckpoint);
            
            LoadHud();
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
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);
        }

        private void LoadHud()
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
