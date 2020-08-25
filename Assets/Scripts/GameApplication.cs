using UnityEngine;

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
            CreateRound(GameType.WITH_BUFFS);
        }

        private void CreateRound(GameType gameType)
        {
            GameRoundModel gameRoundModel = GameRoundFactory.Create(gameType, _dataConfig);
            _gameRoundController.StartRound(gameRoundModel);
        }

        private void LoadDataConfig()
        {
            TextAsset config = Resources.Load<TextAsset>("data");
            _dataConfig = JsonUtility.FromJson<Data>(config.text);
            Debug.Log("Data config loaded");
        }
    }
}