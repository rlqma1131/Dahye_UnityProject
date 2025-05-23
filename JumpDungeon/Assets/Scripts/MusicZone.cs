using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicZone : MonoBehaviour
{
    public AudioSource audiosource;
    public float fadeTime;
    public float maxVolume;
    private float targetVolume;

    void Start()
    {
        targetVolume = 0f;
        audiosource = GetComponent<AudioSource>();
        audiosource.volume = targetVolume;
        audiosource.Play();
    }

    void Update()
    {
        if (!Mathf.Approximately(audiosource.volume, targetVolume))  // 근사값은 동일하게 해주는 함수 Approximately
        {
            audiosource.volume = Mathf.MoveTowards(audiosource.volume, targetVolume, (maxVolume / fadeTime) * Time.deltaTime);    // 점진적으로 증가
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))  // 플레이어가 들어왔을때
        {
            targetVolume = maxVolume;  // 최대 볼륨으로 설정
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))  // 플레이어가 나갔을때
        {
            targetVolume = 0f;  // 볼륨을 0으로 설정
        }
    }
}
