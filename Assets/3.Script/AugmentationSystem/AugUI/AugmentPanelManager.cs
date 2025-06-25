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
    [SerializeField] private Text[] augmentButtonTexts;

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
            Shuffle(available);
            currentOptions = available.GetRange(0, Mathf.Min(3, available.Count));
        }

        for (int i = 0; i < 3; i++)
        {
            if (i >= currentOptions.Count)
            {
                augmentButtons[i].gameObject.SetActive(false); // 선택할 증강이 부족한 경우 버튼 비활성화
                continue;
            }

            AugmentData data = currentOptions[i];

            if (i < augmentButtonTexts.Length)
                augmentButtonTexts[i].text = ""; // 텍스트 제거

            augmentButtons[i].onClick.RemoveAllListeners();

            var augmentButton = augmentButtons[i].GetComponent<AugmentButton>();
            augmentButton.Initialize(data);

            if (hasSelected)
            {
                augmentButtons[i].interactable = false;

                if (i == selectedIndex)
                {
                    augmentButton.ShowSelectedOverlay(); // 선택 시 강조 이미지 표시
                }
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
                    RegisterObtainedAugment(data); // 선택된 증강 저장 + 디버그 출력

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

        var controller = FindObjectOfType<PlayerController>();
        if (controller != null) controller.isOpenPanel = true;
    }

    public void ClosePanel()
    {
        augmentPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        var controller = FindObjectOfType<PlayerController>();
        if (controller != null) controller.isOpenPanel = false;
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
}
