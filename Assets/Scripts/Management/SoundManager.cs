using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance { get; set; }

    public AudioSource dropItemSound;

    public AudioSource toolSound;

    public AudioSource choppingSound;

    public AudioSource pickItemSound;

    public AudioSource walkOnGrassSound;



    public void PlayDropSound()
    {

        if (dropItemSound.isPlaying == false)
        {
            dropItemSound.Play();
        }

    }

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
}
