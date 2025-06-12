using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [Header("무기 장착 위치")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GameObject PrimaryPrefab;
    [SerializeField] private GameObject SecondaryPrefab;

    private GameObject currentPrimary;
    private GameObject currentSecondary;
    private GameObject currentEquipped;

    private enum WeaponSlot { Primary, Secondary }

    private void Start()
    {
        SetPrimaryWeapon(PrimaryPrefab);
        SetSecondaryWeapon(SecondaryPrefab);
        EquipWeapon(WeaponSlot.Primary);
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

    public void SetPrimaryWeapon(GameObject weaponPrefab)
    {
        if (currentPrimary != null)
            Destroy(currentPrimary);

        currentPrimary = Instantiate(weaponPrefab, weaponHolder);
        currentPrimary.SetActive(false);
    }

    public void SetSecondaryWeapon(GameObject weaponPrefab)
    {
        if (currentSecondary != null)
            Destroy(currentSecondary);

        currentSecondary = Instantiate(weaponPrefab, weaponHolder);
        currentSecondary.SetActive(false);
    }

    private void EquipWeapon(WeaponSlot slot)
    {
        if (currentEquipped != null)
            currentEquipped.SetActive(false);

        switch (slot)
        {
            case WeaponSlot.Primary:
                if (currentPrimary != null)
                {
                    currentPrimary.SetActive(true);
                    currentEquipped = currentPrimary;
                    Debug.Log($"주무기 장착: {currentPrimary.name}");
                }
                break;

            case WeaponSlot.Secondary:
                if (currentSecondary != null)
                {
                    currentSecondary.SetActive(true);
                    currentEquipped = currentSecondary;
                    Debug.Log($"보조무기 장착: {currentSecondary.name}");
                }
                break;
        }

       //  // 추가: 플레이어에게 현재 무기 알려주기
       //  if (Player.localPlayer != null)
       //  {
       //      var gun = currentEquipped.GetComponent<Gun>();
       //      Player.localPlayer.myGun = gun;
       //  }
    }

}
