using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private UIMain main;
    [SerializeField] private UIStatus status;
    [SerializeField] private UIInventory inventory;
    [SerializeField] private Character character;

    public UIMain Main => main;
    public UIStatus Status => status;
    public UIInventory Inventory => inventory;
    public Character Character => character;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void OpenOnly(UIAnimation target)
    {
        if (main != null && main != target) main.Close();
        if (status != null && status != target) status.Close();
        if (inventory != null && inventory != target) inventory.Close();

        if (target == main)
        {
            character.MoveIn();
        }
        else
        {
            character.MoveOut();
        }

            target.Open();
    }

    public void OpenMainMenu() => OpenOnly(main);
    public void OpenStatus() => OpenOnly(status);
    public void OpenInventory() => OpenOnly(inventory);
}
