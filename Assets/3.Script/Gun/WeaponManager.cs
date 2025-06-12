using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private Transform characterModel;
    
    [SerializeField] private Animator animator;
    [SerializeField] private RuntimeAnimatorController pistolController;
    [SerializeField] private RuntimeAnimatorController rifleController;
    
    public Gun primaryWeapon;
    public Gun secondaryWeapon;
    public static Gun currentWeapon;

    private readonly Vector3 rifleStancePosition = new Vector3(0f, -1.5f, -0.07f);
    private readonly Quaternion rifleStanceRotation = new Quaternion(0f, 0f, 0f, 0f);
    
    private readonly Vector3 pistolStancePosition = new Vector3(0f, -1.6f, -0.07f);
    private readonly Quaternion pistolStanceRotation = Quaternion.Euler(0f, 20f, 0f);
    
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
        else if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("칼질");
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
                    
                    characterModel.localPosition = rifleStancePosition;
                    characterModel.localRotation = rifleStanceRotation;

                    animator.SetLayerWeight(0, 1f);
                    animator.SetLayerWeight(1, 0);
                    animator.runtimeAnimatorController = rifleController;
                    
                    Debug.Log($"주무기 장착: {primaryWeapon.name}");
                }
                break;

            case WeaponSlot.Secondary:
                if (secondaryWeapon != null)
                {
                    secondaryWeapon.gameObject.SetActive(true);
                    currentWeapon = secondaryWeapon;
                    
                    characterModel.localPosition = pistolStancePosition;
                    characterModel.localRotation = pistolStanceRotation;
                    animator.runtimeAnimatorController = pistolController;
                    
                    animator.SetLayerWeight(0, 0);
                    animator.SetLayerWeight(1, 1f);

                    Debug.Log($"보조무기 장착: {secondaryWeapon.name}");
                }
                break;
        }
    }
}
