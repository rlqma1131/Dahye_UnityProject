using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCondition : MonoBehaviour
{
    public UICondition uiCondition;
    public GameObject Dead;

    Condition health { get { return uiCondition.health; } }
    Condition hunger { get { return uiCondition.hunger; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public float noHungerHealthDecay;

    private Coroutine boostCoroutine;
    private bool isBoosting = false;

    void Update()
    {
        hunger.Subtract(hunger.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (hunger.curValue == 0f)
        {
            health.Subtract(noHungerHealthDecay * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void Eat(float amount)
    {
        hunger.Add(amount);
    }

    public void Boost(float multiplier)
    {
        if (isBoosting) return;

        if (boostCoroutine != null)
        {
            StopCoroutine(boostCoroutine);
            boostCoroutine = null;
        }

        isBoosting = true;
        boostCoroutine = StartCoroutine(BoostCoroutine(multiplier, 10f));
    }

    private IEnumerator BoostCoroutine(float multiplier, float duration)
    {
        PlayerController controller = GetComponent<PlayerController>();
        float originalJump = controller.jumpPower;

        controller.jumpPower *= multiplier;

        yield return new WaitForSeconds(duration);

        isBoosting = false;
        controller.jumpPower = originalJump;
    }

    public void Die()
    {
        Time.timeScale = 0f;
        CharacterManager.Instance.Player.controller.enabled = false;
        CharacterManager.Instance.Player.controller.canLook = false;
        Dead.SetActive(true);
    }

    public bool UseStamina(float amount)
    {
        if (stamina.curValue - amount < 0f)
        {
            return false;
        }
        
        stamina.Subtract(amount);
        return true;
    }
}
