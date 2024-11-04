using UnityEngine;

[CreateAssetMenu(menuName = "Inventory Item Data")]
public class InventoryItemData : ScriptableObject
{
    public string id;
    public string displayName;
    public GameObject mainPrefab;
    public GameObject prefab2;
    public AudioClip eatingSound;
}
