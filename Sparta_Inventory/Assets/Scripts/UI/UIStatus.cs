using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatus : UIAnimation
{
    [SerializeField] private Button ExitBtn;
    [SerializeField] private TextMeshProUGUI attackPower;
    [SerializeField] private TextMeshProUGUI deffencePower;
    [SerializeField] private TextMeshProUGUI hp;
    [SerializeField] private TextMeshProUGUI critical;

    void Start()
    {
        ExitBtn.onClick.AddListener(() => UIManager.Instance.OpenMainMenu());
    }

    public void SetPlayer(Character player)
    {
        attackPower.text = $"{player.AttackPower}";
        deffencePower.text = $"{player.DeffencePower}";
        hp.text = $"{player.CurHP}";
        critical.text = $"{player.Critical}";
    }
}
