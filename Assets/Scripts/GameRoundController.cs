using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game
{
    public class GameRoundController
    {
        private readonly GameViewContext _gameViewContext;
        private readonly Data _dataConfig;

        private GameRoundModel _currentGameRoundModel;

        private Dictionary<PlayerType, PlayerController> _playerControllers = new Dictionary<PlayerType, PlayerController>();
        
        public GameRoundController(GameViewContext gameViewContext, Data dataConfig)
        {
            _gameViewContext = gameViewContext;
            _dataConfig = dataConfig;

            AddGameControls();
            AddPlayerControls();
            CreatePlayerControllers();
        }

        private void CreatePlayerControllers()
        {
            foreach (PlayerView playerView in _gameViewContext.PlayerViews) {
                PlayerController controller = CreatePlayerController(playerView);
                _playerControllers[controller.PlayerType] = controller;
            }
        }

        private PlayerController GetPlayerController(PlayerType playerType)
        {
            return _playerControllers[playerType];
        }
        
        private PlayerController CreatePlayerController(PlayerView playerView)
        {
            return new PlayerController(playerView.PlayerType, playerView.PanelHierarchy.character);
        }

        private void AddGameControls()
        {
            List<GameTypeControl> gameTypeControls = _gameViewContext.GameTypeControls;
            gameTypeControls.ForEach(gc => gc.GameTypeButton.onClick.AddListener(() => { OnGameModeChanged(gc.GameType); }));
        }

        private void OnGameModeChanged(GameType gameType)
        {
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
            if (!CanAttack()) {
                return;
            }
            
            PlayerController playerController = GetPlayerController(playerType);
            PlayerController enemyController = GetEnemyController(playerType);
            
            playerController.Attack();
            
            int hitAmount = CalculateHitAmount(playerController, enemyController);
            enemyController.Hit(hitAmount);
        }

        private bool CanAttack()
        {
            return GameStarted() && !IsGameOver();
        }

        private bool GameStarted()
        {
            return _currentGameRoundModel != null;
        }

        private int CalculateHitAmount(PlayerController playerController, PlayerController enemyController)
        {
            return 10;
        }

        private PlayerController GetEnemyController(PlayerType playerType)
        {
            return _playerControllers.Values.First(pc => pc.PlayerType != playerType);
        }

        public void CreateRound(GameType gameType)
        {
            GameRoundModel gameRoundModel = GameRoundFactory.Create(gameType, _dataConfig);
            StartRound(gameRoundModel);
        }
        
        public void StartRound(GameRoundModel roundModel)
        {
            _currentGameRoundModel = roundModel;

            ResetRound();
            ApplyPlayerModelsToController(roundModel);
            DrawPlayersPanels();
        }

        private void ApplyPlayerModelsToController(GameRoundModel roundModel)
        {
            foreach (PlayerType playerType in _playerControllers.Keys) {
                PlayerModel model = roundModel.GetPlayerByType(playerType);
                GetPlayerController(playerType).PlayerModel = model;
            }
        }

        private void DrawPlayersPanels()
        {
            foreach (PlayerController playerController in _playerControllers.Values) {
                PlayerType playerType = playerController.PlayerType;
                PlayerView playerView = _gameViewContext.GetPlayerView(playerType);
                Transform panelsContainer = playerView.PanelHierarchy.statsPanel;
                DrawBuffPanels(playerController.PlayerModel, panelsContainer);
                DrawStatPanels(playerController.PlayerModel, panelsContainer);
            }
        }

        private void DrawBuffPanels(PlayerModel playerModel, Transform panelsContainer)
        {
            foreach (Stat stat in playerModel.CollectStats()) {
                AddPanel(stat.icon, stat.value.ToString(), panelsContainer);
            }
        }

        private void DrawStatPanels(PlayerModel playerModel, Transform panelsContainer)
        {
            foreach (Buff buff in playerModel.CollectBuffs()) {
                AddPanel(buff.icon, buff.title, panelsContainer);
            }
        }

        private void AddPanel(string icon, string value, Transform container)
        {
            StatPanelViewMediator statPanel = GameObject.Instantiate(_gameViewContext.StatPanel, container);
            statPanel.IconName = icon;
            statPanel.Value = value;
        }

        private void ResetRound()
        {
            RemovePlayerStatPanels();
        }

        private void RemovePlayerStatPanels()
        {
            foreach (PlayerView playerView in _gameViewContext.PlayerViews) {
                foreach (Transform child in playerView.PanelHierarchy.statsPanel) {
                    GameObject.Destroy(child.gameObject);
                }
            }
        }
        
        public bool IsGameOver()
        {
            return _playerControllers.Values.Any(p => p.PlayerModel.IsDead());
        }
    }
}