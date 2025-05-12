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
                AudioManager.Instance.PlayItemSound(itemType);
                switch (itemType)
                {
                    case ItemType.Score:
                        Debug.Log($"{amount}점 획득.");
                        ScoreManager.Instance.AddScore(amount);
                        break;
                    case ItemType.SpeedUp:
                        Debug.Log($"속도 {amount} 증가");
                        player.IncreaseSpeed(amount);
                        break;
                    case ItemType.SpeedDown:
                        Debug.Log($"속도 {amount} 감소");
                        player.DecreaseSpeed(amount);
                        break;
                    case ItemType.Heal:
                        Debug.Log($"체력 {amount} 회복");
                        player.Heal(amount);
                        break;
                }
            }

            StartCoroutine(PickupEffect());
        }
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