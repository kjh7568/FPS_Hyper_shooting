using UnityEngine;

public class NextStageController : MonoBehaviour
{
    [Header("문 오브젝트")]
    [SerializeField] private GameObject door;

    [Header("좀비 오브젝트들")]
    [SerializeField] private GameObject[] zombies;

    private bool isDoorOpened = false;

    private void Update()
    {
        if (isDoorOpened) return;

        // 디버그용 수동 키: C를 누르면 문 열림
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C 키 입력: 문 강제 오픈");
            OpenDoor();
        }
        if (AllZombiesDefeated())
        {
            Debug.Log("좀비 전원 처치 완료: 문 오픈");
            OpenDoor();
        }
    }

    private bool AllZombiesDefeated()
    {
        foreach (GameObject zombie in zombies)
        {
            if (zombie != null)
                return false;
        }
        return true;
    }

    private void OpenDoor()
    {
        isDoorOpened = true;

        if (door != null)
        {
            door.SetActive(false);
            Debug.Log("문이 열렸습니다.");
        }
    }
}