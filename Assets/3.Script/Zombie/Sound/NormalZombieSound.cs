using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombieSound : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    
    [SerializeField] private AudioClip growlClip;
    [SerializeField] private AudioClip deathClip;
    
    [SerializeField] private float minDelay = 3f;
    [SerializeField] private float maxDelay = 8f;
    
    IEnumerator PlayGrowlLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            audioSource.pitch = Random.Range(0.95f, 1.05f); // 피치 변화로 반복감 감소
            audioSource.PlayOneShot(growlClip);
        }
    }

    void Start()
    {
        float initialDelay = Random.Range(0f, 3f); // 각 좀비마다 시작 시간 다르게
        Invoke(nameof(StartGrowlLoop), initialDelay);
    }

    void StartGrowlLoop()
    {
        StartCoroutine(PlayGrowlLoop());
    }

    public void StopGrowlLoopAndPlayDeath()
    {
        StopAllCoroutines();
        audioSource.Stop(); // 현재 사운드 중단

        if (deathClip != null)
        {
            audioSource.pitch = 1f; // 기본 피치로 되돌림
            audioSource.PlayOneShot(deathClip);
        }
    }
}
