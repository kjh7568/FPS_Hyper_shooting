using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager instance;

    public Transform characterModel;

    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController pistolController;
    [SerializeField] private RuntimeAnimatorController rifleController;

    public GameObject grenadePrefab;
    public Transform grenadeSpawnPoint;

    public WeaponController primaryWeapon;
    public WeaponController secondaryWeapon;
    public WeaponController knifeWeapon;
    public WeaponController grenadeWeapon;

    public WeaponController currentWeapon;
    public bool isPrimary = true;

    public AnimatorStateInfo stateInfo;

    private static readonly int STAB = Animator.StringToHash("Stab");
    private static readonly int THROW = Animator.StringToHash("Throw");

    public readonly Vector3 RifleStancePosition = new Vector3(0f, -1.5f, -0.07f);
    public readonly Quaternion RifleStanceRotation = new Quaternion(0f, 0f, 0f, 0f);

    public readonly Vector3 PistolStancePosition = new Vector3(0f, -1.6f, -0.07f);
    public readonly Quaternion PistolStanceRotation = Quaternion.Euler(0f, 20f, 0f);

    public readonly Vector3 KnifeStancePosition = new Vector3(0.42f, -0.99f, -0.55f);
    public readonly Quaternion KnifeStanceRotation = Quaternion.Euler(1.85f, -15.75f, 0f);

    public readonly Vector3 GrenadeStancePosition = new Vector3(0f, -1.5f, -0.3f);
    public readonly Quaternion GrenadeStanceRotation = Quaternion.Euler(0f, -15f, 0f);

    [Header("총기 모델")]
    [SerializeField] private GameObject akmModel;
    [SerializeField] private GameObject m4Model;
    [SerializeField] private GameObject umpModel;
    [SerializeField] private GameObject sniperModel;
    [SerializeField] private GameObject shotgunModel;
    [SerializeField] private GameObject pistolModel;
    [SerializeField] private GameObject knifeModel;

    public void ChangeWeapon(Weapon parameter)
    {
        WeaponController weaponToEquip;
        
        akmModel.SetActive(false);
        m4Model.SetActive(false);
        umpModel.SetActive(false);
        sniperModel.SetActive(false);
        shotgunModel.SetActive(false);
        
        switch (parameter.Type)
        {
            case WeaponType.Akm:
                akmModel.SetActive(true);
                
                weaponToEquip = akmModel.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                primaryWeapon = weaponToEquip; 
                break;
            case WeaponType.M4:
                m4Model.SetActive(true);
                
                weaponToEquip = m4Model.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                primaryWeapon = weaponToEquip; 
                break;
            case WeaponType.Sniper:
                sniperModel.SetActive(true);
                
                weaponToEquip = sniperModel.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                primaryWeapon = weaponToEquip; 
                break;
            case WeaponType.Shotgun:
                shotgunModel.SetActive(true);
                
                weaponToEquip = shotgunModel.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                primaryWeapon = weaponToEquip; 
                break;
            case WeaponType.Ump:
                umpModel.SetActive(true);
                
                weaponToEquip = umpModel.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                primaryWeapon = weaponToEquip; 
                break;
            case WeaponType.Pistol:
                weaponToEquip = pistolModel.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                secondaryWeapon = weaponToEquip; 
                break;
            case WeaponType.Knife:
                weaponToEquip = knifeModel.gameObject.GetComponent<WeaponController>();
                weaponToEquip.Init(parameter);
                
                knifeWeapon = weaponToEquip; 
                break;
            case WeaponType.Grenade:
                grenadeWeapon.Init(parameter);
                break;
        }
        
        currentWeapon = primaryWeapon;
    }
    
    public enum WeaponSlot
    {
        Primary,
        Secondary,
        Melee,
        Grenade
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(LoadWeaponData());
        
        currentWeapon = primaryWeapon;
    }

    private void Update()
    {
        stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipWeapon(WeaponSlot.Primary);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            EquipWeapon(WeaponSlot.Secondary);
        }
        else if (Input.GetKeyDown(KeyCode.F) && !stateInfo.IsName("Stab") && !stateInfo.IsName("Throw"))
        {
            characterModel.localPosition = KnifeStancePosition;
            characterModel.localRotation = KnifeStanceRotation;

            currentWeapon.gameObject.SetActive(false);
            knifeWeapon.gameObject.SetActive(true);

            animator.SetTrigger(STAB);
        }
        else if (Input.GetKeyDown(KeyCode.G) && !stateInfo.IsName("Stab") && !stateInfo.IsName("Throw"))
        {
            characterModel.localPosition = GrenadeStancePosition;
            characterModel.localRotation = GrenadeStanceRotation;

            currentWeapon.gameObject.SetActive(false);

            animator.SetTrigger(THROW);
        }
    }

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

                characterModel.localPosition = RifleStancePosition;
                characterModel.localRotation = RifleStanceRotation;

                    animator.SetLayerWeight(0, 1f);
                    animator.SetLayerWeight(1, 0);
                    animator.runtimeAnimatorController = rifleController;

                isPrimary = true;
                Debug.Log($"주무기 장착: {primaryWeapon.name}");
            }
            break;
            case WeaponSlot.Secondary:
                if (secondaryWeapon != null)
                {
                    secondaryWeapon.gameObject.SetActive(true);
                    currentWeapon = secondaryWeapon;

                    characterModel.localPosition = PistolStancePosition;
                    characterModel.localRotation = PistolStanceRotation;
                    animator.runtimeAnimatorController = pistolController;

                    animator.SetLayerWeight(0, 0);
                    animator.SetLayerWeight(1, 1f);

                isPrimary = false;
                Debug.Log($"보조무기 장착: {secondaryWeapon.name}");
            }
            break;
        case WeaponSlot.Melee:
            if (knifeWeapon != null)
            {
                knifeWeapon.gameObject.SetActive(true);
                currentWeapon = knifeWeapon;

                characterModel.localPosition = KnifeStancePosition;
                characterModel.localRotation = KnifeStanceRotation;

                Debug.Log($"근접무기 장착: {knifeWeapon.name}");
            }
            break;
        case WeaponSlot.Grenade:
            if (grenadeWeapon != null)
            {
                grenadeWeapon.gameObject.SetActive(true);
                currentWeapon = grenadeWeapon;

                characterModel.localPosition = GrenadeStancePosition;
                characterModel.localRotation = GrenadeStanceRotation;

                Debug.Log($"수류탄 장착: {grenadeWeapon.name}");
            }
            break;
    }
}
    public WeaponController GetWeaponBySlot(WeaponSlot slot)
    {
        switch (slot)
        {
            case WeaponSlot.Primary:
                return primaryWeapon;
            case WeaponSlot.Secondary:
                return secondaryWeapon;
            case WeaponSlot.Melee:
                return knifeWeapon;
            case WeaponSlot.Grenade:
                return grenadeWeapon;
            default:
                return null;
        }
    }

    private IEnumerator LoadWeaponData()
    {
        akmModel.SetActive(true);
        m4Model.SetActive(true);
        umpModel.SetActive(true);
        sniperModel.SetActive(true);
        shotgunModel.SetActive(true);
        pistolModel.SetActive(true);
        knifeModel.SetActive(true);
        
        yield return new WaitForSeconds(0.1f);
        
        akmModel.SetActive(false);
        m4Model.SetActive(false);
        umpModel.SetActive(false);
        sniperModel.SetActive(false);
        shotgunModel.SetActive(false);
        pistolModel.SetActive(false);
        knifeModel.SetActive(false);        
        
        primaryWeapon.gameObject.SetActive(true);
    }
    
    public void ApplyWeaponOption(Weapon parts)
    {
        foreach (var option in parts.options)
        {
            switch (option)
            {
                case WeaponSpecialEffect.DashCooldownReduction:
                    Player.localPlayer.inventory.EquipmentStat.dashCooldownReduction += 0.1f;
                    break;
                case WeaponSpecialEffect.ReloadSpeedReduction:
                    Player.localPlayer.inventory.EquipmentStat.reloadSpeedReduction += 0.1f;
                    break;
                case WeaponSpecialEffect.MultiplierAttackDamage:
                    Player.localPlayer.inventory.EquipmentStat.multiplierAttack += 0.05f;
                    break;
                case WeaponSpecialEffect.MultiplierMovementSpeed:
                    Player.localPlayer.inventory.EquipmentStat.multiplierMovementSpeed += 0.1f;
                    break;
                case WeaponSpecialEffect.IncreaseCriticalChance:
                    Player.localPlayer.inventory.EquipmentStat.criticalChance += 10;
                    break;
                case WeaponSpecialEffect.IncreaseCriticalDamage:
                    Player.localPlayer.inventory.EquipmentStat.multiplierCriticalDamage += 0.1f;
                    break;
                case WeaponSpecialEffect.IncreaseItemDropRate:
                    Player.localPlayer.inventory.EquipmentStat.multiplierRareItemChance += 0.1f;
                    break;
            }
        }
    }

    public void RemoveWeaponOption(Weapon parts)
    {
        foreach (var option in parts.options)
        {
            switch (option)
            {
                case WeaponSpecialEffect.DashCooldownReduction:
                    Player.localPlayer.inventory.EquipmentStat.dashCooldownReduction -= 0.1f;
                    break;
                case WeaponSpecialEffect.ReloadSpeedReduction:
                    Player.localPlayer.inventory.EquipmentStat.reloadSpeedReduction -= 0.1f;
                    break;
                case WeaponSpecialEffect.MultiplierAttackDamage:
                    Player.localPlayer.inventory.EquipmentStat.multiplierAttack -= 0.05f;
                    break;
                case WeaponSpecialEffect.MultiplierMovementSpeed:
                    Player.localPlayer.inventory.EquipmentStat.multiplierMovementSpeed -= 0.1f;
                    break;
                case WeaponSpecialEffect.IncreaseCriticalChance:
                    Player.localPlayer.inventory.EquipmentStat.criticalChance -= 10;
                    break;
                case WeaponSpecialEffect.IncreaseCriticalDamage:
                    Player.localPlayer.inventory.EquipmentStat.multiplierCriticalDamage -= 0.1f;
                    break;
                case WeaponSpecialEffect.IncreaseItemDropRate:
                    Player.localPlayer.inventory.EquipmentStat.multiplierRareItemChance -= 0.1f;
                    break;
            }
        }
    }
}