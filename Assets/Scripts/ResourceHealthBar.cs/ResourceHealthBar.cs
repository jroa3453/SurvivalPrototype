using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        if (GlobalState.Instance.resourceMaxHealth > 0)
        {
            slider.value = GlobalState.Instance.resourceHealth / GlobalState.Instance.resourceMaxHealth;
            Debug.Log("Slider value: " + slider.value);

            Debug.Log("Max: " + GlobalState.Instance.resourceMaxHealth + " Current: " + GlobalState.Instance.resourceHealth);
        }
        else
        {
            slider.value = 0f;
        }
    }
}