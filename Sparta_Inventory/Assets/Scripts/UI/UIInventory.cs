using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : UIAnimation
{
    [SerializeField] private GameObject slot;
    [SerializeField] private Transform slotTransform;
    [SerializeField] private Button exitBtn;
    [SerializeField] private Button equipBtn;
    [SerializeField] private Button unequipBtn;
    [SerializeField] private Button useBtn;
    [SerializeField] private TextMeshProUGUI selectedItemName;
    [SerializeField] private TextMeshProUGUI selectedItemDescription;

    private List<UISlot> slots = new List<UISlot>();

    private UISlot selectedSlot;

    void Start()
    {
        InitInventory();
        exitBtn.onClick.AddListener(() => UIManager.Instance.OpenMainMenu());
        equipBtn.onClick.AddListener(() => 
        {
            if (selectedSlot != null)
                selectedSlot.OnEquip();
        });
        unequipBtn.onClick.AddListener(() =>
        {
            if (selectedSlot != null)
                selectedSlot.OnEquip();
        });
        useBtn.onClick.AddListener(() =>
        {
            if (selectedSlot != null)
                selectedSlot.OnUse();
        });

        // 임의로 인벤토리에 아이템 넣기
        string[] itemNames = { "Helmet", "Shield", "Sword", "Health Potion", "Defense Potion" };

        for (int i = 0; i < itemNames.Length && i < slots.Count; i++)
        {
            ItemData item = Instantiate(Resources.Load<ItemData>(itemNames[i]));

            if (item != null)
            {
                slots[i].SetItem(item, item.isEquipped);
            }
            else
            {
                Debug.LogWarning($"{itemNames[i]} 을 찾지 못했어요");
            }
        }

        selectedItemName.enabled = false;
        selectedItemDescription.enabled = true;
        equipBtn.gameObject.SetActive(false);
        useBtn.gameObject.SetActive(false);
    }

    void InitInventory()
    {
        for (int i = 0; i < 20; i++)
        {
            GameObject slotObj = Instantiate(this.slot, slotTransform);
            UISlot slot = slotObj.GetComponent<UISlot>();
            slots.Add(slot);
        }
    }

    public void SelectSlot(UISlot slot) // 버튼/설명 셋팅
    {
        selectedSlot = slot;

        var item = selectedSlot.GetItemData();
        if (item != null)
        {
            selectedItemName.text = item.displayName;
            selectedItemDescription.text = item.description;

            switch (item.itemType)
            {
                case ItemType.Equipable:
                    if (item.isEquipped == false) // 장착/해제 전환
                    {
                        equipBtn.gameObject.SetActive(true);
                        unequipBtn.gameObject.SetActive(false);
                        useBtn.gameObject.SetActive(false);
                    }
                    else
                    {
                        equipBtn.gameObject.SetActive(false);
                        unequipBtn.gameObject.SetActive(true);
                        useBtn.gameObject.SetActive(false);
                    }
                    break;
                case ItemType.Consumable:
                    equipBtn.gameObject.SetActive(false);
                    unequipBtn.gameObject.SetActive(false);
                    useBtn.gameObject.SetActive(true);
                    break;
                default:
                    equipBtn.gameObject.SetActive(false);
                    unequipBtn.gameObject.SetActive(false);
                    useBtn.gameObject.SetActive(false);
                    break;
            }
        }
        else // 빈칸 선택
        {
            selectedItemName.text = "";
            selectedItemDescription.text = "";
            equipBtn.gameObject.SetActive(false);
            useBtn.gameObject.SetActive(false);
        }
    }

}
