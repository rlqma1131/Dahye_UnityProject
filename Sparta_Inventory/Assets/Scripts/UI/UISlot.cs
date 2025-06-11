using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISlot : MonoBehaviour
{
    [SerializeField] private GameObject equipMark;
    [SerializeField] private ItemData currentItem;

    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemQuantity;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(OnClickSlot);

    }

    void OnClickSlot()
    {
        UIManager.Instance.Inventory.SelectSlot(this);
    }

    public void SetItem(ItemData data, bool isEquipped)
    {
        currentItem = data;
        currentItem.isEquipped = isEquipped;
        SetSlot(data);
        SetEquippedMark(data.isEquipped);
    }

    public void SetSlot(ItemData data)
    {
        if (data == null)
        {
            itemIcon.enabled = false;
            itemQuantity.text = "";
        }
        else
        {
            itemIcon.sprite = data.icon;
            itemIcon.enabled = true;

            itemQuantity.text = data.quantity.ToString();
        }
    }

    public void SetEquippedMark(bool isEquipped)
    {
        equipMark.SetActive(isEquipped);
    }

    public void OnEquip()
    {
        if (currentItem == null)
            return;

        var Player = GameManager.Instance.Player;

        if (currentItem.isEquipped)
        {
            switch (currentItem.equipableData.equipableType)
            {
                case EquipableType.Weapon:
                    Player.SetAttackPower(Player.AttackPower - currentItem.equipableData.value);
                    break;

                case EquipableType.Armor:
                    Player.SetDeffencePower(Player.DeffencePower - currentItem.equipableData.value);
                    break;
            }

            currentItem.isEquipped = false;
            SetEquippedMark(false);
        }
        else
        {
            switch (currentItem.equipableData.equipableType)
            {
                case EquipableType.Weapon:
                    Player.SetAttackPower(Player.AttackPower + currentItem.equipableData.value);
                    break;

                case EquipableType.Armor:
                    Player.SetDeffencePower(Player.DeffencePower + currentItem.equipableData.value);
                    break;
            }

            currentItem.isEquipped = true;
            SetEquippedMark(true);
        }
            
        UIManager.Instance.RefreshUI();
    }

    public void OnUse()
    {
        if (currentItem == null) 
            return;

        var Player = GameManager.Instance.Player;

        switch (currentItem.consumableData.consumableType)
        {
            case ConsumableType.Health:
                Player.SetHealthPower(Player.CurHP +  currentItem.consumableData.value); 
                break;

            case ConsumableType.Deffence:
                Player.SetDeffencePower(Player.DeffencePower + currentItem.consumableData.value);
                break;
        }

        currentItem.quantity--;
        if (currentItem.quantity <= 0)
        {
            currentItem = null;
            SetSlot(null);
        }
        else
        {
            SetSlot(currentItem);
        }

        UIManager.Instance.RefreshUI();
    }

    public ItemData GetItemData()
    {
        return currentItem;
    }
}
