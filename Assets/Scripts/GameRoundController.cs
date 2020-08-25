using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameRoundController
    {
        private readonly GameViewContext _gameViewContext;
        private readonly Data _dataConfig;

        private GameRoundModel _currentGameRoundModel;
        
        public GameRoundController(GameViewContext gameViewContext, Data dataConfig)
        {
            _gameViewContext = gameViewContext;
            _dataConfig = dataConfig;

            AddGameControls();
            AddPlayerControls();
        }

        private void AddGameControls()
        {
            List<GameTypeControl> gameTypeControls = _gameViewContext.GameTypeControls;
            gameTypeControls.ForEach(gc => gc.GameTypeButton.onClick.AddListener(() => { OnGameModeChanged(gc.GameType); }));
        }

        private void OnGameModeChanged(GameType gameType)
        {
            Debug.Log("GameType changed: " + gameType);
            CreateRound(gameType);
        }

        private void AddPlayerControls()
        {
            List<PlayerView> playerViews = _gameViewContext.PlayerViews;
            playerViews.ForEach(BindControls);
        }

        private void BindControls(PlayerView playerView)
        {
            playerView.PanelHierarchy.attackButton.onClick.AddListener(() => {
                OnAttackButtonClicked(playerView.PlayerType);
            });
        }

        private void OnAttackButtonClicked(PlayerType playerType)
        {
            Debug.Log("Attack clicked" + playerType);
        }

        public void StartRound(GameRoundModel roundModel)
        {
            _currentGameRoundModel = roundModel;

            ResetRound();

            DrawRoundSettings();
        }

        private void DrawRoundSettings()
        {
            
        }

        private void ResetRound()
        {
            
        }

        public void CreateRound(GameType gameType)
        {
            GameRoundModel gameRoundModel = GameRoundFactory.Create(gameType, _dataConfig);
            StartRound(gameRoundModel);
        }
    }
}