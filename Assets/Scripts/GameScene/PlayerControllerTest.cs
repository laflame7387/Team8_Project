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
        Debug.Log($"점수 증가, 현재 점수 : {score}");
    }

    public void IncreaseSpeed(float amount)
    {
        speed += amount;
        Debug.Log($"속도 증가, 현재 속도 : {speed}");
    }

    public void DecreaseSpeed(float amount)
    {
        speed -= amount;
        if (speed < MinSpeed) speed = MinSpeed;
        Debug.Log($"속도 감소, 현재 속도 : {speed}");
    }

    public void Heal(int mount)
    {
        health += mount;
        if (health > MaxHealth) health = MaxHealth;
        Debug.Log($"체력 회복, 현재 체력 : {health}");
    }
}
