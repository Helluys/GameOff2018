using UnityEngine;

[System.Serializable]
public class PlayerMovement {

    private const float MIN_MOVEMENT_THRESHOLD = 0.1f;

    [SerializeField] private KeyCode[] keys = null;
    private Player player;
    private Rigidbody rigidbody;

    public void OnStart (Player player) {
        this.player = player;
        this.rigidbody = player.GetComponent<Rigidbody>();
    }

    public void OnUpdate () {
        Vector3 inputDirection = GetInputDirection();

        this.rigidbody.AddForce(this.player.sharedStatistics.acceleration * inputDirection, ForceMode.Acceleration);

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            float deltaAngle = Vector3.SignedAngle(this.rigidbody.gameObject.transform.forward, inputDirection, Vector3.up);
            this.rigidbody.AddTorque(Vector3.up * deltaAngle * this.player.sharedStatistics.angularAcceleration);
        }
    }

    private Vector3 GetInputDirection () {
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(this.keys[Direction.UP])) {
            inputDirection += Vector3.forward;
        }

        if (Input.GetKey(this.keys[Direction.DOWN])) {
            inputDirection += Vector3.back;
        }

        if (Input.GetKey(this.keys[Direction.LEFT])) {
            inputDirection += Vector3.left;
        }

        if (Input.GetKey(this.keys[Direction.RIGHT])) {
            inputDirection += Vector3.right;
        }

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            inputDirection.Normalize();
        }

        return inputDirection;
    }

    public void SetKey (int direction, KeyCode key) {
        this.keys[direction] = key;
    }
}
