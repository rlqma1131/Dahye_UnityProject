using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventory : UIAnimation
{
    [SerializeField] private Button cancleBtn;

    void Start()
    {
        cancleBtn.onClick.AddListener(() => UIManager.Instance.OpenMainMenu());
    }
}
