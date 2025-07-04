using System.Collections;
using UnityEngine;
using TMPro;

public class DamageUI : MonoBehaviour
{
    private float floatDuration = 1f; // 몇 초에 걸쳐 올라갈지
    private float floatHeight = 30f; // 몇 픽셀 위로 올라갈지
    [SerializeField] private float screenOffsetRadius = 300f; // 픽셀 단위 반경

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

   // void Update()
   // {
   //     if (worldParent == null) return;

   //     Vector3 screenPos = Camera.main.WorldToScreenPoint(worldParent.position);

   //     RectTransformUtility.ScreenPointToLocalPointInRectangle(
   //         rectTransform.parent as RectTransform,
   //         screenPos,
   //         Camera.main,
   //         out var anchoredPos
   //     );

   //     if (rectTransform != null)
   //         rectTransform.anchoredPosition = anchoredPos;
   // }

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
        Destroy(gameObject); // 애니메이션 종료 후 제거
    }

    public void Set(Transform worldParent, Vector3 worldPosition, float damage, bool isCritical)
    {
        this.worldParent = worldParent;

        text.text = Mathf.RoundToInt(damage).ToString();
        text.color = isCritical ? Color.yellow : Color.white;

        // 1) 월드 → 스크린 좌표 변환
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPosition);

        // 2) 스크린 좌표계 내에서 랜덤 오프셋 추가
        Vector2 randomOffset = Random.insideUnitCircle * screenOffsetRadius;
        screenPos.x += randomOffset.x;
        screenPos.y += randomOffset.y;

        // 3) 로컬 UI 좌표로 변환
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform.parent as RectTransform,
            screenPos,
            uiCamera,
            out var anchoredPos
        );

        rectTransform.anchoredPosition = anchoredPos;
        StartCoroutine(FloatUp());
    }
}