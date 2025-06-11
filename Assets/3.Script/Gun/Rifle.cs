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
    }

    protected override void Update()
    {
        base.Update(); // U 키 레벨업 유지

        if (isReloading)
            return;

        // 좌클릭 발사
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && !isOpenPanel)
        {
            if (currentAmmo > 0)
            {
                Fire();
                playerController.SetShootAnimation(true);
                nextFireTime = Time.time + currentStat.fireRate;
            }
            else
            {
                playerController.SetShootAnimation(false);
                
                Reload();
                playerController.SetReloadAnimation();
            }
        }

        // R 키 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
            playerController.SetReloadAnimation();
        }

        if (Input.GetMouseButtonUp(0))
        {
            playerController.SetShootAnimation(false);
        }
    }

    public override void Fire()
    {
        currentAmmo--;

        // Raycast 쏘기
        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f); // 디버그용 레이 시각화

            if (hit.collider.CompareTag("Zombie"))
            {
                var monster = CombatSystem.Instance.GetMonsterOrNull(hit.collider);

                if (monster != null)
                {
                    CombatEvent combatEvent = new CombatEvent();
                    combatEvent.Sender = Player.localPlayer;
                    combatEvent.Receiver = monster;
                    combatEvent.Damage = (int)currentStat.damage;
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
        if (isReloading)
        {
            return;
        }

        if (currentAmmo == gunData.maxAmmo)
        {
            return;
        }

        StartCoroutine(ReloadRoutine());
    }
    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        yield return new WaitForSeconds(currentStat.reloadTime);

        currentAmmo = gunData.maxAmmo;
        isReloading = false;
    }

}