using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioClip HitBlock;
    public AudioClip HitWall;
    public AudioClip HitBoss;
    public AudioClip DefeatBoss;
    public AudioClip HitAcc;

    AudioSource Audio;


    private void Awake()
    {
        instance = this;
    }


    // Start is called before the first frame update
    void Start()
    {
        Audio= GetComponent<AudioSource>();
    }

    public void HitBlockAudio()
    {
        Audio.PlayOneShot(HitBlock);
    }
    public void HitWallAudio()
    {
        Audio.PlayOneShot(HitWall);
    }
    public void HitBossAudio()
    {
        Audio.PlayOneShot(HitBoss);
    }

    public void DefeatBossAudio()
    {
        Debug.Log("Defeat!!!!!!!!!!!!!!!!!!!");
        Audio.PlayOneShot(DefeatBoss);
    }
    public void HitAccAudio()
    {
        Audio.PlayOneShot(HitAcc);
    }
}
