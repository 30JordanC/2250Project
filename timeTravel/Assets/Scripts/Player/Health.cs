using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
	public float currentHealth;
    public float passiveHealRate;
    public float healthRegenDelay;
    private float lastDamagedTime;
    public DeathUI deathUI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastDamagedTime + healthRegenDelay && currentHealth < maxHealth)
        {
            Heal(passiveHealRate*Time.deltaTime);
        }
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
    }

    public void Die()
    {
        Debug.Log("Player died");

        if (deathUI != null)
        {
            deathUI.ShowDeathScreen();
        }
    }
}
