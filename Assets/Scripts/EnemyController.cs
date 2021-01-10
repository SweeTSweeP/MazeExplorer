using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private AIPath _aiPath;
    [SerializeField] private Animator _animator;
    [SerializeField] private AIDestinationSetter _aiDestinationSetter;
    
    private static readonly int Horizontal = Animator.StringToHash("Horizontal");
    private static readonly int IsWalking = Animator.StringToHash("IsWalking");
    private static readonly int Vertical = Animator.StringToHash("Vertical");

    private void Start()
    {
        _aiDestinationSetter.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        _animator.SetFloat(Horizontal, _aiPath.desiredVelocity.x);
        _animator.SetFloat(Vertical, _aiPath.desiredVelocity.y);
        _animator.SetBool(IsWalking, true);
    }
}
