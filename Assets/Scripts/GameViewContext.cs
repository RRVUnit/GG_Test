using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class GameViewContext : MonoBehaviour
    {
        [SerializeField]
        public List<PlayerView> PlayerViews;
        
        [SerializeField]
        public List<GameTypeControl> GameTypeControls;

        [SerializeField]
        public StatPanelViewMediator StatPanel;
        
        [SerializeField]
        public HealthBarViewMediator HealthPanel;
        
        [SerializeField]
        public GameObject HealthContainer;
        
        public PlayerView GetPlayerView(PlayerType playerType)
        {
            return PlayerViews.First(pw => pw.PlayerType == playerType);
        }
    }

    [Serializable]
    public struct PlayerView
    {
        [SerializeField]
        public PlayerType PlayerType;
        
        [SerializeField]
        public PlayerPanelHierarchy PanelHierarchy;
    }
    
    [Serializable]
    public struct GameTypeControl
    {
        [SerializeField]
        public GameType GameType;
        
        [SerializeField]
        public Button GameTypeButton;
    }
}