using UnityEngine;

public class Knife : Gun
{
    // WeaponManager가 호출해서 Fire만 실행
    public override void Fire()
    {
        float damage = GetFinalDamage();
        Debug.Log($"[칼 휘두름] {gunData.gunName} Lv.{currentLevel} → 데미지: {damage}");

        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 1.5f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
            var target = hit.collider.GetComponent<IDamageAble>();
            if (target != null)
            {
                var combatEvent = new CombatEvent
                {
                    Sender = Player.localPlayer,
                    Receiver = target,
                    Damage = Mathf.RoundToInt(damage),
                    HitPosition = hit.point,
                    Collider = hit.collider
                };
                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }
        else
        {
            Debug.Log("칼 공격이 아무것도 맞추지 못함.");
        }
    }

    public override void Reload()
    {
        Debug.Log("칼은 장전할 수 없습니다.");
    }
}