using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Grenade : WeaponController
{
    [SerializeField] private LayerMask monsterLayer;
    [SerializeField] private WeaponDataSO grenadeRootData; 
    [SerializeField] private ParticleSystem explosion;

    [SerializeField] private bool isDummy = false;
    
    private Rigidbody rb;
    private bool isAlreadyBomb = false;
    [Header("사운드 설정")]
    public AudioSource audioSource;
    public AudioClip explosionSingle;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        this.weapon = new Weapon(grenadeRootData, WeaponGrade.Common); 
        rb.velocity += Camera.main.transform.forward * 20f;
    }

    private void Update()
    {
        if(isDummy) Destroy(gameObject);
        
        // explosion이 활성화 되고, explosion의 파티클이 재생되지 않는 상황이라면
        if (explosion.gameObject.activeInHierarchy && explosion.isPlaying == false)
        {
            Destroy(gameObject);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) return;
        if(isAlreadyBomb) return;

        isAlreadyBomb = true;
        explosion.gameObject.SetActive(true);
        
        // 오직 Monster 레이어에만 반응
        Collider[] hits = Physics.OverlapSphere(transform.position, 3f, monsterLayer);

        foreach (var hit in hits)
        {
            var target = CombatSystem.Instance.GetMonsterOrNull(hit);
            if (target != null)
            {
                var combatEvent = new CombatEvent
                {
                    Sender = Player.localPlayer,
                    Receiver = target,
                    Damage = WeaponManager.instance.grenadeWeapon.weapon.currentStat.damage,
                    HitPosition = hit.ClosestPoint(transform.position),
                    Collider = hit.GetComponent<Collider>()
                };
                CombatSystem.Instance.AddInGameEvent(combatEvent);
            }
        }

        audioSource.PlayOneShot(explosionSingle);
        StartCoroutine(DelayDestroy());
    }

    private IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(3f);        
        Destroy(gameObject);
    }
    
    public override void Fire()
    {
        
    }

    public override void Reload()
    {
        throw new System.NotImplementedException();
    }
}