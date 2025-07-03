using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMapManager : MonoBehaviour
{
    [SerializeField] private GameObject mapManager;
    
    private void Start()
    {
        mapManager.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        mapManager.SetActive(true);
    }
}
