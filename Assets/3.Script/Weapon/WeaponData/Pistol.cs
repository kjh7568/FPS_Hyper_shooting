using System;
using UnityEngine;
using System.Collections;
using Random = UnityEngine.Random;

public class Pistol : WeaponController
{
    private float nextFireTime = 0f;
    private PlayerController playerController;
    [SerializeField] private WeaponDataSO pistolRootData; 

    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip fireSingle;

    private void Awake()
    {
        this.weapon = new Weapon(pistolRootData, WeaponGrade.Common);        
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
        if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime && !isOpenPanel &&
            !WeaponManager.instance.stateInfo.IsName("Draw"))
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
            Debug.Log("피스톨 r키 눌림");
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

        if (Physics.Raycast(ray, out hit, 50f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);

            var isCritical = IsCritical();
            var damage = GetFinalDamage(isCritical);

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
                    StartCoroutine(uiManager.PrintDamage_Coroutine(combatEvent, damage,isCritical));
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
            Debug.Log("이미 탄창이 가득 차 있습니다.");
            return;
        }

        playerController.SetReloadAnimation();
        StartCoroutine(ReloadRoutine());
    }

    private IEnumerator ReloadRoutine()
    {
        weapon.isReloading = true;

        yield return new WaitForSeconds(weapon.currentStat.reloadTime * (1 - Player.localPlayer.inventory.EquipmentStat.increaseReloadSpeed));

        weapon.currentAmmo = weapon.currentStat.magazine;
        weapon.isReloading = false;

        // OnReloadComplete();
    }

    private IEnumerator ResetShootBool()
    {
        yield return null;
        playerController.SetShootAnimation(false);
    }
}