using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawnManager : MonoBehaviour
{
    [Header("스폰 포인트 그룹")]
    [SerializeField] private Transform spawnRoot; // ZombieSpawn 오브젝트

    [Header("좀비 프리팹 (3종류)")]
    [SerializeField] private GameObject[] zombiePrefabs;

    [Header("총 좀비 수 (30~50 랜덤)")]
    private int totalZombieCount;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> spawnedZombies = new List<GameObject>();

    private void Start()
    {
        // 스폰 포인트 수집
        foreach (Transform child in spawnRoot)
            spawnPoints.Add(child);

        // 총 좀비 수 랜덤 설정
        totalZombieCount = Random.Range(30, 51);

        SpawnZombies();
    }

    private void SpawnZombies()
    {
        // 스폰 포인트 중 랜덤으로 3개 선택
        List<Transform> selectedPoints = new List<Transform>();
        while (selectedPoints.Count < 3)
        {
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
            if (!selectedPoints.Contains(randomPoint))
                selectedPoints.Add(randomPoint);
        }

        // 균등 분배
        int zombiesPerPoint = totalZombieCount / 3;
        int remaining = totalZombieCount % 3;

        for (int i = 0; i < selectedPoints.Count; i++)
        {
            int count = zombiesPerPoint + (i < remaining ? 1 : 0); // 남은 좀비 분배
            for (int j = 0; j < count; j++)
            {
                // 좀비 프리팹 중 랜덤 선택
                GameObject selectedZombiePrefab = zombiePrefabs[Random.Range(0, zombiePrefabs.Length)];

                GameObject zombie = Instantiate(selectedZombiePrefab, selectedPoints[i].position, Quaternion.identity);
                spawnedZombies.Add(zombie);

                CombatSystem.Instance.RegisterMonster(zombie.GetComponent<IMonster>());
            }
        }

        // NextStageController에 등록
        NextStageController.Instance?.SetZombies(spawnedZombies);
    }
}