using UnityEngine;

public class SceneChanger : MonoBehaviour
{
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
            Debug.Log($"씬 전환 요청됨 (자동 시퀀스)");
            StageManager.Instance.LoadNextScene();
        }
    }
}