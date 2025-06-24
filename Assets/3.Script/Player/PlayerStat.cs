using System;
using UnityEngine;

[Serializable]
public class PlayerStat
{
    public float health;
    public float maxHealth;    // 기본 최대 체력
    public float moveSpeed;
    public float dashCoolTime;
    public float pickupRadius;
}