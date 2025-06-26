using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CsvHelper;
using System.Globalization;
using System.IO;
using CsvHelper.Configuration;

public class AugmentPanelManager : MonoBehaviour
{
    public static AugmentPanelManager Instance;

    [SerializeField] private GameObject augmentPanel;
    [SerializeField] private Button[] augmentButtons;

    private List<AugmentData> allAugments;
    private List<AugmentData> currentOptions;
    private List<AugmentData> obtainedAugments = new List<AugmentData>();

    private bool hasRerolled = false;
    public bool HasRerolled => hasRerolled;

    private bool hasSelected = false;
    public bool HasSelected => hasSelected;

    private int selectedIndex = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        LoadAugmentsFromTSV();
    }

    private void Update()
    {
        if (augmentPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Space))
            {
                ClosePanel();
            }
        }
    }

    private void LoadAugmentsFromTSV()
    {
        TextAsset tsvFile = Resources.Load<TextAsset>("AugmentData/AugmentItemList");

        if (tsvFile == null)
        {
            Debug.LogError("TSV 파일을 찾을 수 없습니다.");
            return;
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = "\t"
        };

        using var reader = new StringReader(tsvFile.text);
        using var csv = new CsvReader(reader, config);

        allAugments = new List<AugmentData>(csv.GetRecords<AugmentData>());
    }

    private void SetRandomAugments()
    {
        if (currentOptions == null || currentOptions.Count == 0)
        {
            var available = allAugments.FindAll(a => !obtainedAugments.Contains(a));
            currentOptions = GetWeightedRandomAugments(available, Mathf.Min(3, available.Count));
        }

        for (int i = 0; i < 3; i++)
        {
            if (i >= currentOptions.Count)
            {
                augmentButtons[i].gameObject.SetActive(false);
                continue;
            }

            AugmentData data = currentOptions[i];
            augmentButtons[i].onClick.RemoveAllListeners();

            var augmentButton = augmentButtons[i].GetComponent<AugmentButton>();
            augmentButton.Initialize(data); // ← 등급 텍스트 포함 표시

            if (hasSelected)
            {
                augmentButtons[i].interactable = false;

                if (i == selectedIndex)
                    augmentButton.ShowSelectedOverlay();
            }
            else
            {
                int capturedIndex = i;
                augmentButtons[i].interactable = true;
                augmentButtons[i].onClick.AddListener(() =>
                {
                    selectedIndex = capturedIndex;
                    hasSelected = true;

                    GameData.Instance.augmentStat.Apply(data);
                    RegisterObtainedAugment(data);

                    ClosePanel();
                });
            }
        }
    }


    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int rand = Random.Range(i, list.Count);
            list[i] = list[rand];
            list[rand] = temp;
        }
    }

    public void OpenPanel()
    {
        // 남은 증강이 없다면 열지 않음
        if (obtainedAugments.Count >= allAugments.Count)
        {
            Debug.Log("모든 증강을 이미 선택했습니다.");
            return;
        }

        SetRandomAugments();
        augmentPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        FindObjectOfType<PlayerController>().isOpenPanel = true;
        WeaponManager.instance.currentWeapon.isOpenPanel = true;
    }

    public void ClosePanel()
    {
        augmentPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        FindObjectOfType<PlayerController>().isOpenPanel = false;
        WeaponManager.instance.currentWeapon.isOpenPanel = false;
      //  Cursor.lockState = CursorLockMode.Locked;
      //  Cursor.visible = false;

     //   var controller = FindObjectOfType<PlayerController>();
     //   if (controller != null) controller.isOpenPanel = false;
    }

    public void RerollAugments()
    {
        if (hasSelected)
        {
            Debug.Log("이미 선택해서 리롤할 수 없습니다.");
            return;
        }

        hasRerolled = true;
        currentOptions.Clear();
        SetRandomAugments();
    }

    public void RegisterObtainedAugment(AugmentData data)
    {
        if (!obtainedAugments.Contains(data))
        {
            obtainedAugments.Add(data);
        }

        // 디버그: 남은 증강 출력
        var remaining = allAugments.FindAll(a => !obtainedAugments.Contains(a));
        Debug.Log($"[증강] 남은 증강 개수: {remaining.Count}");
        foreach (var a in remaining)
        {
            Debug.Log($"[남은 증강] {a.Type} | Value: {a.Value}");
        }
    }
    private List<AugmentData> GetWeightedRandomAugments(List<AugmentData> candidates, int count)
    {
        List<AugmentData> result = new List<AugmentData>();
        List<AugmentData> pool = new List<AugmentData>();

        foreach (var data in candidates)
        {
            for (int i = 0; i < data.Weight; i++)
            {
                pool.Add(data);
            }
        }

        Shuffle(pool);

        foreach (var item in pool)
        {
            if (!result.Contains(item))
                result.Add(item);

            if (result.Count >= count)
                break;
        }

        return result;
    }

}
