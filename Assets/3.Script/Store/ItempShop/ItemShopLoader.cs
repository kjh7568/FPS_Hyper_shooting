using UnityEngine;

public class ItemShopLoader : MonoBehaviour
{
    [SerializeField] private ItemShopPanelUI[] panelSlots; // 이미 있는 3개의 슬롯 할당

    private void Start()
    {
        LoadWeaponsIntoExistingPanels();
    }

    private void LoadWeaponsIntoExistingPanels()
    {
        WeaponDataSO[] allGuns = Resources.LoadAll<WeaponDataSO>("");
        int slotIndex = 0;

        foreach (var gun in allGuns)
        {
            if (!gun.name.StartsWith("Root_")) continue;
            if (gun.weaponType == WeaponType.Knife || gun.weaponType == WeaponType.Grenade) continue;

            if (slotIndex >= panelSlots.Length) break;

            panelSlots[slotIndex].SetGunData(gun);
            slotIndex++;
        }
    }
}