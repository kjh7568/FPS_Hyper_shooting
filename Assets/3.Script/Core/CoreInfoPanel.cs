using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CoreInfoPanel : MonoBehaviour
{
    public static CoreInfoPanel Instance;

    [Header("Header UI")]
    [SerializeField] private TMP_Text titleText;

    [Header("Level Detail Texts")]
    [SerializeField] private TMP_Text[] levelDetailTexts = new TMP_Text[5];

    private void Awake()
    {
        Instance = this;
    }

    public void DisplayCore(CoreDataSO coreData)
    {
        if (coreData == null)
        {
            Clear();
            return;
        }

        titleText.text = coreData.coreName;
        for (int i = 0; i < levelDetailTexts.Length; i++)
        {
            if (i < coreData.levelStats.Count)
            {
                levelDetailTexts[i].text = coreData.levelStats[i].detailDescription;
            }
            else
            {
                levelDetailTexts[i].text = "";
            }
        }
    }

    public void Clear()
    {
        titleText.text = "";
        foreach (var txt in levelDetailTexts)
        {
            txt.text = "";
        }
    }
}