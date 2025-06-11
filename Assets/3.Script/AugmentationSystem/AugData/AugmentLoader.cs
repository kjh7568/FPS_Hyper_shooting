using System.Collections.Generic;
using System.Globalization;
using System.IO;
using CsvHelper;
using CsvHelper.Configuration;
using UnityEngine;

public static class AugmentLoader
{
    public static List<AugmentData> LoadAugmentsFromTSV(string resourcePath)
    {
        TextAsset tsvFile = Resources.Load<TextAsset>(resourcePath);

        if (tsvFile == null)
        {
            Debug.LogError($"TSV 파일을 찾을 수 없습니다: {resourcePath}");
            return new List<AugmentData>();
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            Delimiter = "\t"
        };

        using var reader = new StringReader(tsvFile.text);
        using var csv = new CsvReader(reader, config);

        try
        {
            return new List<AugmentData>(csv.GetRecords<AugmentData>());
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"TSV 파싱 중 오류 발생: {ex.Message}");
            return new List<AugmentData>();
        }
    }
}