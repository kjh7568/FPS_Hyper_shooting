using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterUIManager : MonoBehaviour
{
    public static MonsterUIManager instance;

    [SerializeField] private Camera mainCamera; // 카메라 할당 예정
    [SerializeField] private GameObject hpBarPrefab;
    [SerializeField] private Transform canvasRoot; // MonsterUICanvas

    private void Awake()
    {
        instance = this;
    }

    public void Init(Camera cam)
    {
        mainCamera = cam;
        canvasRoot.GetComponent<Canvas>().worldCamera = mainCamera;
    }

    public GameObject CreateHPBar(NormalZombie target)
    {
        GameObject hpBar = Instantiate(hpBarPrefab, canvasRoot);
        hpBar.GetComponent<MonsterHPUI>().Init(target, mainCamera);
        return hpBar;
    }
}