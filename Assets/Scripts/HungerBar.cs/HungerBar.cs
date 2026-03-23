using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;   
public class HungerBar : MonoBehaviour
{
    
    public Text hungerCounter;

    private Slider slider;

    public GameObject playerState;

    private float currentHunger;
    private float maxHunger;

    void Awake()
    {
       slider = GetComponent<Slider>();
       hungerCounter = GetComponentInChildren<Text>(); 
    }

    
    void Update()
    {
        currentHunger = playerState.GetComponent<PlayerState>().currentHunger;
        maxHunger = playerState.GetComponent<PlayerState>().maxHunger;

        float fillValue = currentHunger / maxHunger;
        slider.value = fillValue;
        hungerCounter.text = currentHunger + " / " + maxHunger;
    }
}
