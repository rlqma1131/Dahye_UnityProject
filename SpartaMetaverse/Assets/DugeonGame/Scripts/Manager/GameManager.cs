using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerController player { get; private set; }
    private ResourceController _playerResourceController;

    [SerializeField] private int currentWaveIndex = 0;

    private EnemyManager enemyManager;
    
    private UIManager uiManager;
    public static bool isFirstLoading = true;

    private void Awake()
    {
        Instance = this;

        player = FindObjectOfType<PlayerController>();
        player.Init(this);

        uiManager = FindObjectOfType<UIManager>();

        enemyManager = GetComponentInChildren<EnemyManager>();
        enemyManager.Init(this);

        _playerResourceController = player.GetComponent<ResourceController>();
        _playerResourceController.RemoveHealthChangeEvent(uiManager.ChangePlayerHP);
        _playerResourceController.AddHealthChangeEvent(uiManager.ChangePlayerHP);
    }


    private void Start()
    {
        if (!isFirstLoading)
        {
            StartGame(); // Restart를 했을 때 시작화면이 나오지 않게하기 위해
        }
        else
        {
            isFirstLoading = false; // 게임이 처음 실행됐을 때
        }
    }
    public void StartGame()
    {
        uiManager.SetPlayGame();
        StartNextWave();
    }

    void StartNextWave()
    {
        currentWaveIndex++;
        enemyManager.StartWave(1 + currentWaveIndex / 5);
        uiManager.ChangeWave(currentWaveIndex);
    }

    public void EndOfWave()
    {
        StartNextWave();
    }

    public void GameOver()
    {
        enemyManager.StopWave();
        uiManager.SetGameOver();
    }
}
