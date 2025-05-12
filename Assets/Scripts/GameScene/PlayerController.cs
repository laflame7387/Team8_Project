using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float baseJumpVelocity = 4f;
    [SerializeField] private float maxAdditionalJumpVelocity = 4f;
    [SerializeField] private float maxHoldTime = 1f;
    [SerializeField] private float minHoldTime = 0.2f;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private const float MinMoveSpeed = 1.0f;
    [SerializeField] private int health = 100;
    [SerializeField] private const int MaxHealth = 100;
    [SerializeField] private HpbarController hpBarController;

    private Rigidbody2D rb;
    private bool IsGrounded = false;
    private float holdTime = 0f;
    private bool IsJumping = false;
    private bool IsDamaged = false;
    private bool IsDie = false;
    private bool IsCrouching = false;

    public bool Grounded => IsGrounded;
    public bool Damaged => IsDamaged;
    public bool Die => IsDie;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
            Debug.LogError("Rigidbody2D is not attached to the player!");

        if (playerAnimator == null)
            Debug.LogError("Animator is not assigned!");

        if (hpBarController == null)
            Debug.LogError("HpBarController is not assigned!");

        hpBarController.SetMaxHealth(MaxHealth);
        hpBarController.SetHealth(health);

        StartCoroutine(ReduceHealthOverTime());
    }

    private void Update()
    {
        if (IsDie) return;

        UpdateAnimatorParameters();

        if (!IsDamaged)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            if (Input.GetKeyDown(KeyCode.X) && IsGrounded && !IsDamaged)
            {
                IsCrouching = true;
                playerAnimator.SetBool("IsCrouching", true);
            }

            if (Input.GetKeyUp(KeyCode.X))
            {
                IsCrouching = false;
                playerAnimator.SetBool("IsCrouching", false);
            }
            if (Input.GetKey(KeyCode.Z) && IsGrounded)
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
                IsJumping = true;
            }

            if (Input.GetKeyUp(KeyCode.Z) && IsJumping)
            {
                PerformJump();
                holdTime = 0f;
                IsJumping = false;
            }


        }
    }

    public void IncreaseSpeed(float amount)
    {
        moveSpeed += amount;
        Debug.Log($"속도 증가, 현재 속도 : {moveSpeed}");
    }

    public void DecreaseSpeed(float amount)
    {
        moveSpeed -= amount;
        if (moveSpeed < MinMoveSpeed) moveSpeed = MinMoveSpeed;
        Debug.Log($"속도 감소, 현재 속도 : {moveSpeed}");
    }

    public void Heal(int amount)
    {
        health += amount;
        if (health > MaxHealth) health = MaxHealth;
        hpBarController?.SetHealth(health);
        Debug.Log($"체력 회복, 현재 체력 : {health}");
    }

    private void PerformJump()
    {
        float additionalVelocity = 0f;

        if (holdTime >= maxHoldTime)
        {
            additionalVelocity = maxAdditionalJumpVelocity;
        }
        else if (holdTime >= minHoldTime)
        {
            additionalVelocity = Mathf.Lerp(0f, maxAdditionalJumpVelocity, (holdTime - minHoldTime) / (maxHoldTime - minHoldTime));
        }

        float jumpVelocity = baseJumpVelocity + additionalVelocity;
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        playerAnimator?.Play("Jump P1 (Start)");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }

        if (collision.gameObject.CompareTag("Spike") && !IsDamaged)
        {
            StartCoroutine(HandleDamage());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    private IEnumerator HandleDamage()
    {
        IsDamaged = true;
        float originalMoveSpeed = moveSpeed;

        playerAnimator.SetBool("IsDamaged", true);

        health -= 10;
        hpBarController?.SetHealth(health);

        if (CheckDeathCondition())
        {
            HandleDeath();
            yield break;
        }
        moveSpeed = 0f;
        yield return new WaitForSeconds(1f);

        IsDamaged = false;
        playerAnimator.SetBool("IsDamaged", false);
        moveSpeed = originalMoveSpeed;

    }

    private IEnumerator ReduceHealthOverTime()
    {
        while (!IsDie)
        {
            health -= 1;
            health = Mathf.Clamp(health, 0, MaxHealth);
            hpBarController.SetHealth(health);

            if (health <= 0)
            {
                HandleDeath();
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void HandleDeath()
    {
        IsDie = true;
        playerAnimator.SetBool("IsDie", true);
        rb.velocity = Vector2.zero;
        Debug.Log("플레이어 사망!");
    }

    private bool CheckDeathCondition()
    {
        return health <= 0;
    }

    private void UpdateAnimatorParameters()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsGrounded", IsGrounded);
            playerAnimator.SetBool("IsDamaged", IsDamaged);
            playerAnimator.SetBool("IsDie", IsDie);
            playerAnimator.SetBool("IsCrouching", IsCrouching);
        }
    }

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("SpeedUp"))
    //    {
    //        IncreaseSpeed(2f); // 원하는 증가량만큼 설정
    //        Debug.Log("스피드 트리거 감지! 속도 증가.");

    //        Destroy(other.gameObject);
    //    }

    //}
}