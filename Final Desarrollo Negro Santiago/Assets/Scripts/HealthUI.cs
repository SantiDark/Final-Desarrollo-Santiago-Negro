using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health target;
    public Slider slider;

    void Start()
    {
        if (!target) return;

        target.OnHealthChanged += UpdateUI;
        UpdateUI(target.currentHealth, target.maxHealth);
    }

    void UpdateUI(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
