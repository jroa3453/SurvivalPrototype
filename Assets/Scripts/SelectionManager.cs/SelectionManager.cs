using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public bool onTarget;
    public static SelectionManager Instance { get; set; }
    public GameObject selectedObject;

    public GameObject interaction_Info_UI;
    Text interaction_text;

    public Image centerDotImage;
    public Image handIcon;

    public GameObject SelectedTree;
    public GameObject chopHolder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<Text>();
        chopHolder.SetActive(false);
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            ChoppableTree choppableTree = selectionTransform.GetComponentInParent<ChoppableTree>();

            if (choppableTree != null && choppableTree.playerInRange)
            {
                if (SelectedTree != selectionTransform.gameObject)
                {
                    ClearTreeSelection();
                    SelectedTree = selectionTransform.gameObject;
                    choppableTree.canBeChopped = true;
                }

                chopHolder.SetActive(true);
                GlobalState.Instance.resourceHealth = choppableTree.treeHealth;
                GlobalState.Instance.resourceMaxHealth = choppableTree.treeMaxHealth;

                interaction_Info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);

<<<<<<< HEAD
                if (Input.GetMouseButtonDown(0) && choppableTree.canBeChopped && EquipSystem.Instance.IsHoldingWeapon())
                {
=======
                 if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Clicked! IsHoldingWeapon: " + EquipSystem.Instance.IsHoldingWeapon());
                    Debug.Log("canBeChopped: " + choppableTree.canBeChopped);
                }

                if (Input.GetMouseButtonDown(0) && choppableTree.canBeChopped && EquipSystem.Instance.IsHoldingWeapon())
                {
>>>>>>> 0a5989b6fd4b22784c4c20e3b41f614aac0069e4
                    choppableTree.GetHit();
                }
            }
            else
            {
                if (SelectedTree != null)
                {
                    ChoppableTree old = SelectedTree.GetComponentInParent<ChoppableTree>();
                    if (old != null) old.canBeChopped = false;
                    SelectedTree = null;
                    chopHolder.SetActive(false);
                }

                Animal animal = selectionTransform.GetComponentInParent<Animal>();
                if (animal != null && animal.playerInRange)
                {
                    if (animal.isDead)
                    {
                        interaction_text.text = "Loot";
                        interaction_Info_UI.SetActive(true);
                        centerDotImage.gameObject.SetActive(false);
                        handIcon.gameObject.SetActive(true);

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            Lootable lootable = animal.GetComponent<Lootable>();
                            Loot(lootable);
                        }
                    }
                    else
                    {
                        selectedObject = selectionTransform.gameObject;
                        interaction_text.text = animal.animalName;
                        interaction_Info_UI.SetActive(true);
                        centerDotImage.gameObject.SetActive(true);
                        handIcon.gameObject.SetActive(false);

                        if (Input.GetMouseButtonDown(0) && EquipSystem.Instance.IsHoldingWeapon())
                        {
                            EquippableItem equippedItem = EquipSystem.Instance.selectedItem.GetComponent<EquippableItem>();
                            if (equippedItem != null && equippedItem.canHitAnimals)
                                StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
                        }
                        return;
                    }
                }

                var interactable = selectionTransform.GetComponent<InteractableObject>();

                if (!interactable && !animal)
                {
                    onTarget = false;
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                }

                if (!interactable && !animal && !choppableTree)
                {
                    interaction_text.text = "";
                    interaction_Info_UI.SetActive(false);
                }
                else
                {
                    if (interactable != null && interactable.playerInRange)
                    {
                        onTarget = true;
                        selectedObject = interactable.gameObject;
                        interaction_text.text = "";
                        interaction_Info_UI.SetActive(true);

                        if (interactable.CompareTag("Pickable"))
                        {
                            centerDotImage.gameObject.SetActive(false);
                            handIcon.gameObject.SetActive(true);
                        }
                        else
                        {
                            centerDotImage.gameObject.SetActive(true);
                            handIcon.gameObject.SetActive(false);
                        }
                    }
                    else
                    {
                        if (animal == null)
                        {
                            onTarget = false;
                            selectedObject = null;
                            interaction_Info_UI.SetActive(false);
                            centerDotImage.gameObject.SetActive(true);
                            handIcon.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        else
        {
            if (SelectedTree != null)
            {
                ChoppableTree old = SelectedTree.GetComponentInParent<ChoppableTree>();
                if (old != null) old.canBeChopped = false;
                SelectedTree = null;
                chopHolder.SetActive(false);
            }

            onTarget = false;
            selectedObject = null;
            interaction_Info_UI.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
    }

    private void Loot(Lootable lootable)
    {
        if (lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();

            foreach (LootPossibility loot in lootable.possibleLoot)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax + 1);
                if (lootAmount != 0)
                {
                    LootRecieved lT = new LootRecieved();
                    lT.item = loot.item;
                    lT.amount = lootAmount;
                    recievedLoot.Add(lT);
                }
            }
            lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }

        Vector3 lootSpawnPosition = lootable.gameObject.transform.position;

        foreach (LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i = 0; i < lootRecieved.amount; i++)
            {
                GameObject lootSpawn = Instantiate(Resources.Load<GameObject>(lootRecieved.item.name + "_Model"),
                new Vector3(lootSpawnPosition.x, lootSpawnPosition.y + 0.2f, lootSpawnPosition.z),
                Quaternion.Euler(0, 0, 0));
            }
        }
    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
    }

    void ClearTreeSelection()
    {
        if (SelectedTree != null)
        {
            ChoppableTree tree = SelectedTree.GetComponentInParent<ChoppableTree>();
            if (tree != null) tree.canBeChopped = false;
        }

        SelectedTree = null;
        chopHolder.SetActive(false);

        GlobalState.Instance.resourceHealth = 0f;
        GlobalState.Instance.resourceMaxHealth = 0f;
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        centerDotImage.enabled = false;
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
    }

    public void EnableSelection()
    {
        handIcon.enabled = true;
        centerDotImage.enabled = true;
        interaction_Info_UI.SetActive(true);
    }
}