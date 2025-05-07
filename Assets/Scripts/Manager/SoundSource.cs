using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSource : MonoBehaviour
{
    private AudioSource _audioSource;

    public void Play(AudioClip clip, float soundEffectVolume, float soundEffectPitchVariance)
    {
        if(_audioSource == null)
            _audioSource = GetComponent<AudioSource>();

        CancelInvoke(); // 재사용될 때 Invoke 캔슬
        _audioSource.clip = clip;
        _audioSource.volume = soundEffectVolume;
        _audioSource.Play();
        _audioSource.pitch = 1f + Random.Range(-soundEffectPitchVariance, soundEffectPitchVariance);

        Invoke("Disable", clip.length + 2); //2초 뒤 Disable
    }

    public void Disable()
    {
        _audioSource?.Stop();
        Destroy(this.gameObject);
    }
}
