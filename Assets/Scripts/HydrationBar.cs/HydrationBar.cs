using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HydrationBar : MonoBehaviour
{

    public Text hydrationPercentCounter;
    private Slider slider;

    public GameObject playerState;

    private float currentHydrationPercent;
    private float maxHydrationPercent;

    void Awake()
    {
       slider = GetComponent<Slider>();
    }

    
    void Update()
    {
        currentHydrationPercent = playerState.GetComponent<PlayerState>().currentHydrationPercent;
        maxHydrationPercent = playerState.GetComponent<PlayerState>().maxHydrationPercent;

        float fillValue = currentHydrationPercent / maxHydrationPercent;
        slider.value = fillValue;
        hydrationPercentCounter.text = currentHydrationPercent + " % ";

    }
}
