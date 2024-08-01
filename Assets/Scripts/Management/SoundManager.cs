using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; set; }

    public AudioSource dropItemSound;

    public AudioSource toolSound;

    public AudioSource craftingSound;

    public AudioSource choppingSound;

    public AudioSource pickItemSound;

    public AudioSource walkOnGrassSound;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {

        if (soundToPlay.isPlaying == false)
        {
            soundToPlay.Play();
        }

    }



}
