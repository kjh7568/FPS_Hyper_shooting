using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalZombieSound : MonoBehaviour
{
    [Header("사운드 소스")]
    [SerializeField] private AudioSource audioSource;

    [Header("사운드 클립")]
    [SerializeField] private AudioClip growlClip;
    [SerializeField] private AudioClip runLoopClip;
    [SerializeField] private AudioClip deathClip;

    [Header("설정")]
    [SerializeField] private float minGrowlDelay = 3f;
    [SerializeField] private float maxGrowlDelay = 8f;
    
    private Coroutine growlRoutine;
    private Coroutine runLoopRoutine;

    private bool isAlreadyRun = false;
    
    void Start()
    {
        StartGrowlLoop();
    }

    // 🧟 으르렁 루프 시작
    private void StartGrowlLoop()
    {
        StopAllCoroutines();
        growlRoutine = StartCoroutine(GrowlLoop());
    }

    private IEnumerator GrowlLoop()
    {
        while (true)
        {
            float delay = Random.Range(minGrowlDelay, maxGrowlDelay);
            yield return new WaitForSeconds(delay);

            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(growlClip);
        }
    }

    // 🏃‍♂️ 추격 시 루프 사운드 (걷는/뛰는 소리)
    public void PlayRunLoop()
    {
        if(isAlreadyRun) return;

        isAlreadyRun = true;
        
        StopAllCoroutines();

        if (runLoopClip != null)
        {
            audioSource.clip = runLoopClip;
            audioSource.loop = true;
            audioSource.pitch = 1f;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("runLoopClip이 비어있습니다!");
        }
    }

    // ☠️ 죽을 때 호출
    public void PlayDeathSound()
    {
        StopAllCoroutines();
        audioSource.Stop();

        if (deathClip != null)
        {
            audioSource.loop = false;
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(deathClip);
        }
        else
        {
            Debug.LogWarning("deathClip이 비어있습니다!");
        }
    }
}
