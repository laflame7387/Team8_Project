using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HpbarController : MonoBehaviour
{
    private PlayerController playerController;

    [SerializeField] private Slider hpBar; // ü�¹�
    public float maxHealth = 0f; // �ִ� ü��
    public float currentHealth = 0f; // ���� ü��

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

    // ü�� ������Ʈ (�ܺο��� ȣ��)
    //public void SetHealth(float value)
    //{
    //    currentHealth = Mathf.Clamp(value, 0f, maxHealth);
    //    //UpdateBar();
    //}

    // UI�� value�� ����
    //private void UpdateBar()
    //{
        
    //}
}