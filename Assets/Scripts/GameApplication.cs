using System;
using System.Collections.Generic;
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

            CreateGameControls();
            
            CreateGameRoundController();
            
            StartDefaultGame();

            CreateCameraRotationController();
        }

        private void CreateCameraRotationController()
        {
            GameCameraRotationController rotationController = gameObject.AddComponent<GameCameraRotationController>();
            rotationController.CameraAnchor = _gameViewContext.CameraAnchor;
            rotationController.CameraModel = _dataConfig.cameraSettings;
            rotationController.Init();
        }

        private void CreateGameControls()
        {
            List<GameTypeControl> gameTypeControls = _gameViewContext.GameTypeControls;
            gameTypeControls.ForEach(gc => gc.GameTypeButton.onClick.AddListener(() => { OnGameModeChanged(gc.GameType); }));
        }

        private void OnGameModeChanged(GameType gameType)
        {
            _gameRoundController.CreateRound(gameType);
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