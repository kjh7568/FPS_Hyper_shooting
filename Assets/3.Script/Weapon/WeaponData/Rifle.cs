using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Rifle : WeaponController
{
    private float nextFireTime = 0f;
    private PlayerController playerController; 
    [SerializeField] private WeaponDataSO rifleRootData; 

    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip fireSingle;
    public AudioClip reloadSingle;

    private void Awake()
    {
        weapon = new Weapon(rifleRootData, WeaponGrade.Common);
        weapon.currentAmmo = weapon.currentStat.magazine;
    }

    private void OnEnable()
    {
        playerController = FindObjectOfType<PlayerController>();
        weapon.isReloading = false;
    }

    private void Update()
    {
        if (weapon.isReloading)
            return;
        
        // 좌클릭 발사
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime && !isOpenPanel && !WeaponManager.instance.stateInfo.IsName("Draw"))
        {
            if (weapon.currentAmmo > 0)
            {
                Fire();
                
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(fireSingle);
                
                playerController.SetShootAnimation(true);
                nextFireTime = Time.time + weapon.currentStat.fireRate;
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
    }

    public override void Fire()
    {
        weapon.currentAmmo--;

        StartCoroutine(PlayMuzzleFlash());
        
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
                    combatEvent.Damage = GetFinalDamage(); // ✅ 버프 적용된 최종 데미지 사용 --> 수정 중이라 바뀜
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
             if (weapon.currentAmmo >= weapon.currentStat.magazine)
             {
                 return;
             }
             
             StartCoroutine(ReloadRoutine());
         }

    private IEnumerator ReloadRoutine()
    {
        weapon.isReloading = true;
        
        playerController.SetShootAnimation(false);
        playerController.SetReloadAnimation();
        playerController.SetAnimationSpeed(Player.localPlayer.inventory.EquipmentStat.reloadSpeedReduction);
        
        audioSource.PlayOneShot(reloadSingle);
        
        yield return new WaitForSeconds(weapon.currentStat.reloadTime * (1 - Player.localPlayer.inventory.EquipmentStat.reloadSpeedReduction));

        weapon.currentAmmo = weapon.currentStat.magazine;;
        weapon.isReloading = false;

        // OnReloadComplete();
    }
}