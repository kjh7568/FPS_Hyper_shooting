using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropItemUIFiled : MonoBehaviour
{
    public TMP_Text targetLevelText;
    public TMP_Text targetNameText;
    public TMP_Text targetTierText;
    public Image targetItemImage;
    
    public GameObject targetArmorInfoPanel;
    public TMP_Text targetArmorValueText;
    
    public GameObject targetGunInfoPanel;
    public TMP_Text targetGunDamageValueText;
    public TMP_Text targetGunFireRateValueText;
    public TMP_Text targetGunMagazineSizeValueText;
    public TMP_Text targetGunReloadTimeValueText;
    
    public GameObject targetKnifeInfoPanel;
    public TMP_Text targetKnifeDamageValueText;
    public TMP_Text targetKnifeFireRateValueText;
    
    public GameObject targetGrenadeInfoPanel;
    public TMP_Text targetGrenadeDamageValueText;
    public TMP_Text targetGrenadeExplosionRangeValueText;
    
    public TMP_Text[] targetDescriptionTexts;
}
