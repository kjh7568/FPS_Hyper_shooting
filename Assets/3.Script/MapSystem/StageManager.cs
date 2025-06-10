using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

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
            string nextScene = stageOrder[index + 1];
            LoadScene(nextScene);
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
        SaveCurrentState();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(sceneName);
    }

    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; 
        
        Vector3 spawnPoint = GameObject.Find("SpawnPoint").transform.position;
        PlayerManager.Instance.SetPlayerSpawnPosition(spawnPoint);
        
        Gun gun = FindObjectOfType<Gun>();
        if (gun != null)
        {
            GameData.Instance.LoadGunState(gun);
        }
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}