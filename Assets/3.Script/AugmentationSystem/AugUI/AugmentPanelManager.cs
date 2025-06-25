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

    private bool hasRerolled = false;
    public bool HasRerolled => hasRerolled;

    private bool hasSelected = false;
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
            currentOptions = new List<AugmentData>(allAugments);
            Shuffle(currentOptions);
            currentOptions = currentOptions.GetRange(0, 3);
        }

        for (int i = 0; i < 3; i++)
        {
            AugmentData data = currentOptions[i];

            if (i < augmentButtonTexts.Length)
                augmentButtonTexts[i].text = ""; // 텍스트 제거

            augmentButtons[i].onClick.RemoveAllListeners();

            var augmentButton = augmentButtons[i].GetComponent<AugmentButton>();
            augmentButton.Initialize(data);

            if (hasSelected)
            {
                augmentButtons[i].interactable = false;
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
        SetRandomAugments();
        augmentPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ClosePanel()
    {
        augmentPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
}
