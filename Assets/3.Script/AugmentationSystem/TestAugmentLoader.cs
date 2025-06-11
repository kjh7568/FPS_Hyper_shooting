using System.Collections.Generic;
using UnityEngine;

public class TestAugmentLoader : MonoBehaviour
{
    void Start()
    {
        List<AugmentData> augmentList = AugmentLoader.LoadAugmentsFromTsv("AugmentData/AugmentItemList");

        foreach (var augment in augmentList)
        {
            Debug.Log($"[ID: {augment.id}] Type: {augment.type}, Value: {augment.value}, Desc: {augment.description}");
        }
    }
}