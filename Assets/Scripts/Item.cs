using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    [SerializeField] private ItemType itemType = ItemType.Score;

    public enum ItemType
    {
        Score,
        SpeedUp,
        SpeedDown,
        Heal
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerControllerTest player = collision.GetComponent<PlayerControllerTest>();

            if (player != null)
            {
                switch (itemType)
                {
                    case ItemType.Score:
                        player.AddScore(amount);
                        Debug.Log($"{amount}점 획득.");
                        break;
                    case ItemType.SpeedUp:
                        player.IncreaseSpeed(amount);
                        Debug.Log($"속도 {amount} 증가");
                        break;
                    case ItemType.SpeedDown:
                        player.DecreaseSpeed(amount);
                        Debug.Log($"속도 {amount} 감소");
                        break;
                    case ItemType.Heal:
                        player.Heal(amount);
                        Debug.Log($"체력 {amount} 회복");
                        break;
                }
            }

            Destroy(gameObject);
        }
    }
}
