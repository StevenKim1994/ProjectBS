using BS.GameObjects;
using BS.UI;
using UnityEngine;

namespace BS.System
{
    public enum GameStepState
    {
        Start,
        Playing,
        Paused,
        GameOver
    }

    public class GameSequenceSystem : ISystem
    {
        private static GameSequenceSystem _instance;
        public static GameSequenceSystem Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = SystemGameObject.Instance.GetSystem<GameSequenceSystem>();
                }
                return _instance;
            }
        }

        private GameStepState _currentState;
        public GameStepState CurrentState
        {
            get { return _currentState; }
        }

        public void Load()
        {

        }

        public void Unload()
        {

        }

        public void SetGameStepState(GameStepState state)
        {
            _currentState = state;
            switch(_currentState)
            {
                case GameStepState.Playing:
                    {
                        Time.timeScale = 1.0f;
                        InputControlSystem.Instance.IsInput = true;
                        if (UISystem.Instance.IsShowing<PauseUIPresenter>())
                        {
                            UISystem.Instance.Hide<PauseUIPresenter>();
                        }
                    }
                    break;

                case GameStepState.Paused:
                    {
                        InputControlSystem.Instance.IsInput = false;
                        UISystem.Instance.Show<PauseUIPresenter>();
                        Time.timeScale = 0.0f;
                    }
                    break;

                case GameStepState.GameOver:
                    {
                        InputControlSystem.Instance.IsInput = false;
                        UISystem.Instance.Hide<HUDUIPresenter>();
                        UISystem.Instance.Show<GameOverUIPresenter>();
                    }
                    break;
            }
        }
    }
}