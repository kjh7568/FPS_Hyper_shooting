using UnityEngine;
using System.Collections;

public class Rifle : Gun
{
    private float nextFireTime = 0f;

    protected override void Update()
    {
        base.Update(); // U 키 레벨업 유지

        if (isReloading)
            return;

        // 좌클릭 발사
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            if (currentAmmo > 0)
            {
                Fire();
                nextFireTime = Time.time + currentStat.fireRate;
            }
            else
            {
                Debug.Log("탄약 없음! 장전이 필요함.");
            }
        }

        // R 키 장전
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }
    }

    public override void Fire()
    {
        currentAmmo--;
        Debug.Log($"[발사] {gunData.gunName} Lv.{currentLevel} → 데미지: {currentStat.damage} | 남은 탄약: {currentAmmo}");
    }

    public override void Reload()
    {
        if (isReloading)
        {
            Debug.Log("이미 장전 중입니다.");
            return;
        }

        if (currentAmmo == gunData.maxAmmo)
        {
            Debug.Log("탄약이 이미 가득 찼습니다.");
            return;
        }

        StartCoroutine(ReloadRoutine());
    }
    private IEnumerator ReloadRoutine()
    {
        isReloading = true;

        Debug.Log($"[장전 시작] {gunData.gunName}... ({currentStat.reloadTime}초)");

        yield return new WaitForSeconds(currentStat.reloadTime);

        currentAmmo = gunData.maxAmmo;
        isReloading = false;

        Debug.Log($"[장전 완료] 탄약 {currentAmmo}/{gunData.maxAmmo}");
    }

}