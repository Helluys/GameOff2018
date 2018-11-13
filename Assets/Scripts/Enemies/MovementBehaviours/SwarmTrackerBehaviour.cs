using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "SwarmTrackerBehaviour", menuName = "Game data/AI/Movement/Swarm tracker")]
public class SwarmTrackerBehaviour : EnemyBehaviour {

    [SerializeField] private float updateDelay = 0.1f;
    [SerializeField] private float updateRange = 0.1f;

    private Swarm swarm;

    private Enemy enemy;
    private NavMeshAgent navMeshAgent;
    private Transform trackedObject;
    private Coroutine updateCoroutine;

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

        updateCoroutine = enemy.StartCoroutine(UpdateCoroutine());
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

    private void Enemy_OnDeath (Enemy obj) {
        swarm.RemoveAgent(navMeshAgent);
        enemy.StopCoroutine(updateCoroutine);
    }
}
