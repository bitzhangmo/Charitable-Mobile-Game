using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    public static AudioManager _Instence;

    public static AudioManager Instance
    {
        get
        {
            if(_Instence == null)
            {
                GameObject AudioManager = new GameObject("AudioManager");
                _Instence = AudioManager.AddComponent<AudioManager>();
            }
            return _Instence;
        }
    }

    public AudioClip[] clips;
    public void PlayAudio(AudioSource _audio, AudioClip clip, UnityAction callback = null, bool isLoop = false)
    {
        _audio.clip = clip;
        _audio.loop = isLoop;
        _audio.Play();
        StartCoroutine(AudioPlayFinished(_audio.clip.length, callback));
    }

    private IEnumerator AudioPlayFinished(float time, UnityAction callback)
    {
        yield return new WaitForSeconds(time);
        callback.Invoke();
    }
}
