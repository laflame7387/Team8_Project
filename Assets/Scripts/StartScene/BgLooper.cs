using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgLooper : MonoBehaviour
{
    public int numBg = 3;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Intro_BG"))
        {
            float widthOfBg = ((BoxCollider2D)collision).size.x * 0.4f;
            Vector3 pos = collision.transform.position;

            pos.x += widthOfBg * numBg;
            collision.transform.position = pos;
            return;
        }
    }
}
