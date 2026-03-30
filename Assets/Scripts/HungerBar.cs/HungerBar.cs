using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;   
public class HungerBar : MonoBehaviour
{
    
    public Text caloriesCounter;

    private Slider slider;

    public GameObject playerState;

    private float currentCalories;
    private float maxCalories;

    void Awake()
    {
       slider = GetComponent<Slider>();
       caloriesCounter = GetComponentInChildren<Text>(); 
    }

    
    void Update()
    {
        currentCalories = playerState.GetComponent<PlayerState>().currentCalories;
        maxCalories = playerState.GetComponent<PlayerState>().maxCalories;

        float fillValue = currentCalories / maxCalories;
        slider.value = fillValue;
        caloriesCounter.text = currentCalories + " / " + maxCalories;
    }
}
