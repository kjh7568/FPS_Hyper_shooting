using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [Header("플레이어 프리팹")]
    [SerializeField] private GameObject playerPrefab;

    private GameObject playerInstance;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            SpawnPlayer();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// 플레이어가 존재하지 않을 경우 스폰
    private void SpawnPlayer()
    {
        if (playerInstance == null)
        {
            playerInstance = Instantiate(playerPrefab);
            DontDestroyOnLoad(playerInstance);
        }
    }
    /// 다음 씬에서 특정 위치로 플레이어 이동
    public GameObject GetPlayer()
    {
        return playerInstance;
    }
}