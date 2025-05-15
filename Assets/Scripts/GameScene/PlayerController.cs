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
    [SerializeField] private const float MinMoveSpeed = 5f;
    [SerializeField] private int health = 100;
    public int Health => health;
    [SerializeField] private int maxHealth = 100;
    public int MaxHealth => maxHealth;
    [SerializeField] private HpbarController hpBarController;

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private bool IsGrounded = false; // Ground 상태 확인 변수
    private float holdTime = 0f; // Z키 홀드 시간
    private bool IsJumping = false; // 점프 중인지 확인
    private bool IsDamaged = false; // 데미지 상태 확인 변수
    private bool IsDie = false; // 사망 상태 확인 변수
    private bool IsCrouching = false; // 크라우치 상태 확인 변수
    private CapsuleCollider2D capsuleCollider; // BoxCollider2D 컴포넌트
    private Vector2 originalColliderSize; // 원래 콜라이더 크기
    private Vector2 crouchingColliderSize; // 크라우칭 시 콜라이더 크기
    
    public bool isWaiting = false;

    public bool Grounded => IsGrounded;
    public bool Damaged => IsDamaged;
    public bool Die => IsDie;
    

    public GameObject JumpGauge;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>(); // BoxCollider2D 초기화

        StartCoroutine(ReduceHealthOverTime());

        if (rb == null)
            Debug.LogError("Rigidbody2D is not attached to the player!");

        if (playerAnimator == null)
            Debug.LogError("Animator is not assigned!");


        if (capsuleCollider == null)
        {
            Debug.LogError("BoxCollider2D is not attached to the player!");
        }
        else
        {
            originalColliderSize = capsuleCollider.size;
            crouchingColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f); // y축 크기를 절반으로 줄임
        }
    }

    private void Update()
    {
        if (IsDie) return;
        if (isWaiting == false) return;

        UpdateAnimatorParameters();


        if (!IsDamaged)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            // Z키 홀드 시간 측정 (점프 우선)
            if (Input.GetKey(KeyCode.Z) && IsGrounded)
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
                IsJumping = true;
                IsCrouching = false; // 점프 중에는 크라우치 비활성화
                JumpGauge.SetActive(true);
            }
            else if (Input.GetKey(KeyCode.X))
            {
                // X키 홀드 시 크라우치 상태 활성화
                IsCrouching = true;
                AdjustColliderSize(true); // 크라우칭 시 콜라이더 크기 조정
            }
            else
            {
                IsCrouching = false;
                AdjustColliderSize(false); // 크라우칭 해제 시 콜라이더 크기 복구
            }

            if (Input.GetKeyUp(KeyCode.Z) && IsJumping)
            {
                PerformJump();
                AudioManager.Instance.PlayJumpSound();
                holdTime = 0f;
                IsJumping = false;
                JumpGauge.SetActive(false);
            }
        }

        if (transform.position.y <= -6f && !IsDie)
        {
            HandleDeath();
        }
    }

    private void AdjustColliderSize(bool isCrouching)
    {
        if (capsuleCollider != null)
        {
            capsuleCollider.size = isCrouching ? crouchingColliderSize : originalColliderSize;
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
        if (health > maxHealth) health = maxHealth;
        //hpBarController?.SetHealth(health);
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



            // 부모 오브젝트가 SpikeBundle 태그를 가지고 있는지 확인
            Transform parent = collision.gameObject.transform.parent;
            if (parent != null && parent.CompareTag("SpikeBundle"))
            {
                // 부모 오브젝트 파괴
                Destroy(parent.gameObject);
            }
            else
            {
                // 부모가 없거나 SpikeBundle 태그가 없으면 스파이크 오브젝트만 파괴
                Destroy(collision.gameObject);
            }
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

        AudioManager.Instance.PlayDamageSound();

        health -= 10;
        //hpBarController?.SetHealth(health);

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
        //기다리는 중에는 체력 달지 않게
        while (!isWaiting)
        {
            yield return null; // 매 프레임 검사하면서 대기
        }

        while (!IsDie)
        {
            health -= 1;
            health = Mathf.Clamp(health, 0, maxHealth);
            //hpBarController.SetHealth(health);

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

        AudioManager.Instance.StopBGM();
    }

    public bool CheckDeathCondition()
    {
        // 사망 조건을 확인하는 메서드
        if (transform.position.y <= -6f)
        {
            return true;
        }
        else if (health <= 0)
        {
            return true;
        }
        return false;
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
}