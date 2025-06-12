using UnityEngine;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    [Header("무기 장착 위치")]
    [SerializeField] private Transform weaponHolder;

    [Header("무기 프리팹")]
    [SerializeField] private GameObject PrimaryPrefab;
    [SerializeField] private GameObject SecondaryPrefab;
    [SerializeField] private GameObject MeleeWeaponPrefab;

    private GameObject currentPrimary;
    private GameObject currentSecondary;
    private GameObject currentEquipped;
    private GameObject meleeWeapon;

    private enum WeaponSlot { Primary, Secondary }

    private WeaponSlot currentSlot;

    private bool isMeleeAttacking = false;
    private void Start()
    {
        SetPrimaryWeapon(PrimaryPrefab);
        SetSecondaryWeapon(SecondaryPrefab);
        SetMeleeWeapon(MeleeWeaponPrefab);

        EquipWeapon(WeaponSlot.Primary); // 시작 무기
    }

    private void Update()
    {
        // 무기 스왑
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(WeaponSlot.Primary);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponSlot.Secondary);
        }
        // F 눌렀을 때 근접 공격 시작
        if (Input.GetKeyDown(KeyCode.F))
            StartCoroutine(HandleMeleeAttack());
    }

    private IEnumerator HandleMeleeAttack()
    {
        if (isMeleeAttacking) yield break;
        isMeleeAttacking = true;

        // 1) 기존 총기 숨기기
        if (currentEquipped != null)
            currentEquipped.SetActive(false);

        // 2) 칼 보이기
        if (meleeWeapon != null)
            meleeWeapon.SetActive(true);

        // 3) 칼 공격 실행
        Knife knife = meleeWeapon.GetComponent<Knife>();
        if (knife != null)
        {
            knife.Fire();  // 레이 쏘기 + 데미지
        }

        // 4) 애니메이션 재생 (공용 로직)
        PlayerController pc = FindObjectOfType<PlayerController>();
        if (pc != null)
            pc.SetShootAnimation(true);

        // 5) 쿨다운 대기 (칼의 fireRate 기준)
        float cd = knife != null ? knife.GetFireRate() : 1f;
        yield return new WaitForSeconds(cd);

        // 6) 애니메이션 리셋
        if (pc != null)
            pc.SetShootAnimation(false);

        // 7) 칼 숨기기
        if (meleeWeapon != null)
            meleeWeapon.SetActive(false);

        // 8) 원래 총기 복귀
        switch (currentSlot)
        {
            case WeaponSlot.Primary:
                if (currentPrimary != null) currentPrimary.SetActive(true);
                currentEquipped = currentPrimary;
                break;
            case WeaponSlot.Secondary:
                if (currentSecondary != null) currentSecondary.SetActive(true);
                currentEquipped = currentSecondary;
                break;
        }

        isMeleeAttacking = false;
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

    public void SetMeleeWeapon(GameObject weaponPrefab)
    {
        if (meleeWeapon != null)
            Destroy(meleeWeapon);

        meleeWeapon = Instantiate(weaponPrefab, weaponHolder);
        meleeWeapon.SetActive(false); // 처음엔 안 보이게

        if (Player.localPlayer != null)
        {
            Gun knife = meleeWeapon.GetComponent<Gun>();
            Player.localPlayer.myMeleeWeapon = knife;
        }
    }

    private void EquipWeapon(WeaponSlot slot)
    {
        if (meleeWeapon != null)
            meleeWeapon.SetActive(false); // 근접무기 꺼두기

        if (currentEquipped != null)
            currentEquipped.SetActive(false);

        switch (slot)
        {
            case WeaponSlot.Primary:
                if (currentPrimary != null)
                {
                    currentPrimary.SetActive(true);
                    currentEquipped = currentPrimary;
                    currentSlot = WeaponSlot.Primary;
                    Debug.Log($"주무기 장착: {currentPrimary.name}");
                }
                break;

            case WeaponSlot.Secondary:
                if (currentSecondary != null)
                {
                    currentSecondary.SetActive(true);
                    currentEquipped = currentSecondary;
                    currentSlot = WeaponSlot.Secondary;
                    Debug.Log($"보조무기 장착: {currentSecondary.name}");
                }
                break;
        }

        if (Player.localPlayer != null)
        {
            Gun gun = currentEquipped.GetComponent<Gun>();
            Player.localPlayer.myGun = gun;
        }
    }
}
