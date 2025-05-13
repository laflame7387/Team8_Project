using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseController_Main : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    [SerializeField] private SpriteRenderer characterRenderer;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Animation_Main animationHandle;

    protected virtual void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        animationHandle = GetComponent<Animation_Main>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        HandleAction();
        Flip();
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
    }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction) // 플레이어 이동
    {
        direction = direction * 5;
        _rigidbody.velocity = direction;
        animationHandle.Move(direction);
    }

    protected void Flip() //플레이어 방향 전환
    {
        if (movementDirection.x != 0)
        {
            characterRenderer.flipX = movementDirection.x < 0;
        }
    }
}
