using PixelCrew.Model.Data;
using PixelCrew.Utils.Disposables;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace PixelCrew.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        
        public PlayerData Data => _data;

        private PlayerData _save;

        public QuickInventoryModel QuickInventory { get; private set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public void Awake()
        {
            LoadHud();

            if (SessionExists())
            {
                Destroy(gameObject);
            }
            else
            {
                Save();
                InitModels();
                DontDestroyOnLoad(this);
            }
        }

        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }

        public void Save()
        {
            _save = _data.Clone();
        }

        public void LoadLastSave()
        {
            _data = _save.Clone();
            
            _trash.Dispose();
            InitModels();
        }

        private bool SessionExists()
        {
            var sessions = FindObjectsOfType<GameSession>();

            foreach (var session in sessions)
            {
                if (session != this)
                    return true;
            }

            return false;
        }

        private void OnDestroy()
        {
            _trash.Dispose();
        }
    }
}
