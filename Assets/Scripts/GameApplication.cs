using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameApplication : MonoBehaviour
    {
        private Data _dataConfig;

        private void Awake()
        {
            LoadDataConfig();

            StartDefaultGame();
        }

        private void StartDefaultGame()
        {
            
        }

        private void LoadDataConfig()
        {
            TextAsset config = Resources.Load<TextAsset>("data");
            _dataConfig = JsonUtility.FromJson<Data>(config.text);
            Debug.Log("Data config loaded");
        }
    }
}