using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "TrackerBehaviour", menuName = "Game data/AI/Movement/TrackerBehaviour")]
public class TrackerBehaviour : EnemyBehaviour {

    [SerializeField] private float updateDelay = 0.1f;
    [SerializeField] private float updateRange = 0.1f;

    private NavMeshAgent navMeshAgent;
    private Transform trackedObject;

    public override void OnStart (Enemy enemy) {
        navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        trackedObject = GameManager.instance.GetPlayer().transform;
    }

    public override IEnumerator Run () {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);

        while (true) {
            if (Vector3.SqrMagnitude(previousTargetPosition - trackedObject.position) > updateRange) {
                navMeshAgent.SetDestination(trackedObject.position);
                previousTargetPosition = trackedObject.position;
            }

            yield return new WaitForSeconds(updateDelay);
        }
    }
}
