using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour {

    public static class Direction {
        public const int UP = 0;
        public const int DOWN = 1;
        public const int LEFT = 2;
        public const int RIGHT = 3;
    }

    private const float MIN_MOVEMENT_THRESHOLD = 0.1f;
    private readonly Quaternion INPUT_TO_WORLD = Quaternion.FromToRotation(Vector3.forward, (Vector3.forward + Vector3.left).normalized);

    [SerializeField] private KeyCode[] keys;
    [SerializeField] private float acceleration;

    private new Rigidbody rigidbody;

    // Use this for initialization
    void Start () {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 inputDirection = Vector3.zero;
        if (Input.GetKey(keys[Direction.UP])) {
            inputDirection += Vector3.forward;
        }

        if (Input.GetKey(keys[Direction.DOWN])) {
            inputDirection += Vector3.back;
        }

        if (Input.GetKey(keys[Direction.LEFT])) {
            inputDirection += Vector3.left;
        }

        if (Input.GetKey(keys[Direction.RIGHT])) {
            inputDirection += Vector3.right;
        }

        if (inputDirection.magnitude > MIN_MOVEMENT_THRESHOLD) {
            inputDirection.Normalize();
        }
        
        Vector3 movement = InputToMovement(inputDirection);

        rigidbody.AddForce (acceleration * movement, ForceMode.Acceleration);
        if (movement.magnitude > MIN_MOVEMENT_THRESHOLD) {
            transform.rotation = Quaternion.LookRotation(movement);
        }
    }

    private Vector3 InputToMovement (Vector3 inputDirection) {
        return INPUT_TO_WORLD  * inputDirection;
    }

    public void SetKey (int direction, KeyCode key) {
        this.keys[direction] = key;
    }
}
