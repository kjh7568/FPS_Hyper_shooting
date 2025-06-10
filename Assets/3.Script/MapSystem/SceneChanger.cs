using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    private bool canTrigger = false;

    private void Start()
    {
        // 씬 시작 후 1초 후에만 트리거 활성화
        Invoke(nameof(EnableTrigger), 1f);
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
            Debug.Log($"씬 전환: {nextSceneName}");
            StageManager.Instance.LoadNextStage();
        }
    }
}