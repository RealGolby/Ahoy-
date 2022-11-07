using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int Health;
    public int Stamina;

    public int MaxHealth;
    public int MaxStamina;

    [SerializeField] TMP_Text HealthText;
    [SerializeField] TMP_Text StaminaText;

    private void Start()
    {
        HealthText.text = Health.ToString();
        StaminaText.text = Stamina.ToString();
    }

    public void ChangeHealth(int amount)
    {
        Health += amount;
        HealthText.text = Health.ToString();
    }

    public void ChangeStamina(int amount)
    {
        Stamina += amount;
        StaminaText.text = Stamina.ToString();
    }
}
