using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BaseController_Main : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    protected Vector2 movementDirection = Vector2.zero;
    public Vector2 MovementDirection { get { return movementDirection; } }

    protected Animation_Main animationHandle;
    
    private float lastDirectionX = 1f;
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
    }

    protected virtual void FixedUpdate()
    {
        Movement(movementDirection);
    }

    protected virtual void HandleAction()
    {

    }

    private void Movement(Vector2 direction)
    {
        direction = direction * 5;
        _rigidbody.velocity = direction;

        
        if (direction.x != 0)
            lastDirectionX = direction.x;

        float yRotation = lastDirectionX > 0 ? 0f : 180f;
        transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

        animationHandle.Move(direction);
    }


}
