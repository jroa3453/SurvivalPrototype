using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

            // DEBUG — remove after fixing
            Debug.Log("Ray hit: " + selectionTransform.name);

            ChoppableTree choppableTree = selectionTransform.GetComponentInParent<ChoppableTree>();

            // DEBUG — remove after fixing
            Debug.Log("ChoppableTree found: " + (choppableTree != null) + " | playerInRange: " + (choppableTree != null ? choppableTree.playerInRange.ToString() : "N/A"));

            if (choppableTree != null && choppableTree.playerInRange)
            {
                if (SelectedTree != selectionTransform.gameObject)
                {
                    ClearTreeSelection();
                    SelectedTree = selectionTransform.gameObject;
                    choppableTree.canBeChopped = true;
                }

                // Always show bar and update health while looking at tree
                chopHolder.SetActive(true);
                GlobalState.Instance.resourceHealth    = choppableTree.treeHealth;
                GlobalState.Instance.resourceMaxHealth = choppableTree.treeMaxHealth;

                // Hide interact UI
                interaction_Info_UI.SetActive(false);
                centerDotImage.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
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

                var interactable = selectionTransform.GetComponent<InteractableObject>();
                if (interactable != null && interactable.playerInRange)
                {
                    onTarget = true;
                    selectedObject = interactable.gameObject;
                    interaction_text.text = interactable.GetItemName();
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
                    onTarget = false;
                    interaction_Info_UI.SetActive(false);
                    centerDotImage.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
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
            interaction_Info_UI.SetActive(false);
            centerDotImage.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
        }
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

        GlobalState.Instance.resourceHealth    = 0f;
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