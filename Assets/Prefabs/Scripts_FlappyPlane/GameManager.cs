using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Main;
using UnityEngine.SocialPlatforms.Impl;

namespace FlappyPlane
{
    public class GameManager : MonoBehaviour
    {
        static GameManager gameManager;
        public static GameManager Instance { get { return gameManager; } }

        private int curruentScore = 0;
        private int bestScore = 0;

        UIManager uiManager;
        public UIManager UIManager { get { return uiManager; } }
        public Player player;
        public AudioClip MiniGameBGM;

        private void Awake()
        {
            gameManager = this;
            uiManager = FindObjectOfType<UIManager>();
        }

        private void Start()
        {
            SoundManager.instance.ChangeBackGroundMusic(MiniGameBGM);
            int bestScore = PlayerPrefs.GetInt("BestScore", 0);

            uiManager.ShowHomeUI();

        }

        public void GameStart()
        {
            curruentScore = 0;
            uiManager.ShowGameUI();
            uiManager.UpdateScore(curruentScore);

            player.SetGame();
        }

        public void GameOver()
        {
            Debug.Log("Game Over");


            if (curruentScore > bestScore)
            {
                bestScore = curruentScore;
                PlayerPrefs.SetInt("BestScore", bestScore);
                PlayerPrefs.Save();
            }

            uiManager.SetRestart(curruentScore, bestScore);
        }


        public void AddScore(int score)
        {
            curruentScore += score;
            Debug.Log("Score: " + curruentScore);
            uiManager.UpdateScore(curruentScore);
        }
    }
}
