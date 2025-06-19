using UnityEngine;

public class WeaponUpgradeUImanager : MonoBehaviour
{
    [Header("패널별 무기 로더")]
    public MyWeaponLoader primaryLoader;
    public MyWeaponLoader secondaryLoader;
    public MyWeaponLoader knifeLoader;
    public MyWeaponLoader grenadeLoader;

    private void Start()
    {
        RefreshAllPanels();
    }
    private void RefreshAllPanels()
    {
        var wm = WeaponManager.instance;

        primaryLoader.LoadWeapon();
        secondaryLoader.LoadWeapon();
        knifeLoader.LoadWeapon();
        grenadeLoader.LoadWeapon();
    }
}