using UnityEngine;
using System.Collections;

public class Shotgun : WeaponController
{
    private float nextFireTime = 0f;
    private PlayerController playerController;
    [SerializeField] private WeaponDataSO shotgunRootData; 

    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip fireSingle;

    private void Awake()
    {
        this.weapon = new Weapon(shotgunRootData, WeaponGrade.Common);
        weapon.currentAmmo = weapon.currentStat.magazine;
    }
    
    private void OnEnable()
    {
        playerController = FindObjectOfType<PlayerController>();
        weapon.isReloading = false;
    }

    protected void Update()
    {
        if (weapon.isReloading)
            return;

        // 좌클릭 발사
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && !isOpenPanel && !WeaponManager.instance.stateInfo.IsName("Draw"))
        {
            if (weapon.currentAmmo > 0)
            {
                Fire();
                
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(fireSingle);
                
                playerController.SetShootAnimation(true);
                StartCoroutine(ResetShootBool());
                nextFireTime = Time.time + weapon.currentStat.fireRate;
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
        weapon.currentAmmo--;

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

                var damage = GetFinalDamage();

                if (hit.collider.CompareTag("Zombie"))
                {
                    var monster = CombatSystem.Instance.GetMonsterOrNull(hit.collider);

                    if (monster != null)
                    {
                        CombatEvent combatEvent = new CombatEvent();
                        combatEvent.Sender = Player.localPlayer;
                        combatEvent.Receiver = monster;
                        combatEvent.Damage = damage; // ✅ 버프 적용된 최종 데미지 사용 --> 수정 중이라 바뀜
                        combatEvent.HitPosition = hit.point;
                        combatEvent.Collider = hit.collider;

                        CombatSystem.Instance.AddInGameEvent(combatEvent);
                        StartCoroutine(uiManager.PrintDamage_Coroutine(combatEvent, damage));
                    }
                }
            }
        }
    }



    public override void Reload()
    {
        if (weapon.currentAmmo >= weapon.currentStat.magazine)
        {
            return;
        }
        playerController.SetReloadAnimation();
        StartCoroutine(ReloadRoutine());
    }
    private IEnumerator ReloadRoutine()
    {
        weapon.isReloading = true;

        yield return new WaitForSeconds(weapon.currentStat.reloadTime);

        weapon.currentAmmo = weapon.currentStat.magazine;
        weapon.isReloading = false;

        // OnReloadComplete();
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