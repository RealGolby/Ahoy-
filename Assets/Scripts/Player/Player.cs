using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public int Health;
    public int Stamina;

    [SerializeField] TMP_Text HealthText;
    [SerializeField] TMP_Text StaminaText;

    public void ChangeHealth(int amount)
    {
        Health = amount;
        HealthText.text = amount.ToString();
    }

    public void ChangeStamina(int amount)
    {
        Stamina = amount;
        StaminaText.text = amount.ToString();
    }
}
