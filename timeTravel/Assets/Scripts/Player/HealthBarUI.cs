using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public Health health;

    public Slider slider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = health.maxHealth;
        slider.value = health.currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = health.currentHealth;
    }
}
