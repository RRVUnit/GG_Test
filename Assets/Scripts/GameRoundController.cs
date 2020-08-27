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

        private readonly Dictionary<PlayerType, PlayerController> _playerControllers = new Dictionary<PlayerType, PlayerController>();
        private readonly Dictionary<HealthBarViewMediator, GameObject> _healthBarToPlayerCharacters = new Dictionary<HealthBarViewMediator, GameObject>();

        private GameRoundUIController _gameRoundUIController;
        
        public GameRoundController(GameViewContext gameViewContext, Data dataConfig)
        {
            _gameViewContext = gameViewContext;
            _dataConfig = dataConfig;

            BindPlayerControls();
            CreatePlayerControllers();
            CreatePlayerHealthBars();
            CreateGameRoundUIController();
        }

        private void CreateGameRoundUIController()
        {
            _gameRoundUIController = new GameRoundUIController(_gameViewContext, _playerControllers);
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

        private void BindPlayerControls()
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
        
        private void StartRound(GameRoundModel roundModel)
        {
            _currentGameRoundModel = roundModel;

            ResetRound();
            ApplyPlayerModelsToController(roundModel);
            _gameRoundUIController.DrawPlayersPanels();
        }

        private void CreatePlayerHealthBars()
        {
            foreach (PlayerType playerType in _playerControllers.Keys) {
                GameObject playerObject = _gameViewContext.GetPlayerView(playerType).PanelHierarchy.character.gameObject;
                HealthBarViewMediator healthBar = GameObject.Instantiate(_gameViewContext.HealthPanel, _gameViewContext.HealthContainer.transform);
                GetPlayerController(playerType).HealthBar = healthBar;
                _healthBarToPlayerCharacters.Add(healthBar, playerObject);
            }
        }

        private void ApplyPlayerModelsToController(GameRoundModel roundModel)
        {
            foreach (PlayerType playerType in _playerControllers.Keys) {
                PlayerModel model = roundModel.GetPlayerByType(playerType);
                GetPlayerController(playerType).PlayerModel = model;
            }
        }

        private void ResetRound()
        {
            _gameRoundUIController.RemovePlayerStatPanels();
        }
        
        private bool IsGameOver()
        {
            return _playerControllers.Values.Any(p => p.PlayerModel.IsDead());
        }

        public void Tick()
        {
            UpdateHealthBarsPositions();

            foreach (PlayerController playerController in _playerControllers.Values) {
                playerController.Tick();
            }
        }

        private void UpdateHealthBarsPositions()
        {
            foreach (KeyValuePair<HealthBarViewMediator, GameObject> valuePair in _healthBarToPlayerCharacters) {
                UpdateHealthBarPosition(valuePair.Key, valuePair.Value);
            }
        }

        private void UpdateHealthBarPosition(HealthBarViewMediator healthBar, GameObject character)
        {
            Vector3 characterPosition = character.transform.position;
            characterPosition.y += 3.5f;

            RectTransform healthBarRect = healthBar.GetComponent<RectTransform>();
            Vector2 screenPosition = RectTransformUtility.WorldToScreenPoint(Camera.main, characterPosition);
            healthBarRect.anchoredPosition = screenPosition;
        }
    }
}