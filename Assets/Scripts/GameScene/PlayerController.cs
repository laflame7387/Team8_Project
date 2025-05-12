using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // x축 이동 속도
    [SerializeField] private float baseJumpVelocity = 4f; // 기본 y축 점프 속도
    [SerializeField] private float maxAdditionalJumpVelocity = 4f; // 추가 y축 점프 속도
    [SerializeField] private float maxHoldTime = 1f; // Z키 최대 홀드 시간
    [SerializeField] private float minHoldTime = 0.2f; // Z키 최소 홀드 시간
    [SerializeField] private Animator playerAnimator; // 플레이어 애니메이터

    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    private bool IsGrounded = false; // Ground 상태 확인 변수
    private float holdTime = 0f; // Z키 홀드 시간
    private bool IsJumping = false; // 점프 중인지 확인
    private bool IsDamaged = false; // 데미지 상태 확인 변수
    private bool IsDie = false; // 사망 상태 확인 변수
    private bool IsCrouching = false; // 크라우치 상태 확인 변수
    private BoxCollider2D boxCollider; // BoxCollider2D 컴포넌트
    private Vector2 originalColliderSize; // 원래 콜라이더 크기
    private Vector2 crouchingColliderSize; // 크라우칭 시 콜라이더 크기

    public bool Grounded => IsGrounded; // IsGrounded 프로퍼티
    public bool Damaged => IsDamaged; // IsDamaged 프로퍼티
    public bool Die => IsDie; // IsDie 프로퍼티

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>(); // BoxCollider2D 초기화
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D is not attached to the player!");
        }

        if (playerAnimator == null)
        {
            Debug.LogError("Animator is not assigned!");
        }

        if (boxCollider == null)
        {
            Debug.LogError("BoxCollider2D is not attached to the player!");
        }
        else
        {
            originalColliderSize = boxCollider.size;
            crouchingColliderSize = new Vector2(originalColliderSize.x, originalColliderSize.y * 0.5f); // y축 크기를 절반으로 줄임
        }
    }

    private void Update()
    {
        if (IsDie) return; // 사망 상태에서는 아무 동작도 하지 않음

        // 애니메이터에 상태 전달
        UpdateAnimatorParameters();

        if (!IsDamaged)
        {
            // x축 이동 속도 유지
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

            // Z키 홀드 시간 측정 (점프 우선)
            if (Input.GetKey(KeyCode.Z) && IsGrounded)
            {
                holdTime += Time.deltaTime;
                holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime); // 최대 홀드 시간 제한
                IsJumping = true;
                IsCrouching = false; // 점프 중에는 크라우치 비활성화
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

            // Z키를 떼면 점프 실행
            if (Input.GetKeyUp(KeyCode.Z) && IsJumping)
            {
                PerformJump();
                holdTime = 0f; // 홀드 시간 초기화
                IsJumping = false;
            }
        }
    }

    private void AdjustColliderSize(bool isCrouching)
    {
        if (boxCollider != null)
        {
            boxCollider.size = isCrouching ? crouchingColliderSize : originalColliderSize;
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
        // Ground 태그가 붙은 오브젝트에서 벗어날 시
        if (collision.gameObject.CompareTag("Ground"))
        {
            IsGrounded = false;
        }
    }

    private IEnumerator HandleDamage()
    {
        IsDamaged = true;
        float originalMoveSpeed = moveSpeed;

        // 애니메이터에 IsDamaged 전달
        playerAnimator.SetBool("IsDamaged", true);

        // 속도를 -5로 설정
        moveSpeed = -5f;

        // 1초 대기
        yield return new WaitForSeconds(1f);

        // 사망 판정
        if (CheckDeathCondition())
        {
            IsDie = true;
        }
        else
        {
            IsDamaged = false;
            playerAnimator.SetBool("IsDamaged", false); // 애니메이터에 IsDamaged 전달
            moveSpeed = originalMoveSpeed; // 속도 복구
        }
    }

    private bool CheckDeathCondition()
    {
        // 사망 조건을 확인하는 메서드
        // 실제 게임 로직에 따라 수정 필요
        if (transform.position.y <= -6f)
        {
            Debug.Log("Player has fallen to death.");
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