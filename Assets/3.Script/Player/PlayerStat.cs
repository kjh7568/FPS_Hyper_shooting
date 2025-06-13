using System;


[Serializable]
public class PlayerStat
{
    public float health;
    public float maxHealth;
    public float moveSpeed;
    public float dashCoolTime;
    public float currentDefense;

    public void CalculateDefense(Inventory inventory)
    {
        currentDefense = inventory.GetTotalDefense();
    }
}