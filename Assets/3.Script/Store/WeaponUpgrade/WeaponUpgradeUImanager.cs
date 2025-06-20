using UnityEngine;

public class WeaponUpgradeUImanager : MonoBehaviour
{
    [Header("패널별 무기 로더")]
    public MyWeaponLoader primaryLoader;
    public MyWeaponLoader secondaryLoader;
    public MyWeaponLoader knifeLoader;
    public MyWeaponLoader grenadeLoader;

    private void OnEnable()
    {
        RefreshAllPanels();
    }
    private void RefreshAllPanels()
    {
        primaryLoader.LoadWeapon();
        secondaryLoader.LoadWeapon();
        knifeLoader.LoadWeapon();
        grenadeLoader.LoadWeapon();
    }
}