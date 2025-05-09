using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // x�� �̵� �ӵ�
    [SerializeField] private float baseJumpVelocity = 4f; // �⺻ y�� ���� �ӵ�
    [SerializeField] private float maxAdditionalJumpVelocity = 4f; // �߰� y�� ���� �ӵ�
    [SerializeField] private float maxHoldTime = 1f; // ZŰ �ִ� Ȧ�� �ð�
    [SerializeField] private float minHoldTime = 0.2f; // ZŰ �ּ� Ȧ�� �ð�
    [SerializeField] private Animator playerAnimator; // �÷��̾� �ִϸ�����
    [SerializeField] private float speedIncreaseInterval = 50f; // 50 ���ָ��� ����
    [SerializeField] private float speedIncreaseAmount = 0.5f; // �󸶳� ��������

    private float nextSpeedIncreaseDistance = 50f;
    private Rigidbody2D rb; // Rigidbody2D ������Ʈ
    //private bool isGrounded = false; // Ground ���� Ȯ�� ����
    private float holdTime = 0f; // ZŰ Ȧ�� �ð�
    private bool isJumping = false; // ���� ������ Ȯ��
    private float distanceTraveled = 0f;
    private Vector2 lastPosition;

    //public bool IsGrounded => isGrounded; // IsGrounded ������Ƽ

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastPosition = transform.position;

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the player!");
        }

        if (playerAnimator == null)
        {
            Debug.LogError("Animator is not assigned!");
        }
    }

    private void Update()
    {
        // x�� �̵� �ӵ� ����
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        float distanceThisFrame = Vector2.Distance(transform.position, lastPosition);
        distanceTraveled += distanceThisFrame;
        lastPosition = transform.position;

        // ���� �Ÿ����� �ӵ� ����
        if (distanceTraveled >= nextSpeedIncreaseDistance)
        {
            moveSpeed += speedIncreaseAmount;
            nextSpeedIncreaseDistance += speedIncreaseInterval;
        }

        //ZŰ Ȧ�� �ð� ����
        if (Input.GetKey(KeyCode.Z))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime); // �ִ� Ȧ�� �ð� ����
            isJumping = true;
        }

        // ZŰ�� ���� ���� ����
        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            PerformJump();
            holdTime = 0f; // Ȧ�� �ð� �ʱ�ȭ
            isJumping = false;
        }
    }

    private void PerformJump()
    {
        // �߰� �ӵ� ���
        float additionalVelocity = 0f;

        if (holdTime >= maxHoldTime)
        {
            // �ִ� Ȧ�� �ð� ���Ŀ��� �߰� �ӵ� �ִ밪
            additionalVelocity = maxAdditionalJumpVelocity;
        }
        else if (holdTime >= minHoldTime)
        {
            // �ּ� Ȧ�� �ð� �̻��� �� ���� ����
            additionalVelocity = Mathf.Lerp(0f, maxAdditionalJumpVelocity, (holdTime - minHoldTime) / (maxHoldTime - minHoldTime));
        }

        // ���� ���� �ӵ� ���
        float jumpVelocity = baseJumpVelocity + additionalVelocity;

        // Rigidbody2D�� y�� �ӵ� ����
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        // ���� �ִϸ��̼� ���
        if (playerAnimator != null)
        {
            playerAnimator.Play("Jump P1 (Start)");
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Ground �±װ� ���� ������Ʈ�� �浹 ��
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // Ground �±װ� ���� ������Ʈ���� ��� ��
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}
}
