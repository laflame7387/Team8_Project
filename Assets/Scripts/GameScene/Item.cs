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
                        Debug.Log($"{amount}�� ȹ��.");
                        player.AddScore(amount);
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

            Destroy(gameObject);
        }
    }
}
