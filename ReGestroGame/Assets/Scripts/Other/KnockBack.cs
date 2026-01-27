using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour {
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackMovingTimermax = 0.3f;

    private float _knockBackMovingTimer;

    private Rigidbody2D rb;
   


    private void Awake() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }

    public void GetKnockedBack(Transform damageSourse) {
        _knockBackMovingTimer = _knockBackMovingTimermax;
        Vector2 difference = (transform.position - damageSourse.position).normalized * _knockBackForce / rb.mass;
   rb.AddForce(difference, ForceMode2D.Impulse);
    }

    private void StopKnockBackMovement() {
        rb.velocity = Vector2.zero;
    }

}
