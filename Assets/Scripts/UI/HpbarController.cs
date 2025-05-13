using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HpbarController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private Slider hpBar; // 체력바
    public float maxHealth = 0f; // 최대 체력
    public float currentHealth = 0f; // 현재 체력

    private void Start()
    {
        //hpBar.interactable = false;
        playerController = FindObjectOfType<PlayerController>();

        
    }

    private void Update()
    {
        maxHealth = playerController.MaxHealth;
        currentHealth = playerController.Health;

        hpBar.value = currentHealth / maxHealth;
        //Mathf.Clamp(currentHealth, 0, maxHealth);
        //
    }

    //public void SetMaxHealth(float max)
    //{
    //    maxHealth = max;
    //    currentHealth = max;
    //    UpdateBar();
    //}

    // 체력 업데이트 (외부에서 호출)
    //public void SetHealth(float value)
    //{
    //    currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    //    //UpdateBar();
    //}

    // UI의 value값 조절
    //private void UpdateBar()
    //{
        
    //}
}