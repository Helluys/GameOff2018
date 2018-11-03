using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour {

    [SerializeField] private PlayerStatistics statistics;
    [SerializeField] private PlayerMovement movement;

    private void Start () {
        statistics.ApplyStatistics(this);
        this.movement.OnStart(GetComponent<Rigidbody>());
    }

    private void Update () {
        this.movement.OnUpdate();
    }
}
