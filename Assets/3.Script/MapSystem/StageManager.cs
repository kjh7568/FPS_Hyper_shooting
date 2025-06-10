using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;
    private bool isTransitioning = false;

    [SerializeField]
    private List<string> stageOrder = new List<string> { "Stage1", "Stage2", "BossStage" };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadNextStage()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        int index = stageOrder.IndexOf(currentScene);

        if (index >= 0 && index < stageOrder.Count - 1)
        {
            LoadScene(stageOrder[index + 1]);
        }
        else
        {
            Debug.Log("마지막 스테이지이거나 잘못된 씬 이름입니다.");
        }
    }

    private void SaveCurrentState()
    {
        Gun gun = FindObjectOfType<Gun>();
        if (gun != null)
        {
            GameData.Instance.SaveGunState(gun);
        }
    }

    public void LoadScene(string sceneName)
    {
        if (isTransitioning) return;  // 중복 방지
        isTransitioning = true;

        SaveCurrentState();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject spawnPointObj = GameObject.Find("SpawnPoint");
        if (spawnPointObj != null)
        {
            Vector3 spawnPoint = spawnPointObj.transform.position;
            SetPlayerSpawnPosition(spawnPoint);
        }
        else
        {
            Debug.LogWarning("SpawnPoint 오브젝트를 찾을 수 없습니다.");
        }

        Gun gun = FindObjectOfType<Gun>();
        if (gun != null)
        {
            GameData.Instance.LoadGunState(gun);
        }

        isTransitioning = false;  // 방어 플래그 초기화
    }

    public void SetPlayerSpawnPosition(Vector3 spawnPoint)
    {
        GameObject player = PlayerManager.Instance.GetPlayer();
        if (player == null)
        {
            Debug.LogWarning("플레이어 인스턴스를 찾을 수 없습니다.");
            return;
        }

        CharacterController cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.position = spawnPoint;

        if (cc != null) cc.enabled = true;

        Debug.Log($"플레이어 스폰 위치 이동: {spawnPoint}");
    }
}
