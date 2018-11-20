using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMovement {

    private const float MIN_MOVEMENT_THRESHOLD = 0.1f;

    [SerializeField] private KeyCode[] directionKeys = new KeyCode[] { KeyCode.Z, KeyCode.S, KeyCode.Q, KeyCode.D };
    [SerializeField] private KeyCode rollKey = KeyCode.LeftShift;
    [SerializeField] private float rollCost;
    [SerializeField] private float staminaRefillSpeed = 1;

    private Dictionary<int, Vector3> directionVectors = new Dictionary<int, Vector3>() {
        {Direction.UP, (Vector3.forward + Vector3.right).normalized},
        {Direction.DOWN, (Vector3.back + Vector3.left).normalized},
        {Direction.LEFT, (Vector3.left + Vector3.forward).normalized},
        {Direction.RIGHT, (Vector3.right + Vector3.back).normalized}
    };

    private Player player;
    private Rigidbody rigidbody;

    public void OnStart (Player player) {
        this.player = player;
        rigidbody = player.GetComponent<Rigidbody>();
        player.animationManager.OnEnter += AnimationManager_OnEnter;
        player.animationManager.OnExit += AnimatorManager_OnExit;

        Debug.Log(player.instanceStatistics.stamina + " " + player.sharedStatistics.maxStamina);
    }

    public void OnUpdate () {
        Vector3 inputDirection = GetInputDirection();

        rigidbody.AddForce(player.sharedStatistics.acceleration * inputDirection, ForceMode.Acceleration);

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            float deltaAngle = Vector3.SignedAngle(rigidbody.gameObject.transform.forward, inputDirection, Vector3.up);
            rigidbody.AddTorque(Vector3.up * deltaAngle * player.sharedStatistics.angularAcceleration);
        }

        if (!player.state.isRolling && Input.GetKeyDown(rollKey) && player.instanceStatistics.stamina > rollCost) {
            rigidbody.AddForce(player.sharedStatistics.rollStrength * inputDirection, ForceMode.VelocityChange);
            player.animationManager.animator.SetTrigger("roll");
            player.instanceStatistics.stamina -= rollCost;
        }

        player.animationManager.animator.SetFloat("speed", rigidbody.velocity.magnitude);
        player.instanceStatistics.stamina += Time.deltaTime * staminaRefillSpeed;
        player.instanceStatistics.stamina = Mathf.Min(player.instanceStatistics.stamina, player.sharedStatistics.maxStamina);
        player.hud.UpdateCoolDownBar(player.instanceStatistics.stamina / player.sharedStatistics.maxStamina);
    }

    public void SetKey (int direction, KeyCode key) {
        directionKeys[direction] = key;
    }

    private Vector3 GetInputDirection () {
        Vector3 inputDirection = Vector3.zero;
        foreach (int direction in new int[] { Direction.UP, Direction.DOWN, Direction.LEFT, Direction.RIGHT }) {
            if (Input.GetKey(directionKeys[direction])) {
                inputDirection += directionVectors[direction];
            }
        }

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            inputDirection.Normalize();
        }

        return inputDirection;
    }

    private void AnimationManager_OnEnter (string eventName) {
        if (eventName.Equals("Roll")) {
            player.state.isRolling = true;
            player.gameObject.layer = LayerMask.NameToLayer("GhostPlayer"); ;
        }
    }

    private void AnimatorManager_OnExit (string eventName) {
        if (eventName.Equals("Roll")) {
            player.state.isRolling = false;
            player.gameObject.layer = LayerMask.NameToLayer("Player");
        }
    }
}
