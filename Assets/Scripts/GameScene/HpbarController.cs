using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;

public class HpbarController : MonoBehaviour
{
    public RectTransform HpBar; // ü�¹� ������ ������
    private float maxHealth = 100f; // �ִ� ü��
    private float currentHealth = 100f; // ���� ü��

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
        currentHealth = max;
        UpdateBar();
    }

    // ü�� ������Ʈ (�ܺο��� ȣ��)
    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
        UpdateBar();
    }

    // ���� UI ������ ����
    private void UpdateBar()
    {
        float ratio = currentHealth / maxHealth;
        HpBar.localScale = new Vector3(ratio, 1f, 1f);
    }
}