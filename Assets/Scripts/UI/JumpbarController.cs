using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpController : MonoBehaviour
{
    [SerializeField] private Image fillTransform; // 바 채우는 부분
    [SerializeField] private float maxFillWidth = 1f; // 바 최대 길이
    [SerializeField] private float maxHoldTime = 1f;  // 최대 점프 충전 시간

    private float holdTime = 0f;
    private bool isHolding = false;

    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            holdTime += Time.deltaTime;
            holdTime = Mathf.Clamp(holdTime, 0f, maxHoldTime);
            isHolding = true;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            holdTime = 0f;
            isHolding = false;
        }

        UpdateBarVisual();
    }

    private void UpdateBarVisual()
    {
        float ratio = holdTime / maxHoldTime;
        //fillTransform.localScale = new Vector3(ratio * maxFillWidth, 1f, 1f);
        fillTransform.fillAmount = ratio;
    }
}
