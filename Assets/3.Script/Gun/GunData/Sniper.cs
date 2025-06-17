using System.Collections;
using UnityEngine;

public class Sniper : Gun
{
    private float nextFireTime = 0f;
    private PlayerController playerController;
    private Camera mainCam;

    private float originalFOV;
    private float zoomFOV = 15f;
    private bool isZooming = false;

    private float originalSensitivity;
    private float zoomSensitivity = 50f;
    [SerializeField] private GameObject SniperUI;
    [SerializeField] private GameObject CrosshairUI;
    


    private void OnEnable()
    {
        playerController = FindObjectOfType<PlayerController>();
        mainCam = Camera.main;
        isReloading = false;

        if (mainCam != null)
        {
            originalFOV = mainCam.fieldOfView;
        }

        originalSensitivity = playerController.MouseSensitivity;
    }

    protected override void Update()
    {
        if (isReloading)
            return;

        // 발사
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && !isOpenPanel && !WeaponManager.instance.stateInfo.IsName("Draw"))
        {
            if (currentAmmo > 0)
            {
                Fire();
                playerController.SetShootAnimation(true);
                StartCoroutine(ResetShootBool());
                nextFireTime = Time.time + currentStat.fireRate;
            }
            else
            {
                Reload();
            }
        }

        // 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("[Sniper] R키 눌림");
            Reload();
        }

        // 발사 애니메이션 정지
        if (Input.GetMouseButtonUp(0))
        {
            playerController.SetShootAnimation(false);
        }

        // 스코프 줌
        if (Input.GetMouseButtonDown(1))
        {
            ZoomIn();
        }
        if (Input.GetMouseButtonUp(1))
        {
            ZoomOut();
        }

        // 테스트용 레벨업
        if (Input.GetKeyDown(KeyCode.U))
        {
            base.LevelUp();
        }
    }

    public override void Fire()
    {
        currentAmmo--;

        float damage = GetFinalDamage();

        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 1f);

            if (hit.collider.CompareTag("Zombie"))
            {
                var monster = CombatSystem.Instance.GetMonsterOrNull(hit.collider);

                if (monster != null)
                {
                    CombatEvent combatEvent = new CombatEvent();
                    combatEvent.Sender = Player.localPlayer;
                    combatEvent.Receiver = monster;
                    combatEvent.Damage = Mathf.RoundToInt(damage);
                    combatEvent.HitPosition = hit.point;
                    combatEvent.Collider = hit.collider;

                    CombatSystem.Instance.AddInGameEvent(combatEvent);
                }
            }
            else
            {
                Debug.Log($"적이 아님: {hit.collider.gameObject.name}");
            }
        }
        else
        {
            Debug.Log("무언가를 맞추지 못함.");
        }
    }

    public override void Reload()
    {
        if (CurrentAmmo >= gunData.maxAmmo)
        {
            Debug.Log("[Sniper] 이미 탄창이 가득 차 있습니다.");
            return;
        }

        playerController.SetShootAnimation(false);
        playerController.SetReloadAnimation();
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(currentStat.reloadTime);

        currentAmmo = gunData.maxAmmo;
        isReloading = false;

        OnReloadComplete();
    }

    private void ZoomIn()
    {
        isZooming = true;
        mainCam.fieldOfView = zoomFOV;
        playerController.MouseSensitivity = zoomSensitivity;
        SniperUI.SetActive(true);
        CrosshairUI.SetActive(false);
    }

    private void ZoomOut()
    {
        isZooming = false;
        mainCam.fieldOfView = originalFOV;
        playerController.MouseSensitivity = originalSensitivity;
        SniperUI.SetActive(false); // 스코프 UI 끄기
        CrosshairUI.SetActive(true);
    }

    private IEnumerator ResetShootBool()
    {
        yield return null;                        
        playerController.SetShootAnimation(false);
    }
}
