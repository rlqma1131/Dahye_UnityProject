using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IInteractable
{
    public string GetInteractPrompt();  // 띄우는 정보
    public void OnInteract();   // 상호작용 시 실행할 내용
}
public class ItemObject : MonoBehaviour, IInteractable
{
    public ItemData itemData;

    public string GetInteractPrompt()
    {
        string str = $"줍기 [E]\n{itemData.displayName}\n{itemData.description}";
        return str;
    }

    public void OnInteract()
    {
        CharacterManager.Instance.Player.itemData = itemData;
        CharacterManager.Instance.Player.addItem?.Invoke();
        Destroy(gameObject); // 주우면 없어지게
    }
}
