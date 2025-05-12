using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private ItemType itemType = ItemType.Score;
    [SerializeField] private AudioClip pickupSound;

    public enum ItemType
    {
        Score,
        SpeedUp,
        SpeedDown,
        Heal
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController player = collision.GetComponent<PlayerController>();

            if (player != null)
            {
                PlayPickupSound();
                switch (itemType)
                {
                    case ItemType.Score:
                        Debug.Log($"{amount}�� ȹ��.");
                        ScoreManager.Instance.AddScore(amount);
                        break;
                    case ItemType.SpeedUp:
                        Debug.Log($"�ӵ� {amount} ����");
                        player.IncreaseSpeed(amount);
                        break;
                    case ItemType.SpeedDown:
                        Debug.Log($"�ӵ� {amount} ����");
                        player.DecreaseSpeed(amount);
                        break;
                    case ItemType.Heal:
                        Debug.Log($"ü�� {amount} ȸ��");
                        player.Heal(amount);
                        break;
                }
            }

            StartCoroutine(PickupEffect());
        }
    }

    private void PlayPickupSound()
    {
        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && pickupSound != null)
            audio.PlayOneShot(pickupSound);
    }

    private IEnumerator PickupEffect()
    {
        float duration = 0.3f;
        float time = 0f;

        SpriteRenderer sr = GetComponentInChildren<SpriteRenderer>();
        Vector3 startPos = transform.position;
        Vector3 endPos = startPos + new Vector3(0, 0.5f, 0);

        Color originalColor = sr.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            transform.position = Vector3.Lerp(startPos, endPos, t);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 1 - t);

            yield return null;
        }

        Destroy(gameObject);
    }
}