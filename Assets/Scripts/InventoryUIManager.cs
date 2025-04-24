using UnityEngine;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public TextMeshProUGUI textMesh;
    public TextMeshProUGUI snapTextMesh;
    public TextMeshProUGUI readTextMesh;
    private InventorySystem inventorySystem;

    void Start()
    {
        inventorySystem = InventorySystem.current;
        UpdateInventoryText();

    }

    void UpdateInventoryText()
    {
        string itemNames = "";

        foreach (var item in inventorySystem.Inventory)
        {
            itemNames += $"{item.Data.displayName} X {item.StackSize}\n";
        }

        textMesh.text = string.IsNullOrEmpty(itemNames) ? "Inv. Empty" : itemNames;
    }

    //if raycasted item is a snapable item, print "E" to screen
    public void UpdateSnapText(bool canSnap)
    {
        if (canSnap)
        {
            snapTextMesh.text = "E";
        }
        else
        {
            snapTextMesh.text = " ";
        }
    }

    //if raycasted item is recipe book, print E
    public void UpdateBookText(bool canRead)
    {
        if (canRead)
        {
            readTextMesh.text = "E";
        }
        else
        {
            readTextMesh.text = " ";
        }
    }

    public void OnItemChanged()
    {
        UpdateInventoryText();
    }

}
