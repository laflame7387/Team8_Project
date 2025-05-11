using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickButton : MonoBehaviour, IButton
{
    public GameObject go;
    public void OnClicked()
    {
        go.SetActive(true);
    }
}
