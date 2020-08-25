using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameViewContext : MonoBehaviour
    {
        [SerializeField]
        public List<PlayerView> PlayerViews;
    }

    [Serializable]
    public struct PlayerView
    {
        [SerializeField]
        public PlayerType playerType;
        
        [SerializeField]
        public PlayerPanelHierarchy panelHierarchy;
    }
}