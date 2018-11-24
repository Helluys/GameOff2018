using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SwarmTrackerBehaviour", menuName = "Game data/AI/Movement/Swarm tracker")]
public class SwarmTrackerBehaviour : EnemyBehaviour {

    public LayerMask terrainMask;

    [SerializeField] private float updateDelay = 0.1f;
    [SerializeField] private float updateRange = 0.1f;

    private Enemy enemy;
    private NavMeshAgent navMeshAgent;
    private Transform trackedObject;
    private Swarm swarm;

    private Coroutine updateCoroutine;
    private Coroutine groundedCoroutine;

    public override void OnStart (Enemy enemy) {
        this.enemy = enemy;

        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

        trackedObject = GameManager.instance.GetPlayer().transform;

        swarm = GameManager.instance.GetComponent<Swarm>();
        if (swarm == null) {
            swarm = GameManager.instance.gameObject.AddComponent<Swarm>();
        }
        swarm.AddAgent(navMeshAgent);
        enemy.OnDeath += Enemy_OnDeath;
        enemy.OnDamage += Enemy_OnDamage;

        updateCoroutine = enemy.StartCoroutine(UpdateCoroutine());
        groundedCoroutine = enemy.StartCoroutine(GroundedCoroutine());
    }

    public override IEnumerator Run () {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);

        while (true) {
            if (navMeshAgent.enabled) {
                if (Vector3.SqrMagnitude(previousTargetPosition - trackedObject.position) > updateRange) {
                    navMeshAgent.SetDestination(trackedObject.position);
                    previousTargetPosition = trackedObject.position;
                }
            }

            yield return new WaitForSeconds(updateDelay);
        }
    }

    private IEnumerator GroundedCoroutine () {
        while(true) {
            if (!navMeshAgent.enabled && IsGrounded()) {
                enemy.rigidbody.velocity = Vector3.zero;
                enemy.rigidbody.isKinematic = true;
                navMeshAgent.enabled = true;
            }
            yield return null;
        }
    }

    private bool IsGrounded () {
        RaycastHit info;
        if (Physics.Raycast(enemy.transform.position, Vector3.down, out info, 5, terrainMask)) {
            return info.distance < 1;
        }
        return false;
    }

    private IEnumerator UpdateCoroutine () {
        while (true) {
            swarm.UpdateNextPosition(navMeshAgent);
            if (navMeshAgent.velocity.magnitude > 0.1f) {
                enemy.transform.rotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
            }
            enemy.animationManager.animator.SetFloat("speed", navMeshAgent.velocity.magnitude);

            yield return null;
        }
    }

    private void Enemy_OnDamage (Enemy enemy, float amount) {
        navMeshAgent.enabled = false;
    }

    private void Enemy_OnDeath (Enemy obj) {
        enemy.StopCoroutine(updateCoroutine);
        swarm.RemoveAgent(navMeshAgent);
    }
}
