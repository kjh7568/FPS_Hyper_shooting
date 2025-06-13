using System;
using UnityEngine;
using System.Collections;

public class Rifle : Gun
{
    private float nextFireTime = 0f;
    private PlayerController playerController;

    private void OnEnable()
    {
        playerController = FindObjectOfType<PlayerController>();
        isReloading = false;
    }

    protected override void Update()
    {
        if (isReloading)
            return;
        
        // 좌클릭 발사
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && !isOpenPanel && !WeaponManager.instance.stateInfo.IsName("Draw"))
        {
            if (currentAmmo > 0)
            {
                Fire();
                playerController.SetShootAnimation(true);
                nextFireTime = Time.time + currentStat.fireRate;
            }
            else
            {
                Reload();
            }
        }

        // R 키 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("R키 눌림");
            Reload();
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerController.SetShootAnimation(false);
        }

        if (Input.GetKeyDown(KeyCode.U))
        {
            base.LevelUp();
        }
    }

    public override void Fire()
    {
        currentAmmo--;

        float damage = GetFinalDamage(); // ✅ 버프 포함된 데미지 계산

        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);

            if (hit.collider.CompareTag("Zombie"))
            {
                var monster = CombatSystem.Instance.GetMonsterOrNull(hit.collider);

                if (monster != null)
                {
                    CombatEvent combatEvent = new CombatEvent();
                    combatEvent.Sender = Player.localPlayer;
                    combatEvent.Receiver = monster;
                    combatEvent.Damage = Mathf.RoundToInt(damage); // ✅ 버프 적용된 최종 데미지 사용
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
                 Debug.Log("[Rifle] 이미 탄창이 가득 차 있습니다.");
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
}