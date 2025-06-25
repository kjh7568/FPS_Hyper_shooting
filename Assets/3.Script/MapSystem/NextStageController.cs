using UnityEngine;
using System;
using System.Collections.Generic;


public class NextStageController : MonoBehaviour
{
    public static NextStageController Instance;

    [Header("문 오브젝트")]
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
            Debug.Log("C 키 입력: 문 강제 오픈");
            OpenDoor();
        }

        if (CombatSystem.Instance.AreAllMonstersDead())
        {
            Debug.Log("좀비 전원 처치 완료: 문 오픈");
            OpenDoor();
        }
    }

    public void SetZombies(List<GameObject> zombieList)
    {
        zombies = zombieList;
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