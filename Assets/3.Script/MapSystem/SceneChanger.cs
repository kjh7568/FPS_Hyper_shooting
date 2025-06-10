using UnityEngine;

public class SceneChanger : MonoBehaviour
{
    [Header("전환할 씬 이름")]
    [SerializeField] private string nextSceneName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"씬 전환: {nextSceneName}");
            StageManager.Instance.LoadScene(nextSceneName);
        }
    }
}