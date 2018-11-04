using UnityEngine;

public class LaserAttack : MonoBehaviour {

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float damagePerSecond = 10f;

    // Use this for initialization
    private void Start () {
        Destroy(this.gameObject, this.duration);
    }

    private void OnTriggerStay (Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(this.damagePerSecond * Time.fixedDeltaTime);
        }

    }
}
