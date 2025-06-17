using UnityEngine;

public class ArmorGenerator : MonoBehaviour
{
    public static ArmorGenerator instance;
    
    [SerializeField] private GameObject droppedItemPrefab;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnItem(Vector3 position)
    {
        Instantiate(droppedItemPrefab, position + new Vector3(0, 0.6f, 0), Quaternion.identity, transform);
    }
}