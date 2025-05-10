using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // x축 이동 속도
    [SerializeField] private float baseJumpVelocity = 4f; // 기본 y축 점프 속도
    [SerializeField] private float maxAdditionalJumpVelocity = 4f; // 추가 y축 점프 속도
    [SerializeField] private float maxHoldTime = 1f; // Z키 최대 홀드 시간
    [SerializeField] private float minHoldTime = 0.2f; // Z키 최소 홀드 시간
    [SerializeField] private Animator playerAnimator; // 플레이어 애니메이터
    [SerializeField] private float speedIncreaseAmount = 1f; // 트리거로 증가할 이동 속도
    [SerializeField] private CapsuleCollider2D normalCollider; // 기본 콜라이더
    [SerializeField] private CapsuleCollider2D slideCollider; // 슬라이딩용 콜라이더
    [SerializeField] private float damageInterval = 1f; // 체력 감소 간격 (초)
    [SerializeField] private float healthDecreaseRate = 1f; //
    [SerializeField] private int maxHealth = 100; // <__최대 체력__>


    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private bool IsGrounded = false; // Ground 상태 확인 변수
    private float holdTime = 0f; // Z키 홀드 시간
    private bool IsJumping = false; // 점프 중인지 확인
    private bool IsDamaged = false; // 데미지 상태 확인 변수
    private bool IsDie = false; // 사망 상태 확인 변수
    private bool IsCrouching = false; // 슬라이딩 상태 여부
    private float damageTimer = 0f;
    private int currentHealth;
    public bool Grounded => IsGrounded; // IsGrounded 프로퍼티
    public bool Damaged => IsDamaged; // IsDamaged 프로퍼티
    public bool Die => IsDie; // IsDie 프로퍼티

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the player!");
        }

        if (playerAnimator == null)
        {
            Debug.LogError("Animator is not assigned!");
        }
        currentHealth = maxHealth; // <__초기 체력 설정__>
        //hpBar.UpdateHPBar(currentHealth, maxHealth); // <__HP 바 초기화__>
    }

    private void Update()
    {
        if (IsDie) return; // 사망 상태에서는 아무 동작도 하지 않음

        // 애니메이터에 상태 전달
        UpdateAnimatorParameters();

        if (!IsDamaged)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                damageTimer = 0f;
                currentHealth -= Mathf.RoundToInt(healthDecreaseRate);
                //hpBar.UpdateHPBar(currentHealth, maxHealth); // <__HP바 반영__>

                if (CheckDeathCondition())
                {
                    HandleDeathByTime(); // <__시간 기반 사망 처리__>
                }
            }

            // x축 이동 속도 유지
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            // Z키 홀드 시간 측정
            if (Input.GetKey(KeyCode.Z) && IsGrounded)
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime); // 최대 홀드 시간 제한
                IsJumping = true;
            }

            // Z키를 떼면 점프 실행
            if (Input.GetKeyUp(KeyCode.Z) && IsJumping)
            {
                PerformJump();
                holdTime = 0f; // 홀드 시간 초기화
                IsJumping = false;
            }

            // 슬라이딩 입력 처리
            if (Input.GetKey(KeyCode.X) && !IsCrouching)
            {
                StartSlide(); // X 키 누름 → 슬라이드 시작
            }
            else if (Input.GetKeyUp(KeyCode.X) && IsCrouching)
            {
                EndSlide(); // X 키 뗌 → 슬라이드 종료
            }
        }
    }

    private void PerformJump()
    {
        // 추가 속도 계산
        float additionalVelocity = 0f;

        if (holdTime >= maxHoldTime)
        {
            // 최대 홀드 시간 이후에는 추가 속도 최대값
            additionalVelocity = maxAdditionalJumpVelocity;
        }
        else if (holdTime >= minHoldTime)
        {
            // 최소 홀드 시간 이상일 때 선형 보간
            additionalVelocity = Mathf.Lerp(0f, maxAdditionalJumpVelocity, (holdTime - minHoldTime) / (maxHoldTime - minHoldTime));
        }

        // 최종 점프 속도 계산
        float jumpVelocity = baseJumpVelocity + additionalVelocity;

        // Rigidbody2D에 y축 속도 적용
        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        // 점프 애니메이션 재생
        if (playerAnimator != null)
        {
            playerAnimator.Play("Jump P1 (Start)");
        }
    }
    private IEnumerator HandleDamage()
    {
        IsDamaged = true;
        float originalMoveSpeed = moveSpeed;

        playerAnimator.SetBool("IsDamaged", true);
        moveSpeed = -5f;

        currentHealth--; // 체력 감소

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 체력 또는 낙사 체크
        if (CheckDeathCondition())
        {
            HandleDeath();
        }
        else
        {
            IsDamaged = false;
            playerAnimator.SetBool("IsDamaged", false);
            moveSpeed = originalMoveSpeed;
        }
    }
    private void StartSlide()
    {
        IsCrouching = true;

        normalCollider.enabled = false;
        slideCollider.enabled = true;

        if (playerAnimator != null)
        {
            playerAnimator.Play("Slide P1");
        }
    }

    private void EndSlide()
    {
        IsCrouching = false;

        slideCollider.enabled = false;
        normalCollider.enabled = true;

        if (playerAnimator != null)
        {
            playerAnimator.Play("Run P1");
        }
    }

    //private IEnumerator HandleDamage()
    //{
    //    IsDamaged = true;
    //    float originalMoveSpeed = moveSpeed;

    //    playerAnimator.SetBool("IsDamaged", true); // <__피격 애니메이션 활성화__>

    //    moveSpeed = -5f; // <__피격 시 후진__>

    //    yield return new WaitForSeconds(1f);

    //    currentHealth--; // <__체력 감소__>
    //    //hpBar.UpdateHPBar(currentHealth, maxHealth); // <__HP 바 반영__>

    //    if (CheckDeathCondition())
    //    {
    //        IsDie = true; // <__사망 처리__>
    //    }
    //    else
    //    {
    //        IsDamaged = false;
    //        playerAnimator.SetBool("IsDamaged", false); // <__애니메이션 리셋__>
    //        moveSpeed = originalMoveSpeed; // <__속도 복원__>
    //    }
    //}

    private bool CheckDeathCondition()
    {
        return currentHealth <= 0; // <__체력이 0 이하일 때 사망__>
    }

    private void HandleDeathByTime()
    {
        IsDie = true;
        Debug.Log("Player died by time/health depletion."); // <__체력 고갈로 인한 사망__>
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Ground 태그가 붙은 오브젝트와 충돌 시
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = true;
        }

        // Spike 태그가 붙은 오브젝트와 충돌 시
        if (collision.gameObject.CompareTag("Spike") && !IsDamaged)
        {
            StartCoroutine(HandleDamage());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Ground 태그가 붙은 오브젝트에서 벗어날 시
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }
    //private bool CheckDeathCondition()
    //{
    //    if (transform.position.y < -10f) // 낙사
    //    {
    //        return true;
    //    }

    //    if (currentHealth <= 0) // 체력 고갈
    //    {
    //        return true;
    //    }

    //    return false;
    //}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedUpZone")) // 트리거에 태그가 SpeedUpZone이면
        {
            moveSpeed += speedIncreaseAmount; // 이동 속도 증가
        }
    }
    private void UpdateAnimatorParameters()
    {
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("IsGrounded", IsGrounded);
            playerAnimator.SetBool("IsDamaged", IsDamaged);
            playerAnimator.SetBool("IsDie", IsDie);
        }
    }

    private void HandleDeath()
    {
        IsDie = true;
        playerAnimator.SetBool("IsDie", true);
        rb.velocity = Vector2.zero;

        // 필요 시: 게임 오버 UI 호출, 재시작 버튼 활성화 등
        Debug.Log("플레이어가 사망했습니다.");
    }
}