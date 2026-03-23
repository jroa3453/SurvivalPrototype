using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler,
    IPointerDownHandler,
    IPointerUpHandler
{
    // Is this item trashable
    public bool isTrashable;

    // Item Info UI
    private GameObject itemInfoUI;

    private Text itemInfoUI_itemName;
    private Text itemInfoUI_itemDescription;
    private Text itemInfoUI_itemFunctionality;

    public string thisName;
    public string thisDescription;
    public string thisFunctionality;

    // Consumption
    private GameObject itemPendingConsumption;
    public bool isConsumable;

    public float healthEffect;
    public float caloriesEffect;
    public float hydrationEffect;

    private void Start()
    {
        if (InventorySystem.Instance == null)
        {
            Debug.LogError("InventorySystem.Instance is null");
            return;
        }

        itemInfoUI = InventorySystem.Instance.ItemInfoUI;

        if (itemInfoUI == null)
        {
            Debug.LogError("ItemInfoUI is null on InventorySystem");
            return;
        }

        itemInfoUI_itemName = itemInfoUI.transform.Find("itemName")?.GetComponent<Text>();
        itemInfoUI_itemDescription = itemInfoUI.transform.Find("itemDescription")?.GetComponent<Text>();
        itemInfoUI_itemFunctionality = itemInfoUI.transform.Find("itemFunctionality")?.GetComponent<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemInfoUI == null) return;

        itemInfoUI.SetActive(true);

        if (itemInfoUI_itemName != null)
            itemInfoUI_itemName.text = thisName;

        if (itemInfoUI_itemDescription != null)
            itemInfoUI_itemDescription.text = thisDescription;

        if (itemInfoUI_itemFunctionality != null)
            itemInfoUI_itemFunctionality.text = thisFunctionality;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemInfoUI != null)
            itemInfoUI.SetActive(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && isConsumable)
        {
            itemPendingConsumption = gameObject;
            ConsumingFunction(healthEffect, caloriesEffect, hydrationEffect);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (isConsumable && itemPendingConsumption == gameObject)
            {
                Destroy(gameObject);

                if (InventorySystem.Instance != null)
                    InventorySystem.Instance.ReCalculateList();

                if (CraftingSystem.Instance != null)
                    CraftingSystem.Instance.RefreshNeededItems();
            }
        }
    }

    private void ConsumingFunction(float healthEffect, float caloriesEffect, float hydrationEffect)
    {
        if (itemInfoUI != null)
            itemInfoUI.SetActive(false);

        HealthEffectCalculation(healthEffect);
        CaloriesEffectCalculation(caloriesEffect);
        HydrationEffectCalculation(hydrationEffect);
    }

    private void HealthEffectCalculation(float healthEffect)
    {
        if (PlayerState.Instance == null) return;

        float healthBeforeConsumption = PlayerState.Instance.currentHealth;
        float maxHealth = PlayerState.Instance.maxHealth;

        if (healthEffect != 0)
        {
            if ((healthBeforeConsumption + healthEffect) > maxHealth)
            {
                PlayerState.Instance.SetHealth(maxHealth);
            }
            else
            {
                PlayerState.Instance.SetHealth(healthBeforeConsumption + healthEffect);
            }
        }
    }

    private void CaloriesEffectCalculation(float caloriesEffect)
    {
        if (PlayerState.Instance == null) return;

        float caloriesBeforeConsumption = PlayerState.Instance.currentHunger;
        float maxCalories = PlayerState.Instance.maxHunger;

        if (caloriesEffect != 0)
        {
            if ((caloriesBeforeConsumption + caloriesEffect) > maxCalories)
            {
                PlayerState.Instance.SetCalories(maxCalories);
            }
            else
            {
                PlayerState.Instance.SetCalories(caloriesBeforeConsumption + caloriesEffect);
            }
        }
    }

    private void HydrationEffectCalculation(float hydrationEffect)
    {
        if (PlayerState.Instance == null) return;

        float hydrationBeforeConsumption = PlayerState.Instance.currentHydrationPercent;
        float maxHydration = PlayerState.Instance.maxHydrationPercent;

        if (hydrationEffect != 0)
        {
            if ((hydrationBeforeConsumption + hydrationEffect) > maxHydration)
            {
                PlayerState.Instance.SetHydration(maxHydration);
            }
            else
            {
                PlayerState.Instance.SetHydration(hydrationBeforeConsumption + hydrationEffect);
            }
        }
    }
}