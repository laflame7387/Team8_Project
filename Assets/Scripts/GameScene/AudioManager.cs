using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource bgmSource;

    [Header("Effect Soudns")]
    [SerializeField] private AudioClip scoreClip;
    [SerializeField] private AudioClip healClip;
    [SerializeField] private AudioClip speedUpClip;
    [SerializeField] private AudioClip speedDownClip;

    [Header("Player Sounds")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip damageClip;

    [Header("BGM Sounds")]
    [SerializeField] private AudioClip bgmClip;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (SceneManager.GetActiveScene().name == "GameScene")
            {
                PlayBGM();
            }
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

    public void PlayBGM()
    {
        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }

    public void StopBGM()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
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

    public void PlayJumpSound()
    {
        PlaySFX(jumpClip);
    }

    public void PlayDamageSound()
    {
        PlaySFX(damageClip);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "GameScene")
        {
            PlayBGM();
        }
    }
}
