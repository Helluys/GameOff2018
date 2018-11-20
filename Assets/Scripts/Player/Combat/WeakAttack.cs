using UnityEngine;

public class WeakAttack : Attack {

    [SerializeField] private new BoxCollider collider;
    [SerializeField] private float detectionRadius = 20;
    [SerializeField] private float speed = 30;
    [SerializeField] private LayerMask mask;

    [SerializeField] private Material weakAttackMat;

    private Material mat;
    private new MeshRenderer renderer;

    private Transform ennemyTarget;
    private Vector3 direction;
    private bool init = false;
    private bool aimBot = true;
    private float lifeTime = 10;

    public void Init (float damageAmount, bool aimBot, Vector3 direction,Color color) {

        mat = new Material(weakAttackMat);
        renderer = GetComponent<MeshRenderer>();
        renderer.material = mat;
        mat.SetColor("_Color", color);

        Destroy(gameObject, lifeTime);

        this.aimBot = aimBot;
        this.damageAmount = damageAmount;

        if (aimBot) {
            ennemyTarget = GetClosestEnnemy();
            if (ennemyTarget == null) {
                init = true;
                this.direction = direction;
                aimBot = false;
            } else {
                init = true;
                this.direction = (ennemyTarget.position - transform.position).normalized;
            }
        }
    }

    private void Update () {
        if (init)
            transform.position += direction * Time.deltaTime * speed;
    }

    public override void OnEnter (Enemy enemy) {
        base.OnEnter(enemy);
        Destroy(gameObject);
    }

    private Transform GetClosestEnnemy () {
        float radius = 1;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius, mask);
        while (colliders.Length < 1 && radius < detectionRadius) {
            radius++;
            colliders = Physics.OverlapSphere(transform.position, radius, mask);
        }

        if (colliders.Length < 1)
            return null;

        float minDist = Mathf.Infinity;
        int index = -1;

        for (int i = 0; i < colliders.Length; i++) {
            float dist = Vector3.Distance(transform.position, colliders[i].transform.position);
            if (dist < minDist) {
                minDist = dist;
                index = i;
            }
        }
        return colliders[index].transform;
    }

    private void OnSelectedDrawGizmos () {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
