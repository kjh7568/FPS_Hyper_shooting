using System;
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

    public void ChangeWeapon(WeaponType type)
    {
        akmModel.SetActive(false);
        m4Model.SetActive(false);
        umpModel.SetActive(false);
        sniperModel.SetActive(false);
        shotgunModel.SetActive(false);
        
        switch (type)
        {
            case WeaponType.Akm:
                akmModel.SetActive(true);
                primaryWeapon = akmModel.gameObject.GetComponent<WeaponController>(); 
                currentWeapon = primaryWeapon;
                break;
            case WeaponType.M4:
                m4Model.SetActive(true);
                primaryWeapon = m4Model.gameObject.GetComponent<WeaponController>(); 
                currentWeapon = primaryWeapon;
                break;
            case WeaponType.Sniper:
                sniperModel.SetActive(true);
                primaryWeapon = sniperModel.gameObject.GetComponent<WeaponController>(); 
                currentWeapon = primaryWeapon;
                break;
            case WeaponType.Shotgun:
                shotgunModel.SetActive(true);
                primaryWeapon = shotgunModel.gameObject.GetComponent<WeaponController>(); 
                currentWeapon = primaryWeapon;
                break;
            case WeaponType.Ump:
                umpModel.SetActive(true);
                primaryWeapon = umpModel.gameObject.GetComponent<WeaponController>(); 
                currentWeapon = primaryWeapon;
                break;
            case WeaponType.Pistol:
                break;
            case WeaponType.Grenade:
                break;
            case WeaponType.Knife:
                break;
        }
        
        currentWeapon = primaryWeapon;
    }

    
    private enum WeaponSlot
    {
        Primary,
        Secondary
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        primaryWeapon.gameObject.SetActive(true);
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
        }
    }
    
}