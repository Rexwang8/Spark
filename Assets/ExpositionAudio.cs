using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TigerForge;
using UnityEngine.Audio;

public class ExpositionAudio : MonoBehaviour
{
    public AudioClip[] startExposition;
    public AudioClip[] endExposition;
    public AudioClip[] killallExposition;

    AudioSource aud;

    private void Awake()
    {
        aud = this.GetComponent<AudioSource>();
        EventManager.StartListening("AUDIOSTART", playStart);
        EventManager.StartListening("AUDIOEND", playEnd);
        EventManager.StartListening("AUDIOKILL", playKill);
    }

    private void playStart()
    {
        aud.PlayOneShot(startExposition[Mathf.RoundToInt(Random.Range(0, startExposition.Length))]);
        Debug.Log("Playing AUdio START");

    }

    private void playEnd()
    {
        AudioClip clip = endExposition[Mathf.RoundToInt(Random.Range(0, endExposition.Length))];
        aud.PlayOneShot(clip);
        Debug.Log("Playing AUdio END");

    }

    private void playKill()
    {
        aud.PlayOneShot(killallExposition[Mathf.RoundToInt(Random.Range(0, killallExposition.Length))]);
        Debug.Log("Playing AUdio KILL");

    }
}
