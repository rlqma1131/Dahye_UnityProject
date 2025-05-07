using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TheStack
{
    public class ScoreUI : BaseUI
    {
        TextMeshProUGUI scoreTxt;
        TextMeshProUGUI comboTxt;
        TextMeshProUGUI bestComboTxt;
        TextMeshProUGUI bestScoreTxt;

        Button startBtn;
        Button exitBtn;

        protected override UIState GetUIState()
        {
            return UIState.Score;
        }
        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            scoreTxt = transform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();
            comboTxt = transform.Find("ComboTxt").GetComponent<TextMeshProUGUI>();
            bestComboTxt = transform.Find("BestComboTxt").GetComponent<TextMeshProUGUI>();
            bestScoreTxt = transform.Find("BestScoreTxt").GetComponent<TextMeshProUGUI>();

            startBtn = transform.Find("StartBtn").GetComponent<Button>();
            exitBtn = transform.Find("ExitBtn").GetComponent<Button>();

            startBtn.onClick.AddListener(OnClickStartButton);
            exitBtn.onClick.AddListener(OnClickExitButton);
        }

        public void SetUI(int score, int combo, int bestCombo, int bestScore)
        {
            scoreTxt.text = score.ToString();
            comboTxt.text = combo.ToString();
            bestComboTxt.text = bestCombo.ToString();
            bestScoreTxt.text = bestScore.ToString();
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
