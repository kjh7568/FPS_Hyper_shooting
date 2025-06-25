using System.Collections;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    private float floatDuration = 1f;     // 몇 초에 걸쳐 올라갈지
    private float floatHeight = 30f;      // 몇 픽셀 위로 올라갈지

    private RectTransform rectTransform;
    private TMP_Text text;
    private Camera uiCamera;    
    private Transform worldParent;

    private void Awake()
    {
        uiCamera = GameObject.FindGameObjectWithTag("UICamera")?.GetComponent<Camera>();
        
        rectTransform = GetComponent<RectTransform>();
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        //StartCoroutine(FloatUp());
    }

    void Update()
    {
        if(worldParent == null) return;
        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldParent.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            screenPos,
            Camera.main,
            out var anchoredPos
        );
        
        if (rectTransform != null)
            rectTransform.anchoredPosition = anchoredPos;
    }
    
    private IEnumerator FloatUp()
    {
        Vector2 start = rectTransform.anchoredPosition;
        Vector2 end = start + Vector2.up * floatHeight;

        float elapsed = 0f;
        Color originalColor = text.color;

        while (elapsed < floatDuration)
        {
            float t = elapsed / floatDuration;

            // 위치 이동
            rectTransform.anchoredPosition = Vector2.Lerp(start, end, t);

            // 알파값 줄이기
            Color c = originalColor;
            c.a = 1f - t;
            text.color = c;

            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    
    public void Set(Transform worldParent, Vector3 worldPosition, float damage)
    {
        this.worldParent = worldParent;
        
        if (text != null)
            text.text = Mathf.RoundToInt(damage).ToString();

        // 월드 → 스크린 → 로컬 UI 좌표 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            screenPos,
            uiCamera,
            out var anchoredPos
        );
        
        if (rectTransform != null)
            rectTransform.anchoredPosition = anchoredPos;
    }

}