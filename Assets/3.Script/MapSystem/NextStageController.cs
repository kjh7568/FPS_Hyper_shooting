using UnityEngine;
using System;
using System.Collections.Generic;


public class NextStageController : MonoBehaviour
{
    public static NextStageController Instance;

    [SerializeField] private GameObject door;

    private List<GameObject> zombies = new List<GameObject>();
    private bool isDoorOpened = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (isDoorOpened) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C 키 입력: 다음 씬 강제 이동");
            GoToNextStage();
        }

        if (CombatSystem.Instance.AreAllMonstersDead())
        {
            Debug.Log("좀비 전원 처치 완료: 문 오픈 + 다음 씬 이동 준비");
            OpenDoor();
        }
    }

    public void SetZombies(List<GameObject> zombieList)
    {
        zombies = zombieList;
    }
    public void GoToNextStage()
    {
        StageManager.Instance.LoadNextScene();
    }
    private void OpenDoor()
    {
        isDoorOpened = true;
        if (door != null)
        {
            door.SetActive(false);
            Debug.Log("문이 열렸습니다.");
        }
    }
}