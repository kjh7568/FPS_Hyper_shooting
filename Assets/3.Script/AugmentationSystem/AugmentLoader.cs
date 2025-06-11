using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AugmentLoader : MonoBehaviour
{
    public static List<AugmentData> LoadAugmentsFromTsv(string fileName)
    {
        List<AugmentData> list = new List<AugmentData>();
        TextAsset tsvFile = Resources.Load<TextAsset>(fileName);

        if (tsvFile == null)
        {
            Debug.LogError($"TSV 파일을 찾을 수 없습니다: {fileName}");
            return list;
        }

        string[] lines = tsvFile.text.Split('\n');

        for (int i = 1; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] parts = lines[i].Trim().Split('\t');
            int id = int.Parse(parts[0]);
            AugmentType type = (AugmentType)System.Enum.Parse(typeof(AugmentType), parts[1]);
            float value = float.Parse(parts[2]);
            string description = parts[3];

            list.Add(new AugmentData(id, type, value, description));
        }

        return list;
    }

}
