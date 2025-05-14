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
        //체력바 상호작용 못하게
        hpBar.interactable = false;

        //외부에 있는 playerController 스크립트 찾기
        playerController = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        maxHealth = playerController.MaxHealth;
        currentHealth = playerController.Health;

        hpBar.value = currentHealth / maxHealth;
        //Mathf.Clamp(currentHealth, 0, maxHealth);
    }
}