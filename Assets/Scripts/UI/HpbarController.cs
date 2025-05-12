using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;
using UnityEngine.UI;

public class HpbarController : MonoBehaviour
{
    [SerializeField] private Slider HpBar; // 체력바 스케일 조정용
    private float maxHealth = 100f; // 최대 체력
    private float currentHealth = 100f; // 현재 체력

    public void SetMaxHealth(float max)
    {
        maxHealth = max;
        currentHealth = max;
        UpdateBar();
    }

    // 체력 업데이트 (외부에서 호출)
    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
        UpdateBar();
    }

    // 실제 UI 스케일 조정
    private void UpdateBar()
    {
        float ratio = currentHealth / maxHealth;
        //HpBar.localScale = new Vector3(ratio, 1f, 1f);
    }
}