using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerMovement {

    private const float MIN_MOVEMENT_THRESHOLD = 0.1f;

    [SerializeField] private KeyCode[] directionKeys = new KeyCode[] { KeyCode.Z, KeyCode.S, KeyCode.Q, KeyCode.D };
    [SerializeField] private KeyCode rollKey = KeyCode.LeftShift;
    [SerializeField] private float rollStrength;

    private Player player;
    private Rigidbody rigidbody;

    public void OnStart (Player player) {
        this.player = player;
        rigidbody = player.GetComponent<Rigidbody>();
    }

    public void OnUpdate () {
        Vector3 inputDirection = GetInputDirection();

        rigidbody.AddForce(player.sharedStatistics.acceleration * inputDirection, ForceMode.Acceleration);

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            float deltaAngle = Vector3.SignedAngle(rigidbody.gameObject.transform.forward, inputDirection, Vector3.up);
            rigidbody.AddTorque(Vector3.up * deltaAngle * player.sharedStatistics.angularAcceleration);
        }

        if (!player.state.isRolling && Input.GetKeyDown(rollKey)) {
            rigidbody.AddForce(rollStrength * inputDirection, ForceMode.VelocityChange);
            player.animator.SetTrigger("roll");
            player.state.isRolling = true;
            player.StartCoroutine(DetectRollEnd());
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

    private IEnumerator DetectRollEnd () {
        yield return new WaitUntil(() => !System.Array.Exists(player.animator.GetCurrentAnimatorClipInfo(0), c => c.clip.name.Equals("Roll")));
        player.state.isRolling = false;
    }
}
