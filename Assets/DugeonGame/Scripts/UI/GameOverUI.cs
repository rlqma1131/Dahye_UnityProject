using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : BaseUI
{
    [SerializeField] private Button restartBtn;
    [SerializeField] private Button exitBtn;

    public override void Init(UIManager uIManager)
    {
        base.Init(uIManager);

        restartBtn.onClick.AddListener(onClickRestartButton);
        exitBtn.onClick.AddListener(OnClickExitButton);
    }

    public void onClickRestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnClickExitButton()
    {
        ReturnToMainScene();
    }

    void ReturnToMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene",
    UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }
}
