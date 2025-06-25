using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawnManager : MonoBehaviour
{
    [Header("스폰 포인트 그룹")]
    [SerializeField] private Transform spawnRoot; // ZombieSpawn 오브젝트

    [Header("좀비 프리팹")]
    [SerializeField] private GameObject zombiePrefab;

    [Header("총 좀비 수")]
    [SerializeField] private int totalZombieCount = 10;

    private List<Transform> spawnPoints = new List<Transform>();
    private List<GameObject> spawnedZombies = new List<GameObject>();

    private void Start()
    {
        // 하위 모든 스폰포인트 가져오기
        foreach (Transform child in spawnRoot)
            spawnPoints.Add(child);

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

        // 총 좀비 수를 3개 지점에 균등 분배
        int zombiesPerPoint = totalZombieCount / 3;
        for (int i = 0; i < selectedPoints.Count; i++)
        {
            for (int j = 0; j < zombiesPerPoint; j++)
            {
                GameObject zombie = Instantiate(zombiePrefab, selectedPoints[i].position, Quaternion.identity);
                spawnedZombies.Add(zombie);
                CombatSystem.Instance.RegisterMonster(zombie.GetComponent<IMonster>()); // CombatSystem 연동
            }
        }

        // 좀비 배열을 NextStageController에 전달
        NextStageController.Instance?.SetZombies(spawnedZombies);
    }
}