using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField]
    public List<Recipe> recipes = new List<Recipe>();
    public InventorySystem inventory;
    public TextMeshProUGUI recipeText;

    public void UpdateCraftableItems()
    {
        var validRecipe = GetCraftableRecipe();
        if (validRecipe != null)
        {
            recipeText.text = $"Craftable: {validRecipe.result}";
            PlayerController.instance.SetItemToCraft(validRecipe.result);
        }
        else
        {
            recipeText.text = "No Craftable Items";
            PlayerController.instance.SetItemToCraft(null);
        }
    }

    private Recipe GetCraftableRecipe()
    {
        Recipe mostIngredientRecipe = null;

        foreach (var recipe in recipes)
        {
            if (HasIngredients(recipe.ingredients))
            {
                if (mostIngredientRecipe == null || recipe.ingredients.Count > mostIngredientRecipe.ingredients.Count)
                {
                    mostIngredientRecipe = recipe;
                }
            }
        }
        return mostIngredientRecipe;
    }

    public bool Craft(InventoryItemData itemToCraft)
    {
        Recipe recipe = recipes.Find(r => r.result == itemToCraft);

        if(recipe != null && HasIngredients(recipe.ingredients))
        {
            foreach (var ingredient in recipe.ingredients)
            {
                inventory.Remove(ingredient);
            }

            GameObject craftedItem = Instantiate(itemToCraft.mainPrefab, PlayerController.instance.transform.position + PlayerController.instance.transform.forward + new Vector3(0, 1f, 0), Quaternion.identity);

            Rigidbody rb = craftedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 throwForce = PlayerController.instance.transform.forward * 10f;
                rb.AddForce(throwForce, ForceMode.Impulse);
            }

            return true;
        }
        else
        {
            DropAllItems();
            return false;
        }
    }

    public void DropAllItems()
    {
        var itemsToDrop = new List<InventoryItem>(inventory.Inventory);

        foreach (var item in itemsToDrop)
        {
            while (item.StackSize > 0)
            {
                GameObject droppedItem = Instantiate(item.Data.mainPrefab,
                    PlayerController.instance.transform.position + PlayerController.instance.transform.forward, Quaternion.identity);

                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(PlayerController.instance.transform.forward * 2f, ForceMode.Impulse);
                }

                InventorySystem.current.Remove(item.Data); // Remove using InventoryItem
            }
        }
    }

    private bool HasIngredients(List<InventoryItemData> ingredients)
    {
        Dictionary<InventoryItemData, int> requiredCounts = new Dictionary<InventoryItemData, int>();

        foreach (var ingredient in ingredients)
        {
            if (requiredCounts.ContainsKey(ingredient))
            {
                requiredCounts[ingredient]++;
            }
            else
            {
                requiredCounts[ingredient] = 1;
            }
        }
        foreach (var required in requiredCounts)
        {
            InventoryItem itemInInventory = inventory.get(required.Key);
            if(itemInInventory == null || itemInInventory.StackSize < required.Value)
            {
                return false;
            }
        }
        return true;
    }

    [System.Serializable]
    public class Recipe
    {
        public InventoryItemData result;
        public List<InventoryItemData> ingredients;
    }
}