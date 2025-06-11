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
    [SerializeField] private Button[] augmentButtons;        // 버튼 3개
    [SerializeField] private Text[] augmentButtonTexts;      // 각 버튼 설명 텍스트
    private List<AugmentData> allAugments;                   // TSV에서 불러온 전체 리스트
    private List<AugmentData> currentOptions;                // 현재 선택지 3개

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        LoadAugmentsFromTSV(); // 시작 시 1회 로딩
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
        currentOptions = new List<AugmentData>(allAugments);
        Shuffle(currentOptions);

        for (int i = 0; i < 3; i++)
        {
            AugmentData data = currentOptions[i];

            // 1. 텍스트 표시
            augmentButtonTexts[i].text = $"{data.Type}\n<size=12>{data.Description}</size>";

            // 2. 버튼 바인딩 초기화
            augmentButtons[i].onClick.RemoveAllListeners();

            // 3. 증강 데이터 바인딩
            AugmentButton augmentButton = augmentButtons[i].GetComponent<AugmentButton>();
            augmentButton.Initialize(data);

            // 4. OnClick은 AugmentButton 내부 메서드로 연결
            augmentButtons[i].onClick.AddListener(() => augmentButton.OnClick());
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
}
