using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameRoundUIController
    {
        private readonly GameViewContext _gameViewContext;
        private readonly Dictionary<PlayerType, PlayerController> _playerControllers;

        public GameRoundUIController(GameViewContext gameViewContext, Dictionary<PlayerType,PlayerController> playerControllers)
        {
            _gameViewContext = gameViewContext;
            _playerControllers = playerControllers;
        }

        public void DrawPlayersPanels()
        {
            foreach (PlayerController playerController in _playerControllers.Values) {
                PlayerType playerType = playerController.PlayerType;
                PlayerView playerView = _gameViewContext.GetPlayerView(playerType);
                Transform panelsContainer = playerView.PanelHierarchy.statsPanel;
                DrawBuffPanels(playerController.PlayerModel, panelsContainer);
                DrawStatPanels(playerController.PlayerModel, panelsContainer);
            }
        }

        public void RemovePlayerStatPanels()
        {
            foreach (PlayerView playerView in _gameViewContext.PlayerViews) {
                foreach (Transform child in playerView.PanelHierarchy.statsPanel) {
                    GameObject.Destroy(child.gameObject);
                }
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
    }
}