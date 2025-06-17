using UnityEngine;
using System.Collections;

public class Shotgun : Gun
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
                playerController.SetShootAnimation(false);
                
                Reload();
                playerController.SetReloadAnimation();
            }
        }

        // R 키 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
          
        }
        if (Input.GetMouseButtonUp(0))
        {
            playerController.SetShootAnimation(false);
        }
    }

    public override void Fire()
    {
        currentAmmo--;

        float damage = GetFinalDamage();
        Camera cam = Camera.main;
        Vector3 origin = cam.transform.position;
        Vector3 forward = cam.transform.forward;

        int pelletCount = 7;
        float spreadAngle = 10f; // 퍼짐 각도 (도 단위)

        for (int i = 0; i < pelletCount; i++)
        {
            // 랜덤한 방향 벡터 생성
            Vector3 spreadDirection = GetSpreadDirection(forward, spreadAngle);

            Ray ray = new Ray(origin, spreadDirection);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 50f))
            {
                Debug.DrawRay(origin, spreadDirection * hit.distance, Color.red, 1f);

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
            }
        }
    }



    public override void Reload()
    {
        if (CurrentAmmo >= gunData.maxAmmo)
        {
            return;
        }
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
 private IEnumerator ResetShootBool()
 {
     yield return null;                        
     playerController.SetShootAnimation(false);
 }
 private Vector3 GetSpreadDirection(Vector3 forward, float angle)
 {
     Quaternion randomRotation = Quaternion.Euler(
         Random.Range(-angle, angle), // pitch
         Random.Range(-angle, angle), // yaw
         0
     );

     return randomRotation * forward;
 }

}