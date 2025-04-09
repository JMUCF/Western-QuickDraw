using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // ===================== Audio =====================
    [Header("Audio Settings")]
    public AudioClip gunClick;
    public AudioClip gunShot;
    public int poolSize = 10;

    private Queue<AudioSource> audioPool = new Queue<AudioSource>();

    void Awake()
    {
        // Initialize audio source pool
        for (int i = 0; i < poolSize; i++)
        {
            GameObject audioObj = new GameObject("PooledAudioSource");
            audioObj.transform.SetParent(transform);
            AudioSource source = audioObj.AddComponent<AudioSource>();
            source.playOnAwake = false;
            audioPool.Enqueue(source);
        }
    }

    private AudioSource GetPooledSource()
    {
        AudioSource source = audioPool.Dequeue();
        audioPool.Enqueue(source);
        return source;
    }

    public void PlayGunClick()
    {
        AudioSource source = GetPooledSource();
        source.clip = gunClick;
        source.volume = 1f;
        source.Play();
    }

    public void PlayGunShot()
    {
        AudioSource source = GetPooledSource();
        source.clip = gunShot;
        source.volume = 1f;
        source.Play();
    }
}
