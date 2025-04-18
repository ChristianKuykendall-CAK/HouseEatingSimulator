using UnityEngine;

public class PlaceHolderController : MonoBehaviour
{
    public GameObject placedObject;
    public InventoryItemData requiredItem;

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (InventorySystem.current.m_itemDictionary.TryGetValue(requiredItem, out InventoryItem value)){
                Instantiate(placedObject, gameObject.transform.position, gameObject.transform.rotation);
                InventorySystem.current.Remove(value.Data);
                Destroy(gameObject);
            }
        }
    }
}
