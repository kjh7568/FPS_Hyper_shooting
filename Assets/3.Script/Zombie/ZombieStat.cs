using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ZombieStat
{
    public string name;
    public float maxHealth;
    public float health
    {
        get => maxHealth;
        set => maxHealth = value;
    }

    public float damage;
    public float moveSpeed;
    public bool isCanRun;
}
