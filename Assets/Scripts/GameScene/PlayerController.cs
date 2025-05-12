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
    [SerializeField] private HpbarController hpBarController; // 체력 관리 전용 컴포넌트

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private bool IsGrounded = false; // Ground 상태 확인 변수
    private float holdTime = 0f; // Z키 홀드 시간
    private bool IsJumping = false; // 점프 중인지 확인
    private bool IsDamaged = false; // 피격 상태 확인
    private bool IsDie = false; // 사망 상태 확인
    private bool IsCrouching = false; // 슬라이딩 상태 여부

    public bool Grounded => IsGrounded; // 외부 접근용 프로퍼티
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
            Debug.LogError("HpbarController is not assigned!");

        // 체력 시스템에서 사망 이벤트 구독
        hpBarController.OnDeath += HandleDeath;
    }

    private void Update()
    {
        if (IsDie) return; // 사망 상태에서는 조작 불가

        UpdateAnimatorParameters(); // 애니메이터 파라미터 갱신

        // x축 이동
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        // 점프 입력 처리
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

        // 슬라이드 처리
        if (Input.GetKey(KeyCode.X) && !IsCrouching)
        {
            StartSlide();
        }
        else if (Input.GetKeyUp(KeyCode.X) && IsCrouching)
        {
            EndSlide();
        }
    }

    private void PerformJump()
    {
        float additionalVelocity = 0f;

        if (holdTime >= maxHoldTime)
            additionalVelocity = maxAdditionalJumpVelocity;
        else if (holdTime >= minHoldTime)
            additionalVelocity = Mathf.Lerp(0f, maxAdditionalJumpVelocity, (holdTime - minHoldTime) / (maxHoldTime - minHoldTime));

        float jumpVelocity = baseJumpVelocity + additionalVelocity;

        rb.velocity = new Vector2(rb.velocity.x, jumpVelocity);

        if (playerAnimator != null)
            playerAnimator.Play("Jump P1 (Start)");
    }

    private void StartSlide()
    {
        IsCrouching = true;
        normalCollider.enabled = false;
        slideCollider.enabled = true;

        if (playerAnimator != null)
            playerAnimator.Play("Slide P1");
    }

    private void EndSlide()
    {
        IsCrouching = false;
        slideCollider.enabled = false;
        normalCollider.enabled = true;

        if (playerAnimator != null)
            playerAnimator.Play("Run P1");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = true;

        if (collision.gameObject.CompareTag("Spike") && !IsDamaged)
        {
            StartCoroutine(HandleDamage()); // 피격 처리 코루틴 실행
            hpBarController.TakeDamage(0.1f); // 체력 감소 호출
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            IsGrounded = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("SpeedUpZone"))
        {
            moveSpeed += speedIncreaseAmount; // 트리거 진입 시 속도 증가
        }
    }

    private IEnumerator HandleDamage()
    {
        IsDamaged = true;
        float originalMoveSpeed = moveSpeed;

        playerAnimator.SetBool("IsDamaged", true);
        moveSpeed = 0f; // 뒤로 밀림

        yield return new WaitForSeconds(1f);

        // 사망 여부는 HpbarController에서 판정
        if (!hpBarController.IsDead())
        {
            IsDamaged = false;
            playerAnimator.SetBool("IsDamaged", false);
            moveSpeed = originalMoveSpeed;
        }
    }

    private void HandleDeath()
    {
        IsDie = true;
        playerAnimator.SetBool("IsDie", true);
        rb.velocity = Vector2.zero;
        Debug.Log("플레이어가 사망했습니다.");
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
}