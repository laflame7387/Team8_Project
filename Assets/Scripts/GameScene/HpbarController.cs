using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;

public class HpbarController : MonoBehaviour
{
    public GameObject Hp; // HP 바 오브젝트
    public RectTransform HpBar; // 체력바 스케일 조정용
    public float full = 5.0f; // 최대 체력
    private float energy = 0.0f; // 현재 누적 데미지
    private bool isDie = false;

    public event Action OnDeath; // 사망 시 이벤트

    private void Start()
    {
        StartCoroutine(ReduceHpOverTime()); // 시간에 따른 체력 감소
    }

    private IEnumerator ReduceHpOverTime()
    {
        while (!isDie)
        {
            TakeDamage(1.01f); // 0.1씩 체력 감소
            yield return new WaitForSeconds(0.1f); // 1초 간격
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") && !isDie)
        {
            TakeDamage(0.1f); // 트리거 충돌 시 체력 감소
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDie) return;

        energy += amount;
        energy = Mathf.Min(energy, full); // 최대 체력 초과 제한

        // 체력바 크기 갱신
        HpBar.localScale = new Vector3(1.0f - (energy / full), 1.0f, 1.0f);

        if (energy >= full && !isDie)
        {
            Die(); // 체력 0 → 사망 처리
        }
    }

    public bool IsDead()
    {
        return isDie;
    }

    private void Die()
    {
        isDie = true;
        Debug.Log("플레이어 사망!");
        gameObject.SetActive(false); // 체력 UI 또는 본체 비활성화
        OnDeath?.Invoke(); // 외부에 사망 알림
    }
}