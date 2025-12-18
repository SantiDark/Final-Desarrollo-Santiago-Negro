using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Health target;
    public Slider slider;

    void Awake()
    {
        if (slider == null)
            slider = GetComponentInChildren<Slider>(true);
    }

    void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                target = player.GetComponent<Health>();
        }

        if (target != null)
        {
            target.OnHealthChanged += UpdateBar;
            UpdateBar(target.currentHealth, target.maxHealth);
        }
    }

    void OnDestroy()
    {
        if (target != null)
            target.OnHealthChanged -= UpdateBar;
    }

    void UpdateBar(int current, int max)
    {
        slider.maxValue = max;
        slider.value = current;
    }
}
