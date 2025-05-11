using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    public GameObject go;
    public GameObject largeVolume;
    public GameObject smallVolume;
    public GameObject mute;

    float bgmVolume;

    // Start is called before the first frame update
    void Start()
    {
        bgmVolume = go.GetComponent<Slider>().value * 100;
    }

    // Update is called once per frame
    void Update()
    {
        SoundImage();
    }

    void SoundImage()
    {
        if (bgmVolume == 100) return;

        if (bgmVolume <= 67.0f && bgmVolume > 34f)
        {
            largeVolume.SetActive(false);
        }

        else if (bgmVolume <= 34f && bgmVolume > 0)
        {
            smallVolume.SetActive(false);
        }

        else
        {
            mute.SetActive(true);
        }
    }
}
