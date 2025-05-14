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
        //ü�¹� ��ȣ�ۿ� ���ϰ�
        hpBar.interactable = false;

        //�ܺο� �ִ� playerController ��ũ��Ʈ ã��
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