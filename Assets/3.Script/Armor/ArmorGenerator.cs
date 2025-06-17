using UnityEngine;

public class ArmorGenerator : MonoBehaviour
{
    [SerializeField] private GameObject droppedItemPrefab;
    [SerializeField] private int dropCount = 3;
    [SerializeField] private float dropRadius = 3f;

    private void Start()
    {
        Vector3 playerPos = Player.localPlayer.transform.position;

        for (int i = 0; i < dropCount; i++)
        {
            Vector2 randomCircle = Random.insideUnitCircle * dropRadius;
            Vector3 spawnPos = new Vector3(playerPos.x + randomCircle.x, playerPos.y + 1, playerPos.z + randomCircle.y);

            Instantiate(droppedItemPrefab, spawnPos, Quaternion.identity);
        }
    }
}