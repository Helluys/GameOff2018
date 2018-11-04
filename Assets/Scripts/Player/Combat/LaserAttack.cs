using UnityEngine;

public class LaserAttack : MonoBehaviour {

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float damagePerSecond = 10f;
    [SerializeField] private Material mat;
    [SerializeField] private new CapsuleCollider collider;

    // Use this for initialization
    private void Start () {
        UpdateMatValue(0);
        LeanTween.delayedCall(duration,() =>Fade() );
    }

    private void Fade()
    {
        collider.enabled = false;
        LeanTween.value(gameObject, UpdateMatValue, 0, 1.0f,2.0f).setOnComplete(()=>Destroy(gameObject));
    }

    private void OnTriggerStay (Collider other) {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null) {
            enemy.Damage(this.damagePerSecond * Time.fixedDeltaTime);
        }
    }

    private void UpdateMatValue(float val)
    {
        mat.SetFloat("_DissolveValue", val);
    }
}
