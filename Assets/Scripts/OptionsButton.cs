using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsButton : MonoBehaviour, IButton
{
    public GameObject go;
    public void OnClicked()
    {
        go.SetActive(true);
    }
}
