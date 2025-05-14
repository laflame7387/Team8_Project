using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public GameObject go;
    public void OnPaused()
    {
        go.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void OnPlay()
    {
        go.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
