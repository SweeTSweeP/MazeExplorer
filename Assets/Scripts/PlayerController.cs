using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _speed = 5f;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Animator _animator;

    private float _vertical;
    private float _horizontal;

    private Vector2 _movement;

    private void Update()
    {
        _movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        _animator.SetFloat("Horizontal", _movement.x);
        _animator.SetFloat("Vertical", _movement.y);
        _animator.SetBool("IsWalking", _movement.sqrMagnitude > 0.01);

        if (_movement.x > 0.1 || _movement.x < -0.1 || _movement.y > 0.1 || _movement.y < -0.1)
        {
            _animator.SetFloat("Last_Horizontal", _movement.x);
            _animator.SetFloat("Last_Vertical", _movement.y);
        }
    }

    private void FixedUpdate()
    {
        _rigidbody.MovePosition(_rigidbody.position + _movement * _speed * Time.fixedDeltaTime);
    }

    private void Move()
    {
        _rigidbody.velocity = _movement * _speed;
    }
}
