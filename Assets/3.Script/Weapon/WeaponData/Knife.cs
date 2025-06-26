using UnityEngine;

public class Knife : WeaponController
{
    [SerializeField] private WeaponDataSO knifeRootData; 

    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip fireSingle;
    
    private void Awake()
    {
        this.weapon = new Weapon(knifeRootData, WeaponGrade.Common);
        weapon.currentAmmo = weapon.currentStat.magazine;
    }
    
    // WeaponManager가 호출해서 Fire만 실행
    public override void Fire()
    {
        audioSource.PlayOneShot(fireSingle);
        
        Camera cam = Camera.main;
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 1.5f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.red, 1f);
            
            var isCritical = IsCritical();
            var damage = GetFinalDamage(isCritical);
            
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
                StartCoroutine(uiManager.PrintDamage_Coroutine(combatEvent, damage, isCritical));
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