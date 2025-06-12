using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Gun primaryWeapon;
    public Gun secondaryWeapon;
    public static Gun currentWeapon;

    private enum WeaponSlot { Primary, Secondary }

    private void Start()
    {
        primaryWeapon.gameObject.SetActive(true);
        currentWeapon = primaryWeapon;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(WeaponSlot.Primary);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponSlot.Secondary);
        }
    }

    // public void SetPrimaryWeapon(GameObject weaponPrefab)
    // {
    //     if (currentPrimary != null)
    //         Destroy(currentPrimary);
    //
    //     currentPrimary = Instantiate(weaponPrefab, weaponHolder);
    //     currentPrimary.SetActive(false);
    // }
    //
    // public void SetSecondaryWeapon(GameObject weaponPrefab)
    // {
    //     if (currentSecondary != null)
    //         Destroy(currentSecondary);
    //
    //     currentSecondary = Instantiate(weaponPrefab, weaponHolder);
    //     currentSecondary.SetActive(false);
    // }

    private void EquipWeapon(WeaponSlot slot)
    {
        if (currentWeapon != null)
            currentWeapon.gameObject.SetActive(false);

        switch (slot)
        {
            case WeaponSlot.Primary:
                if (primaryWeapon != null)
                {
                    primaryWeapon.gameObject.SetActive(true);
                    currentWeapon = primaryWeapon;
                    Debug.Log($"주무기 장착: {primaryWeapon.name}");
                }
                break;

            case WeaponSlot.Secondary:
                if (secondaryWeapon != null)
                {
                    secondaryWeapon.gameObject.SetActive(true);
                    currentWeapon = secondaryWeapon;
                    Debug.Log($"보조무기 장착: {secondaryWeapon.name}");
                }
                break;
        }

       // // 추가: 플레이어에게 현재 무기 알려주기
       // if (Player.localPlayer != null)
       // {
       //     var gun = currentWeapon.GetComponent<Gun>();
       //     Player.localPlayer.currentGun = gun;
       // }
    }

}
