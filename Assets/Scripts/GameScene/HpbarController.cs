using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using System;

public class HpbarController : MonoBehaviour
{
    public GameObject Hp; // HP �� ������Ʈ
    public RectTransform HpBar; // ü�¹� ������ ������
    public float full = 5.0f; // �ִ� ü��
    private float energy = 0.0f; // ���� ���� ������
    private bool isDie = false;

    public event Action OnDeath; // ��� �� �̺�Ʈ

    private void Start()
    {
        StartCoroutine(ReduceHpOverTime()); // �ð��� ���� ü�� ����
    }

    private IEnumerator ReduceHpOverTime()
    {
        while (!isDie)
        {
            TakeDamage(1.01f); // 0.1�� ü�� ����
            yield return new WaitForSeconds(0.1f); // 1�� ����
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Spike") && !isDie)
        {
            TakeDamage(0.1f); // Ʈ���� �浹 �� ü�� ����
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDie) return;

        energy += amount;
        energy = Mathf.Min(energy, full); // �ִ� ü�� �ʰ� ����

        // ü�¹� ũ�� ����
        HpBar.localScale = new Vector3(1.0f - (energy / full), 1.0f, 1.0f);

        if (energy >= full && !isDie)
        {
            Die(); // ü�� 0 �� ��� ó��
        }
    }

    public bool IsDead()
    {
        return isDie;
    }

    private void Die()
    {
        isDie = true;
        Debug.Log("�÷��̾� ���!");
        gameObject.SetActive(false); // ü�� UI �Ǵ� ��ü ��Ȱ��ȭ
        OnDeath?.Invoke(); // �ܺο� ��� �˸�
    }
}