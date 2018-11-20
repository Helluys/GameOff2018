using UnityEngine;

public class StrongAttack : Attack {

    [SerializeField] private Material sphereMat;
    private Material mat;
    private new MeshRenderer renderer;
    private new SphereCollider collider;

    private float sizeRatio = 1.33f;
    private float timeRatio = 4f;
    private float attackTime { get { return attackSize / 40; } }
    private float attackSize = 20;

    private float dissolveSize { get { return attackSize * sizeRatio; } }
    private float dissolveTime { get { return attackTime * timeRatio; } }

    private float currentRadius = 0;

    public void Init (float damageAmount, float attackSize,Color color) {

        this.damageAmount = damageAmount;
        this.attackSize = attackSize;

        mat = new Material(sphereMat);
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<SphereCollider>();
        renderer.material = mat;
        mat.SetColor("_Color", color);
        SphereAppear();
    }

    private void SphereAppear () {
        UpdateDissolveValue(0);
        LeanTween.scale(gameObject, Vector3.one * attackSize, attackTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateMatRadius, 1, attackSize, attackTime).setEaseOutSine();
        LeanTween.delayedCall(attackTime, SphereDisappear);
    }

    private void SphereDisappear () {
        collider.radius = 0;
        LeanTween.scale(gameObject, Vector3.one * dissolveSize, dissolveTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateMatRadius, attackSize, dissolveSize, dissolveTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateDissolveValue, 0, 1, dissolveTime).setEaseOutSine().setOnComplete(() => Destroy(gameObject));
    }

    public override void OnEnter (Enemy enemy) {
        base.OnEnter(enemy);
        enemy.rigidbody.AddTorque(Vector3.one * Random.value, ForceMode.VelocityChange);
    }

    private void UpdateMatRadius (float radius) {
        mat.SetFloat("_Radius", radius);
        currentRadius = radius;
    }

    private void UpdateDissolveValue (float dissolve) {
        mat.SetFloat("_DissolveValue", dissolve);
    }
}
