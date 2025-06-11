using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private UIMain main;
    [SerializeField] private UIStatus status;
    [SerializeField] private UIInventory inventory;
    [SerializeField] private Character character;
    [SerializeField] private UISlot slot;

    public UIMain Main => main;
    public UIStatus Status => status;
    public UIInventory Inventory => inventory;
    public Character Character => character;
    public UISlot Slot => slot;

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
    public void RefreshUI()
    {
        character = GameManager.Instance.Player;

        if (character != null && Status != null && Main != null)
        {
            Status.SetPlayer(character);
            Main.SetPlayer(character);
        }
    }

    public void OpenMainMenu() => OpenOnly(main);
    public void OpenStatus() => OpenOnly(status);
    public void OpenInventory() => OpenOnly(inventory);
}
