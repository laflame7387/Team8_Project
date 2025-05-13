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
    public float maxHealth = 100f; // �ִ� ü��
    public float currentHealth = 100f; // ���� ü��

    private void Start()
    {
        hpBar.interactable = false;
        //playerController = FindObjectOfType<PlayerController>();

        maxHealth = playerController.MaxHealth;
        currentHealth = playerController.Health;
    }

    private void Update()
    {
        //currentHealth -= Time.deltaTime;
        hpBar.value = currentHealth / maxHealth;
        
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