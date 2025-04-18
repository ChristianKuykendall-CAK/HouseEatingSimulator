using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    // Create a dictionary to store the inventory item data as the key and inventory item as the value.
    public Dictionary<InventoryItemData, InventoryItem> m_itemDictionary;

    [SerializeField]
    private List<InventoryItem> inventory = new List<InventoryItem>();

    public List<InventoryItem> Inventory => inventory;

    public static InventorySystem current;

    [SerializeField]
    private CraftingManager craftingManager;

    private void Awake()
    {
        m_itemDictionary = new Dictionary<InventoryItemData, InventoryItem>();
        current = this;
    }

    public void Start()
    {
        craftingManager.UpdateCraftableItems();
    }

    public InventoryItem get(InventoryItemData referenceData)
    {
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            return value;
        }
        return null;
    }

    // Add the inventory item and all of its data to the inventory list/dictionary.
    public void Add(InventoryItemData referenceData)
    {
        // Check if the item is already in the inventory dictionary
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            // If the item is present, add 1 to the stack.
            value.AddToStack();
        }
        else
        {
            // If the item is not present, Create a new inventory item using the reference data, Append it to the list, Create an entry in the dictionary.
            InventoryItem newItem = new InventoryItem(referenceData);
            inventory.Add(newItem);
            m_itemDictionary.Add(referenceData, newItem);
        }
        // If ADD is called, check to see if an item is now craftable.
        craftingManager.UpdateCraftableItems();
        FindObjectOfType<InventoryUIManager>().OnItemChanged();
    }

    // Remove an item and all of its data from the inventory list/dictionary.
    public void Remove(InventoryItemData referenceData)
    {
        // Get the value from the reference data as the key.
        if (m_itemDictionary.TryGetValue(referenceData, out InventoryItem value))
        {
            // Call the remove from stack function on value
            value.RemoveFromStack();

            // If there is none in the stack then remove the data from the list and dictionary
            if (value.StackSize == 0)
            {
                inventory.Remove(value);
                m_itemDictionary.Remove(referenceData);
            }
        }
        // if REMOVE is called, update which item is currently craftable
        craftingManager.UpdateCraftableItems();
        FindObjectOfType<InventoryUIManager>().OnItemChanged();
    }
}



[System.Serializable]
public class InventoryItem
{
    [SerializeField]
    private InventoryItemData data;

    [SerializeField]
    private int stackSize;

    public InventoryItemData Data => data;
    public int StackSize => stackSize;

    public InventoryItem(InventoryItemData source)
    {
        data = source;
        if (data.displayName == "FORK")
        {
            PlayerController.instance.SUCKTIME();
        }
        AddToStack();
    }

    public void AddToStack()
    {
        stackSize++;
    }

    public void RemoveFromStack()
    {
        stackSize--;
    }
}
