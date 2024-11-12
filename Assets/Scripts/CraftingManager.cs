using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingManager : MonoBehaviour
{
    [SerializeField]
    public List<Recipe> recipes = new List<Recipe>();  // List to hold all available crafting recipes
    public InventorySystem inventory;  // Reference to the InventorySystem for checking available items
    public TextMeshProUGUI recipeText;  // UI element to display the current craftable item

    // Method to update the list of craftable items
    public void UpdateCraftableItems()
    {
        var validRecipe = GetCraftableRecipe();  // Get the most suitable recipe that can be crafted
        if (validRecipe != null)
        {
            // If a valid recipe is found, display it in the UI and set it for crafting
            recipeText.text = $"Craftable: {validRecipe.result}";
            PlayerController.instance.SetItemToCraft(validRecipe.result);
        }
        else
        {
            // If no valid recipe is found, inform the player that no items can be crafted
            recipeText.text = "No Craftable Items";
            PlayerController.instance.SetItemToCraft(null);  // Set the crafting item to null
        }
    }

    // Method to find the most suitable recipe that can be crafted with the available ingredients
    private Recipe GetCraftableRecipe()
    {
        Recipe mostIngredientRecipe = null;  // Variable to store the recipe with the most ingredients we can craft

        // Iterate through all available recipes
        foreach (var recipe in recipes)
        {
            // Check if we have the required ingredients for this recipe
            if (HasIngredients(recipe.ingredients))
            {
                // If we find a recipe with more ingredients than the current one, update the mostIngredientRecipe
                if (mostIngredientRecipe == null || recipe.ingredients.Count > mostIngredientRecipe.ingredients.Count)
                {
                    mostIngredientRecipe = recipe;
                }
            }
        }
        return mostIngredientRecipe;  // Return the recipe that requires the most ingredients and is craftable
    }

    // Method to craft an item using a specific recipe
    public bool Craft(InventoryItemData itemToCraft)
    {
        // Find the recipe for the item to craft
        Recipe recipe = recipes.Find(r => r.result == itemToCraft);

        // Check if the recipe is valid and if we have enough ingredients
        if (recipe != null && HasIngredients(recipe.ingredients))
        {
            // Remove the required ingredients from the inventory
            foreach (var ingredient in recipe.ingredients)
            {
                inventory.Remove(ingredient);  // Remove the ingredient from the inventory
            }

            // Instantiate the crafted item in the world
            GameObject craftedItem = Instantiate(itemToCraft.mainPrefab, PlayerController.instance.transform.position + PlayerController.instance.transform.forward + new Vector3(0, 1f, 0), Quaternion.identity);

            // Apply a force to the crafted item (for example, to throw it or make it fall)
            Rigidbody rb = craftedItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                Vector3 throwForce = PlayerController.instance.transform.forward * 10f;  // Throw force in the forward direction
                rb.AddForce(throwForce, ForceMode.Impulse);  // Add the force to the item
            }

            return true;  // Successful crafting
        }
        else
        {
            // If crafting is not possible (either no recipe found or ingredients are missing), drop all items
            DropAllItems();
            return false;  // Crafting failed
        }
    }

    // Method to drop all items currently in the inventory
    public void DropAllItems()
    {
        var itemsToDrop = new List<InventoryItem>(inventory.Inventory);  // Create a copy of the current inventory list

        // Iterate through each item in the inventory
        foreach (var item in itemsToDrop)
        {
            // While the item has any stack left, drop it
            while (item.StackSize > 0)
            {
                // Instantiate the item in the world at the player's position
                GameObject droppedItem = Instantiate(item.Data.mainPrefab,
                    PlayerController.instance.transform.position + PlayerController.instance.transform.forward, Quaternion.identity);

                // Apply a force to the dropped item to simulate it being thrown
                Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.AddForce(PlayerController.instance.transform.forward * 2f, ForceMode.Impulse);  // Apply a small throw force
                }

                // Remove the item from the inventory system
                InventorySystem.current.Remove(item.Data);
            }
        }
    }

    // Method to check if the player has the required ingredients for a recipe
    private bool HasIngredients(List<InventoryItemData> ingredients)
    {
        Dictionary<InventoryItemData, int> requiredCounts = new Dictionary<InventoryItemData, int>();  // Dictionary to track how many of each ingredient are needed

        // Count the required ingredients for the recipe
        foreach (var ingredient in ingredients)
        {
            if (requiredCounts.ContainsKey(ingredient))
            {
                requiredCounts[ingredient]++;  // Increment the count if the ingredient is already in the dictionary
            }
            else
            {
                requiredCounts[ingredient] = 1;  // Otherwise, add it with a count of 1
            }
        }

        // Check if the player has enough of each required ingredient
        foreach (var required in requiredCounts)
        {
            // Get the corresponding inventory item for this ingredient
            InventoryItem itemInInventory = inventory.get(required.Key);

            // If the ingredient is not found or the stack size is not enough, return false (not enough ingredients)
            if (itemInInventory == null || itemInInventory.StackSize < required.Value)
            {
                return false;
            }
        }
        return true;  // All required ingredients are available
    }

    // A recipe is a structure that contains the result and the list of ingredients needed to craft it
    [System.Serializable]
    public class Recipe
    {
        public InventoryItemData result;  // The item that will be created by crafting this recipe
        public List<InventoryItemData> ingredients;  // The list of ingredients required to craft the result
    }
}
