using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ReGestroGame.Utils;
using UnityEngine.InputSystem.XR.Haptics;
public class EnemyAI : MonoBehaviour {
    [SerializeField] private State _startingState;
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 3f;
    [SerializeField] private float _roamingTimermax = 2f;

    [SerializeField] private bool _isChasingEnemy = false;
    private float _chasingDistance = 4f;
    private float _chasingSpeedMultiplier = 2f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private float _roamingTimer;
    private Vector3 _roamPosition;
    private Vector3 _startingPosition;

    private float _roamingSpeed;
    private float _chasingSpeed;
    public bool IsRunning {
        get { 
        if (_navMeshAgent.velocity == Vector3.zero) {
            return false;
        }
        else {
            return true;
        }
    }

    }

    private enum State {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Death
    }


    private void Awake() {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startingState;

        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMultiplier;
    }

    private void Update() {
        Statehandler();
    }

    private void Statehandler() {
        switch (_currentState) {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0) {
                    Roaming();
                    _roamingTimer = _roamingTimermax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChasingTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackingTarget();
                CheckCurrentState();
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }

    }

    private void ChasingTarget() {
        _navMeshAgent.SetDestination(Player.Instance.transform.position);
    }
    private void CheckCurrentState() {
        float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
        State newState = State.Roaming;

        if (_isChasingEnemy) {
            if (distanceToPlayer <= _chasingDistance) {
                newState = State.Chasing;
               
            }
        }
        if (newState != _currentState) {
            if ( newState == State.Chasing) { 
                _navMeshAgent.ResetPath();
                _navMeshAgent.speed = _chasingSpeed;
            }
            else if ( newState == State.Roaming) {
                _roamingTimer = 0f;
                _navMeshAgent.speed = _roamingSpeed;
            }
            _currentState = newState;
        }
       
    }
    private void AttackingTarget() {
        // Attack logic here
    }

    private void Roaming() {
        _startingPosition = GetRoamingPosition();
        _roamPosition = GetRoamingPosition();
        ChangeFacingDirection(_startingPosition, _roamPosition);
        _navMeshAgent.SetDestination(_roamPosition);
    }
    private Vector3 GetRoamingPosition() {
        return _startingPosition + ReGestroUtils.GetRandomDir() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 soursePosition, Vector3 targetPosition) {
        if (soursePosition.x > targetPosition.x) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }

    }

}

