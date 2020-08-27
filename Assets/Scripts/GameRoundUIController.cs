using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameRoundUIController
    {
        private const string STAT_PANEL_PREFIX = "stat_";
        private const string BUFF_PANEL_PREFIX = "buff_";
        
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

        public void UpdateHealthPanelValue(PlayerType playerType, string value)
        {
            PlayerView playerView = _gameViewContext.GetPlayerView(playerType);
            Transform transform = playerView.PanelHierarchy.statsPanel.Find(STAT_PANEL_PREFIX + StatsId.LIFE_ID);
            StatPanelViewMediator statPanel = transform.GetComponent<StatPanelViewMediator>();
            statPanel.Value = value;
        }
        
        private void DrawBuffPanels(PlayerModel playerModel, Transform panelsContainer)
        {
            foreach (Stat stat in playerModel.CollectStats()) {
                string panelName = STAT_PANEL_PREFIX + stat.id;
                AddPanel(panelName, stat.icon, stat.value.ToString(), panelsContainer);
            }
        }

        private void DrawStatPanels(PlayerModel playerModel, Transform panelsContainer)
        {
            foreach (Buff buff in playerModel.CollectBuffs()) {
                string panelName = BUFF_PANEL_PREFIX + buff.id;
                AddPanel(panelName, buff.icon, buff.title, panelsContainer);
            }
        }

        private void AddPanel(string panelName, string icon, string value, Transform container)
        {
            StatPanelViewMediator statPanel = GameObject.Instantiate(_gameViewContext.StatPanel, container);
            statPanel.name = panelName;
            statPanel.IconName = icon;
            statPanel.Value = value;
        }
    }
}