using UnityEngine;

public class MasteryPanelManager : MonoBehaviour
{
    public static MasteryPanelManager instance;

    [Header("Mastery Panel Root")]
    [SerializeField] private GameObject masteryPanel;

    [Header("탭 별 패널")]
    [SerializeField] private GameObject defenseTab;
    [SerializeField] private GameObject damageTab;
    [SerializeField] private GameObject utilityTab;

    [Header("탭 버튼 영역")]
    [SerializeField] private GameObject tabButtons;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        masteryPanel.SetActive(false);
        defenseTab.SetActive(false);
        damageTab.SetActive(false);
        utilityTab.SetActive(false);
        tabButtons.SetActive(false);
    }

    private void Update()
    {
        if (masteryPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePanel();
        }
    }

    public void OpenPanel()
    {
        masteryPanel.SetActive(true);
        tabButtons.SetActive(true);
        SwitchToDefense(); // 기본 탭 설정

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        FindObjectOfType<PlayerController>().isOpenPanel = true;
        WeaponManager.instance.currentWeapon.isOpenPanel = true;
    }

    public void ClosePanel()
    {
        masteryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        FindObjectOfType<PlayerController>().isOpenPanel = false;
        WeaponManager.instance.currentWeapon.isOpenPanel = false;
    }

    public void SwitchToDefense()
    {
        defenseTab.SetActive(true);
        damageTab.SetActive(false);
        utilityTab.SetActive(false);
    }

    public void SwitchToDamage()
    {
        defenseTab.SetActive(false);
        damageTab.SetActive(true);
        utilityTab.SetActive(false);
    }

    public void SwitchToUtility()
    {
        defenseTab.SetActive(false);
        damageTab.SetActive(false);
        utilityTab.SetActive(true);
    }
}
