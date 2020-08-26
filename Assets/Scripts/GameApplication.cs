using System;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

namespace Game
{
    public class GameApplication : MonoBehaviour
    {
        private Data _dataConfig;

        private GameRoundController _gameRoundController;

        private GameViewContext _gameViewContext;
        
        private void Awake()
        {
            LoadDataConfig();

            CollectGameContext();

            CreateGameRoundController();
            
            StartDefaultGame();
        }

        private void CollectGameContext()
        {
            _gameViewContext = GetComponent<GameViewContext>();
        }

        private void CreateGameRoundController()
        {
            _gameRoundController = new GameRoundController(_gameViewContext, _dataConfig);
        }
        
        private void StartDefaultGame()
        {
            _gameRoundController.CreateRound(GameType.WITH_BUFFS);
        }

        private void Update()
        {
            _gameRoundController.Tick();
        }

        private void LoadDataConfig()
        {
            TextAsset config = Resources.Load<TextAsset>("data");
            _dataConfig = JsonUtility.FromJson<Data>(config.text);
            Debug.Log("Data config loaded");
        }
    }
}