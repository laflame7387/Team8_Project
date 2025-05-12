using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;

    [Header("Effect Soudns")]
    public AudioClip scoreClip;
    public AudioClip healClip;
    public AudioClip speedUpClip;
    public AudioClip speedDownClip;

    /* [Header("Player Sounds")]
    public AudioClip jumpClip;
    public AudioClip landClip;
    public AudioClip slidingClip;
    public AudioClip damageClip; */

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip != null)
            sfxSource.PlayOneShot(clip);
    }

    public void PlayItemSound(Item.ItemType type)
    {
        switch (type)
        {
            case Item.ItemType.Score:
                PlaySFX(scoreClip);
                break;
            case Item.ItemType.Heal:
                PlaySFX(healClip);
                break;
            case Item.ItemType.SpeedUp:
                PlaySFX(speedUpClip);
                break;
            case Item.ItemType.SpeedDown:
                PlaySFX(speedDownClip);
                break;
        }
    }

    /* public void PlayJumpSound()
    {
        PlaySFX(jumpClip); // »£√‚¿∫ AudioManager.Instance.PlayJumpSound();
    } */
}
