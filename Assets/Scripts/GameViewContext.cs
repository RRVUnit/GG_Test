using System;
using System.Collections.Generic;
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