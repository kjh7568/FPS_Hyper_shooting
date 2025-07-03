using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    private bool isTransitioning = false;
    private List<string> sceneSequence;
    private int currentStageIndex = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitSceneSequence();
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (var scene in sceneSequence)
        {
            Debug.Log($"씬 순서: {scene}");
        }
    }

    private void InitSceneSequence()
    {
        // 중간 맵 셔플
        var randomMaps = new List<string> { "MapDesign1", "MapDesign2", "MapDesign3", "MapDesign4", "MapDesign5" };

        for (int i = randomMaps.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (randomMaps[i], randomMaps[rnd]) = (randomMaps[rnd], randomMaps[i]);
        }

        sceneSequence = new List<string>();
        
        sceneSequence.Add("CoreScene"); // 시작
        sceneSequence.AddRange(randomMaps); // 랜덤 5개
        sceneSequence.Add("BossStage"); // 마지막
    }

    public void LoadNextScene()
    {
        if (isTransitioning) return;
        
        if (currentStageIndex >= sceneSequence.Count - 1)
        {
            Debug.Log("모든 씬을 완료했습니다.");
            return;
        }

        currentStageIndex++;
        string nextSceneName = sceneSequence[currentStageIndex];

        isTransitioning = true;
        SaveCurrentState();
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(nextSceneName);
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