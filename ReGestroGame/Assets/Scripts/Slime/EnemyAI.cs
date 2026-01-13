using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using ReGestroGame.Utils;
using UnityEditor.Experimental.GraphView;
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State startingState;
   [SerializeField] private float roamingDistanceMax = 7f;
    [SerializeField] private float roamingDistanceMin = 3f;
    [SerializeField] private float roamingTimermax = 2f;

    private NavMeshAgent navMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamPosition;
    private Vector3 startingPosition;

    private enum State {
        Roaming
    }


    private void Awake() {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = false;
        navMeshAgent.updateUpAxis = false;
        state = startingState;
    }

    private void Update() {
        switch (state) {
            default:
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0) {
                    Roaming();
                    roamingTime = roamingTimermax;
                }
                break;

        }
    }
    private void Roaming() {
        startingPosition = GetRoamingPosition();
        roamPosition = GetRoamingPosition();
        ChangeFacingDirection(startingPosition, roamPosition);
        navMeshAgent.SetDestination(roamPosition);
        }
    private Vector3 GetRoamingPosition() {
        return startingPosition + ReGestroUtils.GetRandomDir() * UnityEngine.Random.Range(roamingDistanceMin, roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 soursePosition, Vector3 targetPosition) {
        if (soursePosition.x > targetPosition.x) {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        } else {
            transform.rotation = Quaternion.Euler(0, 0, 0);
          }

        }
    
    }

