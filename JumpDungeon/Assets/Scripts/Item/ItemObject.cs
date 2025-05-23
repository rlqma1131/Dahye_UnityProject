using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IInteractable
{
    public string GetInteractPrompt();  // ���� ����
    public void OnInteract();   // ��ȣ�ۿ� �� ������ ����
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public string GetInteractPrompt()
    {
        string str = $"�ݱ� [E]\n{itemData.displayName}\n{itemData.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = itemData;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject); // �ֿ�� ��������
    }
}
