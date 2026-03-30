using UnityEngine;
using System.Collections;
using System.Collections.Generic;



public class GlobalState : MonoBehaviour
{
    public float resourceHealth;
    public float resourceMaxHealth;  
     public static GlobalState Instance {get; set; }


 private void Awake()

    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);

        }
        else
        {
            Instance = this;
        }
    }




}
