using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpZone : MonoBehaviour
{
    public bool WarpZoneEnter;
    public GameObject PressKeyUI;
    public GameObject WarpUI;

    private void Update()
    {
        if(WarpZoneEnter && Input.GetKey(KeyCode.F))
        {
            WarpZoneEnter = true;
            WarpUI.SetActive(true);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            WarpZoneEnter = true;
            PressKeyUI.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            WarpZoneEnter = false;
            PressKeyUI.SetActive(false);
            WarpUI.SetActive(false);
        }
    }
    
}
