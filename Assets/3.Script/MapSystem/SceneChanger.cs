using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private int nextStageIndex = -1;
    private bool canTrigger = false;

    private void Start()
    {
        Invoke(nameof(EnableTrigger), 1f);  // 트리거 활성화 지연
    }

    private void EnableTrigger()
    {
        canTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canTrigger) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log($"씬 전환 요청: 인덱스 {nextStageIndex}");
            StageManager.Instance.LoadSceneByIndex(nextStageIndex);
        }
    }
}