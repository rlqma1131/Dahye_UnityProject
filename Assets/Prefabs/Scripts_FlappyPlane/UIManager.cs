using System.Collections;
using System.Collections.Generic;
using Main;
using TheStack;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FlappyPlane
{
    public class UIManager : MonoBehaviour
    {
        public Player player;

        [Header("UI Panels")]
        public GameObject homeUI;
        public GameObject gameUI;
        public GameObject scoreUI;

        [Header("Score Texts")]
        public TextMeshProUGUI scoreTxt;         
        public TextMeshProUGUI bestScoreTxt;     

        void Start()
        {
        }

        public void ShowHomeUI()
        {
            homeUI.SetActive(true);
            gameUI.SetActive(false);
            scoreUI.SetActive(false);
        }

        public void ShowGameUI()
        {
            homeUI.SetActive(false);
            gameUI.SetActive(true);
            UpdateScore(0);
        }

        public void SetRestart(int currentScore, int bestScore)
        {
            scoreUI.SetActive(true);
            gameUI.SetActive(false);

            bestScoreTxt.text = bestScore.ToString();
            scoreTxt.text = currentScore.ToString();
        }

        public void UpdateScore(int score)
        {
            scoreTxt.text = score.ToString();
        }
        public void OnClickRestart()
        {
            ShowHomeUI();
            player.ResetGame();
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
    }
}
