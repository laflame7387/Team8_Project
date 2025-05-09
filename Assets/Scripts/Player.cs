using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f; // x축 이동 속도
    [SerializeField] private float baseJumpVelocity = 4f; // 기본 y축 점프 속도
    [SerializeField] private float maxAdditionalJumpVelocity = 4f; // 추가 y축 점프 속도
    [SerializeField] private float maxHoldTime = 1f; // Z키 최대 홀드 시간
    [SerializeField] private float minHoldTime = 0.2f; // Z키 최소 홀드 시간
    [SerializeField] private Animator playerAnimator; // 플레이어 애니메이터
    [SerializeField] private float speedIncreaseInterval = 50f; // 50 유닛마다 증가
    [SerializeField] private float speedIncreaseAmount = 0.5f; // 얼마나 빨라질지

    private float nextSpeedIncreaseDistance = 50f;
    private Rigidbody2D rb; // Rigidbody2D 컴포넌트
    //private bool isGrounded = false; // Ground 상태 확인 변수
    private float holdTime = 0f; // Z키 홀드 시간
    private bool isJumping = false; // 점프 중인지 확인
    private float distanceTraveled = 0f;
    private Vector2 lastPosition;

    //public bool IsGrounded => isGrounded; // IsGrounded 프로퍼티

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
        // x축 이동 속도 유지
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        float distanceThisFrame = Vector2.Distance(transform.position, lastPosition);
        distanceTraveled += distanceThisFrame;
        lastPosition = transform.position;

        // 일정 거리마다 속도 증가
        if (distanceTraveled >= nextSpeedIncreaseDistance)
        {
            moveSpeed += speedIncreaseAmount;
            nextSpeedIncreaseDistance += speedIncreaseInterval;
        }

        //Z키 홀드 시간 측정
        if (Input.GetKey(KeyCode.Z))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime); // 최대 홀드 시간 제한
            isJumping = true;
        }

        // Z키를 떼면 점프 실행
        if (Input.GetKeyUp(KeyCode.Z) && isJumping)
        {
            PerformJump();
            holdTime = 0f; // 홀드 시간 초기화
            isJumping = false;
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

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    // Ground 태그가 붙은 오브젝트와 충돌 시
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = true;
    //    }
    //}

    //private void OnCollisionExit2D(Collision2D collision)
    //{
    //    // Ground 태그가 붙은 오브젝트에서 벗어날 시
    //    if (collision.gameObject.CompareTag("Ground"))
    //    {
    //        isGrounded = false;
    //    }
    //}
}
