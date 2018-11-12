using UnityEngine;

[System.Serializable]
public class PlayerMovement {

    private const float MIN_MOVEMENT_THRESHOLD = 0.1f;

    [SerializeField] private KeyCode[] directionKeys = new KeyCode[] { KeyCode.Z, KeyCode.S, KeyCode.Q, KeyCode.D };
    [SerializeField] private KeyCode rollKey = KeyCode.LeftShift;

    private Player player;
    private Rigidbody rigidbody;

    public void OnStart (Player player) {
        this.player = player;
        rigidbody = player.GetComponent<Rigidbody>();

        AnimatorTrigger animatorTrigger = player.animator.GetBehaviour<AnimatorTrigger>();
        animatorTrigger.OnEnter += AnimatorTrigger_OnEnter;
        animatorTrigger.OnExit += AnimatorTrigger_OnExit;
    }

    public void OnUpdate () {
        Vector3 inputDirection = GetInputDirection();

        rigidbody.AddForce(player.sharedStatistics.acceleration * inputDirection, ForceMode.Acceleration);

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            float deltaAngle = Vector3.SignedAngle(rigidbody.gameObject.transform.forward, inputDirection, Vector3.up);
            rigidbody.AddTorque(Vector3.up * deltaAngle * player.sharedStatistics.angularAcceleration);
        }

        if (!player.state.isRolling && Input.GetKeyDown(rollKey)) {
            rigidbody.AddForce(player.sharedStatistics.rollStrength * inputDirection, ForceMode.VelocityChange);
            player.animator.SetTrigger("roll");
        }

        player.animator.SetFloat("speed", rigidbody.velocity.magnitude);
    }

    public void SetKey (int direction, KeyCode key) {
        directionKeys[direction] = key;
    }

    private Vector3 GetInputDirection () {
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(directionKeys[Direction.UP])) {
            inputDirection += Vector3.forward;
        }

        if (Input.GetKey(directionKeys[Direction.DOWN])) {
            inputDirection += Vector3.back;
        }

        if (Input.GetKey(directionKeys[Direction.LEFT])) {
            inputDirection += Vector3.left;
        }

        if (Input.GetKey(directionKeys[Direction.RIGHT])) {
            inputDirection += Vector3.right;
        }

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            inputDirection.Normalize();
        }

        return inputDirection;
    }

    private void AnimatorTrigger_OnEnter (string eventName) {
        if (eventName.Equals("Roll"))
            player.state.isRolling = true;
    }

    private void AnimatorTrigger_OnExit (string eventName) {
        if (eventName.Equals("Roll"))
            player.state.isRolling = false;
    }
}
