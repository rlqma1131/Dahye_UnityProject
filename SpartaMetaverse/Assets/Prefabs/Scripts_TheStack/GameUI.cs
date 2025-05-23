using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TheStack
{
    public class GameUI : BaseUI
    {
        TextMeshProUGUI scoreTxt;
        TextMeshProUGUI comboTxt;
        TextMeshProUGUI maxComboTxt;

        protected override UIState GetUIState()
        {
            return UIState.Game;
        }
        public override void Init(UIManager uiManager)
        {
            base.Init(uiManager);

            scoreTxt = transform.Find("ScoreTxt").GetComponent<TextMeshProUGUI>();
            comboTxt = transform.Find("ComboTxt").GetComponent<TextMeshProUGUI>();
            maxComboTxt = transform.Find("MaxComboTxt").GetComponent<TextMeshProUGUI>();
        }

        public void SetUI(int score, int combo, int maxCombo)
        {
            scoreTxt.text = score.ToString();
            comboTxt.text = combo.ToString();
            maxComboTxt.text = maxCombo.ToString();
        }
    }
}
