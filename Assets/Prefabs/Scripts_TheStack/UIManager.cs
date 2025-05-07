using UnityEngine;

namespace TheStack
{
    public enum UIState
    {
        Home,
        Game,
        Score,
    }

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        UIState currentState = UIState.Home;
        HomeUI homeUI = null;
        GameUI gameUI = null;
        ScoreUI scoreUI = null;
        TheStack theStack = null;

        private void Awake()
        {
            // ΩÃ±€≈Ê ¡ﬂ∫π πÊ¡ˆ
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            theStack = FindObjectOfType<TheStack>();
            homeUI = GetComponentInChildren<HomeUI>(true);
            homeUI?.Init(this);
            gameUI = GetComponentInChildren<GameUI>(true);
            gameUI?.Init(this);
            scoreUI = GetComponentInChildren<ScoreUI>(true);
            scoreUI?.Init(this);

            ChangeState(UIState.Home);
        }

        public void ChangeState(UIState state)
        {
            currentState = state;
            homeUI?.SetActive(currentState);
            gameUI?.SetActive(currentState);
            scoreUI?.SetActive(currentState);
        }

        public void OnClickStart()
        {
            theStack.Restart();
            ChangeState(UIState.Game);
        }

        public void OnClickExit()
        {
            ReturnToMainScene();

        }
        void ReturnToMainScene()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene",
        UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public void UpdateScore()
        {
            gameUI.SetUI(theStack.Score, theStack.Combo, theStack.MaxCombo);
        }

        public void SetScoreUI()
        {
            scoreUI.SetUI(theStack.Score, theStack.MaxCombo, theStack.BestCombo, theStack.BestScore);
            ChangeState(UIState.Score);
        }
    }
}
