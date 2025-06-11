using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIAnimation
{
    [SerializeField] private Button statusBtn;
    [SerializeField] private Button inventoryBtn;
    [SerializeField] private Image HP;
    [SerializeField] private TextMeshProUGUI HPAmount;

    void Start()
    {
        statusBtn.onClick.AddListener(() => UIManager.Instance.OpenStatus());
        inventoryBtn.onClick.AddListener(() => UIManager.Instance.OpenInventory());
    }

    public void SetPlayer(Character player)
    {
        HP.fillAmount = player.CurHP / player.MaxHP;
        HPAmount.text = $"{player.CurHP} / {player.MaxHP}";
    }
}
