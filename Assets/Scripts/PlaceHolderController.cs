using UnityEngine;

public class PlaceHolderController : MonoBehaviour
{
    public GameObject placedObject;
    public InventoryItem requiredItem;

    private void OnMouseOver()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {

            Instantiate(placedObject, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
        }
    }
}
