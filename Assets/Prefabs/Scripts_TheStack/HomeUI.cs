using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace TheStack
{
    public class HomeUI : BaseUI
    {
        Button startBtn;
        Button exitBtn;

        protected override UIState GetUIState()
        {
            return UIState.Home;
        }

        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            startBtn = transform.Find("StartBtn").GetComponent<Button>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();

            startBtn.onClick.AddListener(OnClickStartButton);
            exitBtn.onClick.AddListener(OnClickExitButton);
        }

        void OnClickStartButton()
        {
            UIManager.OnClickStart();
        }

        void OnClickExitButton()
        {
            UIManager.OnClickExit();
        }
    }
}
