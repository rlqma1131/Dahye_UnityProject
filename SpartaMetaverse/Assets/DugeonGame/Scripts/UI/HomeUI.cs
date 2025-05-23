using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeUI : BaseUI
{
    [SerializeField] private Button startBtn;
    [SerializeField] private Button exitBtn;

    public override void Init(UIManager uIManager)
    {
        base.Init(uIManager);

        startBtn.onClick.AddListener(OnClickStartBtn);
        exitBtn.onClick.AddListener(OnClickExitBtn);
    }

    public void OnClickStartBtn()
    {
        GameManager.Instance.StartGame();
    }

    public void OnClickExitBtn()
    {
        Application.Quit();
    }

    protected override UIState GetUIState()
    {
        return UIState.Home;
    }
}
