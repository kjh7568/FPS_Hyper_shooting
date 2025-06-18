using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    private bool isTransitioning = false;
    private int requestedSceneIndex = -1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneByIndex(int index)
    {
        if (isTransitioning) return;
        if (index < 0 || index >= SceneManager.sceneCountInBuildSettings)
        {
            Debug.LogWarning("유효하지 않은 씬 인덱스입니다.");
            return;
        }

        isTransitioning = true;
        requestedSceneIndex = index;

        SaveCurrentState();

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(index);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject player = PlayerManager.Instance?.GetPlayer();
        if (player == null)
        {
            Debug.LogWarning("플레이어 객체를 찾을 수 없습니다.");
            isTransitioning = false;
            return;
        }

        GameObject spawnPointObj = GameObject.Find("SpawnPoint");
        if (spawnPointObj != null)
        {
            Vector3 spawnPos = spawnPointObj.transform.position;

            CharacterController cc = player.GetComponent<CharacterController>();
            if (cc != null) cc.enabled = false;

            player.transform.position = spawnPos;

            if (cc != null) cc.enabled = true;

            Debug.Log($"[StageManager] 플레이어를 스폰 위치로 이동: {spawnPos}");
        }
        else
        {
            Debug.LogWarning("SpawnPoint 오브젝트를 찾지 못했습니다.");
        }

        var weapon = FindObjectOfType<WeaponController>();
        if (weapon != null)
        {
            GameData.Instance.LoadGunState(weapon);
        }

        isTransitioning = false;
    }

    private void SaveCurrentState()
    {
        var weapon = FindObjectOfType<WeaponController>();

        if (weapon != null)
        {
            GameData.Instance.SaveGunState(weapon);
        }
    }
}