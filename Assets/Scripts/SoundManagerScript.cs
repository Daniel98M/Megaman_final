using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerDeath;
    public static AudioClip enemyDeath;
    public static AudioClip jump;
    public static AudioClip bullet;
    static AudioSource audioSrc;

    void Start()
    {
        playerDeath = Resources.Load<AudioClip>("death");
        enemyDeath = Resources.Load<AudioClip>("enemy_death");
        jump = Resources.Load<AudioClip>("jump");
        bullet = Resources.Load<AudioClip>("bullet");
        audioSrc = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "death":
                audioSrc.PlayOneShot(playerDeath);
                break;
            case "enemyDeath":
                audioSrc.PlayOneShot(enemyDeath);
                break;
            case "jump":
                audioSrc.PlayOneShot(jump);
                break;
            case "bullet":
                audioSrc.PlayOneShot(bullet);
                break;
        }
    }
}
