using UnityEngine;

public class Stamina : MonoBehaviour
{
    public float maxStamina;

    public float currentStamina;

    public float sprintStaminaDrain;

    public float jumpStaminaDrain;

    public float staminaRegen;

    public float staminaRegenDelay;

    private float lastStaminaUseTime;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentStamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= lastStaminaUseTime + staminaRegenDelay && currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        }
    }

    public bool HasStamina()
    {
        return currentStamina > 0f;
    }

    public void UseStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
        lastStaminaUseTime = Time.time;
    }
}
