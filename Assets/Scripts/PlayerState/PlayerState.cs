using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour
{
    public static PlayerState Instance { get; set; }

    // Player Health
    public float currentHealth;
    public float maxHealth;

    // Player Hunger
    public float currentCalories;
    public float maxCalories;

    // Player Hydration
    public float currentHydrationPercent;
    public float maxHydrationPercent;

    private float distanceTraveled = 0f;
    private Vector3 lastPosition;

    public GameObject playerModel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentCalories = maxCalories;
        currentHydrationPercent = maxHydrationPercent;

        if (playerModel != null)
        {
            lastPosition = playerModel.transform.position;
        }

        StartCoroutine(DecreaseHydration());
    }

    void Update()
    {
        if (playerModel == null) return;

        distanceTraveled += Vector3.Distance(playerModel.transform.position, lastPosition);
        lastPosition = playerModel.transform.position;

        if (distanceTraveled >= 10f)
        {
            currentCalories -= 5f;
            distanceTraveled = 0f;
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentHealth -= 10f;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            currentCalories -= 10f;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            currentHydrationPercent -= 10f;
        }

        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentCalories = Mathf.Clamp(currentCalories, 0f, maxCalories);
        currentHydrationPercent = Mathf.Clamp(currentHydrationPercent, 0f, maxHydrationPercent);
    }

    private IEnumerator DecreaseHydration()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            currentHydrationPercent -= 1f;
            currentHydrationPercent = Mathf.Clamp(currentHydrationPercent, 0f, maxHydrationPercent);
        }
    }

    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    }

    public void SetCalories(float value)
    {
        currentCalories = Mathf.Clamp(value, 0f, maxCalories);
    }

    public void SetHydration(float value)
    {
        currentHydrationPercent = Mathf.Clamp(value, 0f, maxHydrationPercent);
    }
}