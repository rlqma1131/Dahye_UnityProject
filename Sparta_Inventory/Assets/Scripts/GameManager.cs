using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Character Player { get; private set; }

    void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        SetData();
    }

    void SetData()
    {
        Player = UIManager.Instance.Character;

        UIManager.Instance.Main.SetPlayer(Player);
        UIManager.Instance.Status.SetPlayer(Player);
    }
}
