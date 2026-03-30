using UnityEngine;
using UnityEngine.UI;

public class ResourceHealthText : MonoBehaviour
{
    public Text resourceHealthText;

    void Update()
    {
        if (SelectionManager.Instance == null)
        {
            return;
        }

        if (SelectionManager.Instance.selectedObject == null)
        {
            resourceHealthText.text = "";
            return;
        }

        ChoppableTree tree = SelectionManager.Instance.selectedObject.GetComponentInParent<ChoppableTree>();

        if (tree == null)
        {
            resourceHealthText.text = "";
            return;
        }

        resourceHealthText.text = tree.treeHealth.ToString();
    }
}