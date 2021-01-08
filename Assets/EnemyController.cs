using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private Animator _animator;
    [SerializeField] private AIDestinationSetter _aiDestinationSetter;

    private void Start()
    {
        _aiDestinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _animator.SetFloat("Horizontal", _aiPath.desiredVelocity.x);
        _animator.SetFloat("Vertical", _aiPath.desiredVelocity.y);
        _animator.SetBool("IsWalking", true);
    }
}
