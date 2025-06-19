using UnityEngine;

public class WeaponUpgradeUImanager : MonoBehaviour
{
    [Header("패널별 무기 로더")]
    public CurrentWeaponLoader primaryLoader;
    public CurrentWeaponLoader secondaryLoader;
    public CurrentWeaponLoader knifeLoader;
    public CurrentWeaponLoader grenadeLoader;

    private void OnEnable()
    {
        RefreshAllPanels();
    }

    private void RefreshAllPanels()
    {
        var wm = WeaponManager.instance;

        primaryLoader.LoadWeapon(wm.primaryWeapon.weapon);
        secondaryLoader.LoadWeapon(wm.secondaryWeapon.weapon);
        knifeLoader.LoadWeapon(wm.knifeWeapon.weapon);
        grenadeLoader.LoadWeapon(wm.grenadeWeapon.weapon);
    }
}