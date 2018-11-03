using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [SerializeField] private Transform trackedObject;
    [SerializeField] private float updateDelay = 0.1f;
    [SerializeField] private float updateRange = 0.1f;

    private NavMeshAgent navMeshAgent;
    private Coroutine updatePositionCoroutine;

    private void Start () {
        this.navMeshAgent = GetComponent<NavMeshAgent>();
        this.updatePositionCoroutine = StartCoroutine(FollowTarget(this.trackedObject));
    }

    private IEnumerator FollowTarget (Transform target) {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);

        while (true) {
            if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > this.updateRange) {
                this.navMeshAgent.SetDestination(target.position);
                previousTargetPosition = target.position;
            }

            yield return new WaitForSeconds(this.updateDelay);
        }
    }
}
