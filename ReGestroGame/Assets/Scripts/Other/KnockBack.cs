using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockBack : MonoBehaviour {
    [SerializeField] private float _knockBackForce = 3f;
    [SerializeField] private float _knockBackMovingTimermax = 0.3f;

    private float _knockBackMovingTimer;

    private Rigidbody2D _rb;
   
    public bool IsGettingKnockedBack { get; private set; }
    
    private void Awake() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        _knockBackMovingTimer -= Time.deltaTime;
        if (_knockBackMovingTimer < 0)
            StopKnockBackMovement();
    }

    public void GetKnockedBack(Transform damageSourse) {
        IsGettingKnockedBack = true;
        _knockBackMovingTimer = _knockBackMovingTimermax;
        Vector2 difference = (transform.position - damageSourse.position).normalized * _knockBackForce / _rb.mass;
   _rb.AddForce(difference, ForceMode2D.Impulse);
    }

    public void StopKnockBackMovement() {
        _rb.velocity = Vector2.zero;
        IsGettingKnockedBack = false;
    }

}
