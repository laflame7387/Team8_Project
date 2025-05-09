using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    private int score = 0;
    private float speed = 5.0f;
    private const float MinSpeed = 1.0f;
    private int health = 100;
    private const int MaxHealth = 100;

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log($"���� ����, ���� ���� : {score}");
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        Debug.Log($"�ӵ� ����, ���� �ӵ� : {speed}");
    }

    public void DecreaseSpeed(float amount)
    {
        speed -= amount;
        if (speed < MinSpeed) speed = MinSpeed;
        Debug.Log($"�ӵ� ����, ���� �ӵ� : {speed}");
    }

    public void Heal(int mount)
    {
        health += mount;
        if (health > MaxHealth) health = MaxHealth;
        Debug.Log($"ü�� ȸ��, ���� ü�� : {health}");
    }
}
