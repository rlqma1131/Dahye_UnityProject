using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Equipment : MonoBehaviour
{
    public Equip curEquip;
    public Transform equipParent; // ��� �޾��� ī�޶� ��ġ

    private PlayerController controller;
    private PlayerCondition condition;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<PlayerController>();
        condition = GetComponent<PlayerCondition>();
    }

    public void EquipNew(ItemData data)
    {
        UnEquip(); // ���� ��� ����
        curEquip = Instantiate(data.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if(curEquip != null)
        {
            Destroy(curEquip.gameObject);
            curEquip = null;
        }
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && curEquip != null && controller.canLook)
        {
            curEquip.OnAttackInput();
        }
    }
}
