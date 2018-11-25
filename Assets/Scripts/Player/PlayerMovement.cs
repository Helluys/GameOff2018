using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerMovement {

    private const float MIN_MOVEMENT_THRESHOLD = 0.1f;

    [SerializeField] private float rollCost;
    [SerializeField] private float staminaRefillSpeed = 1;

    private Dictionary<InputType, Vector3> directionVectors = new Dictionary<InputType, Vector3>() {
        {InputType.Up, (Vector3.forward + Vector3.right).normalized},
        {InputType.Down, (Vector3.back + Vector3.left).normalized},
        {InputType.Left, (Vector3.left + Vector3.forward).normalized},
        {InputType.Right, (Vector3.right + Vector3.back).normalized}
    };

    private Player player;
    private Rigidbody rigidbody;

    public void OnStart (Player player) {
        this.player = player;
        rigidbody = player.GetComponent<Rigidbody>();
        player.animationManager.OnEnter += AnimationManager_OnEnter;
        player.animationManager.OnExit += AnimatorManager_OnExit;
    }

    public void OnUpdate () {
        Vector3 inputDirection = GetInputDirection();

        rigidbody.AddForce(player.sharedStatistics.acceleration * inputDirection, ForceMode.Acceleration);

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            float deltaAngle = Vector3.SignedAngle(rigidbody.gameObject.transform.forward, inputDirection, Vector3.up);
            rigidbody.AddTorque(Vector3.up * deltaAngle * player.sharedStatistics.angularAcceleration);
        }

        if (!player.state.isRolling && InputManager.Instance.IsKeyDown(InputType.Roll) && player.instanceStatistics.stamina > rollCost) {
            rigidbody.AddForce(player.sharedStatistics.rollStrength * inputDirection, ForceMode.VelocityChange);
            player.animationManager.animator.SetTrigger("roll");
            player.instanceStatistics.stamina -= rollCost;
        }

        player.animationManager.animator.SetFloat("speed", rigidbody.velocity.magnitude);
        player.instanceStatistics.stamina += Time.deltaTime * staminaRefillSpeed;
        player.instanceStatistics.stamina = Mathf.Min(player.instanceStatistics.stamina, player.sharedStatistics.maxStamina);
        player.hud.UpdateCoolDownBar(player.instanceStatistics.stamina / player.sharedStatistics.maxStamina);
    }

    private Vector3 GetInputDirection () {
        Vector3 inputDirection = Vector3.zero;
        foreach (InputType input in new InputType[] { InputType.Up, InputType.Down, InputType.Left, InputType.Right }) {
            if (InputManager.Instance.IsKey(input)) {
                inputDirection += directionVectors[input];
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
