using UnityEngine;

public class StorePanelManager : MonoBehaviour
{
    public static StorePanelManager Instance;

    [Header("Store Panel Root")]
    [SerializeField] private GameObject storePanel;

    [Header("탭 별 패널")]
    [SerializeField] private GameObject itemShop;
    [SerializeField] private GameObject weaponUpgrade;
    [SerializeField] private GameObject armorUpgrade;

    [Header("탭 버튼 영역")]
    [SerializeField] private GameObject storeSystem;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        storePanel.SetActive(false);
        itemShop.SetActive(false);
        weaponUpgrade.SetActive(false);
        armorUpgrade.SetActive(false);
        storeSystem.SetActive(false);
    }

    
    private void Update()
    {
        if (storePanel.activeSelf && Input.GetKeyDown(KeyCode.Space))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        storePanel.SetActive(true);
        itemShop.SetActive(true);
        weaponUpgrade.SetActive(false);
        armorUpgrade.SetActive(false);
        storeSystem.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FindObjectOfType<PlayerController>().isOpenPanel = true;
        WeaponManager.instance.currentWeapon.isOpenPanel = true;
        // FindObjectOfType<UIManager>().isOpenPanel = false;
        // 상점열었을때 PlayerUI 끄다 실패
    }

    public void ClosePanel()
    {
        storePanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<PlayerController>().isOpenPanel = false;
        WeaponManager.instance.currentWeapon.isOpenPanel = false;
        // FindObjectOfType<UIManager>().isOpenPanel = true;
        // 상점열었을때 PlayerUI 끄다 실패
    }
    public void SwitchToItemShop()
    {
        itemShop.SetActive(true);
        weaponUpgrade.SetActive(false);
        armorUpgrade.SetActive(false);
    }

    public void SwitchToWeaponUpgrade()
    {
        itemShop.SetActive(false);
        weaponUpgrade.SetActive(true);
        armorUpgrade.SetActive(false);
    }

    public void SwitchToArmorUpgrade()
    {
        itemShop.SetActive(false);
        weaponUpgrade.SetActive(false);
        armorUpgrade.SetActive(true);
    }
}
