using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator instance;
    
    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] private GameObject droppedGoldPrefab;
    [SerializeField] private GameObject droppedCorePrefab;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnItem(Vector3 position)
    {
        Instantiate(droppedItemPrefab, position + new Vector3(0, 0.6f, 0), Quaternion.identity, transform);
    }

    public void SpawnGold(Vector3 position, int goldCount)
    {
        float spreadForce = 5f;

        for (int i = 0; i < goldCount; i++)
        {
            GameObject gold = Instantiate(droppedGoldPrefab, position, Quaternion.identity);
            Rigidbody rb = gold.GetComponent<Rigidbody>();
            rb.drag = 2f;           // 공기 저항
            rb.angularDrag = 5f;
            
            if (rb != null)
            {
                // 분수처럼 위로 튀고 퍼지게 하는 힘
                Vector3 forceDir = Random.insideUnitSphere + Vector3.up * 1.5f;
                rb.AddForce(forceDir.normalized * spreadForce, ForceMode.Impulse);
            }
        }
    }

    public void SpawnCore(Vector3 position, int coreCount)
    {
        float spreadForce = 5f;

        for (int i = 0; i < coreCount; i++)
        {
            GameObject gold = Instantiate(droppedCorePrefab, position, Quaternion.identity);
            Rigidbody rb = gold.GetComponent<Rigidbody>();
            rb.drag = 2f;           // 공기 저항
            rb.angularDrag = 5f;
            
            if (rb != null)
            {
                // 분수처럼 위로 튀고 퍼지게 하는 힘
                Vector3 forceDir = Random.insideUnitSphere + Vector3.up * 1.5f;
                rb.AddForce(forceDir.normalized * spreadForce, ForceMode.Impulse);
            }
        }
    }
}