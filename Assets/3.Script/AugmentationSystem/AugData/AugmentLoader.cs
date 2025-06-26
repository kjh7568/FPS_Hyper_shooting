using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
            Delimiter = "\t",
            PrepareHeaderForMatch = args => args.Header.Trim(),
            IgnoreBlankLines = true,
            MissingFieldFound = null,
            BadDataFound = null,
            TrimOptions = TrimOptions.Trim
        };

        try
        {
            using var reader = new StringReader(tsvFile.text);
            using var csv = new CsvReader(reader, config);

            // 💡 대소문자 구분 없이 Enum 매핑 가능하게 설정
            csv.Context.TypeConverterOptionsCache
                .GetOptions<AugmentGrade>().EnumIgnoreCase = true;

            // 💡 AugmentType enum 도 대비
            csv.Context.TypeConverterOptionsCache
                .GetOptions<AugmentType>().EnumIgnoreCase = true;

            var records = new List<AugmentData>(csv.GetRecords<AugmentData>());
            return records;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"TSV 파싱 중 오류 발생: {ex.Message}");
            return new List<AugmentData>();
        }
    }
}