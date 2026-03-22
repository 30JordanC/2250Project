using UnityEngine;
using UnityEngine.UI;

public class StaminaBarUI : MonoBehaviour
{
    public Stamina stamina;

    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        slider.maxValue = stamina.maxStamina;
        slider.value = stamina.currentStamina;
    }

    // Update is called once per frame
    void Update()
    {
        slider.value = stamina.currentStamina;
    }
}
