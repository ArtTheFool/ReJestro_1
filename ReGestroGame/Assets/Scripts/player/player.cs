using UnityEngine;
[SelectionBase]
public class Player : MonoBehaviour {

    public static Player Instance { get; private set; }

   [SerializeField] private float _movingSpeed = 10f;
    Vector2 inputVector;
    private Rigidbody2D rb;
    private KnockBack _knockBack;

    private float _minMovingSpeed = 0.1f;
    private bool _isRunning = false;

    private void Awake() {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();   
        _knockBack = GetComponent<KnockBack>();
    }
    private void Start() {
        GameInput.Instance.OnPlayerAttack += Player_OnPlayerAttack;
    }

    private void Player_OnPlayerAttack(object sender, System.EventArgs e) {
     ActiveWeapon.Instance.GetActiveWepon().Attack();
    }

    private void Update() {
      inputVector = GameInput.Instance.GetMovementVector();
    }

    private void FixedUpdate() {
        HandleMovement();
    }
    private void TakeDamage(Transform damageSource, int damage) 
    {
        _knockBack.GetKnockedBack(damageSource);
    }

    private void HandleMovement() {
        rb.MovePosition(rb.position + inputVector * (_movingSpeed * Time.fixedDeltaTime));
        if (Mathf.Abs(inputVector.x) > _minMovingSpeed || Mathf.Abs(inputVector.y) > _minMovingSpeed) {
            _isRunning = true;
        }
        else {
            _isRunning = false;
        }
    }

    public bool IsRunning() {
        return _isRunning;
    }

    public Vector3 GetPlayerScreenPosition() {
        Vector3 playerScreenPosition = Camera.main.WorldToScreenPoint(transform.position);
        return playerScreenPosition;
    }

}