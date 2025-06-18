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
        GunDataSO[] allGuns = Resources.LoadAll<GunDataSO>("");
        int slotIndex = 0;

        foreach (var gun in allGuns)
        {
            if (!gun.name.StartsWith("Root_")) continue;
            if (gun.gunType == GunType.Knife || gun.gunType == GunType.Grenade) continue;

            if (slotIndex >= panelSlots.Length) break;

            panelSlots[slotIndex].SetGunData(gun);
            slotIndex++;
        }
    }
}