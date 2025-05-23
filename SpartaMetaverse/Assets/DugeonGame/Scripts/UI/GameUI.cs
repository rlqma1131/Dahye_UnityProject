using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI waveTxt;
    [SerializeField] private Slider hpSlider;

    private void Start()
    {
        UpdateHPSlider(1); // ²Ë Ã¤¿öÁø °ª
    }

    public void UpdateHPSlider(float percentage)
    {
        hpSlider.value = percentage;
    }

    public void UpdateWaveText(int wave)
    {
        waveTxt.text = wave.ToString();
    }

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }
}
