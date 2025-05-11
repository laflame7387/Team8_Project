using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseButton : MonoBehaviour, IButton
{
    public GameObject go;
    public void OnClicked()
    {
        go.SetActive(false);
    }
}
