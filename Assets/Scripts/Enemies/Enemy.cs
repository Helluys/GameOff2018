using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    [SerializeField] private Transform trackedObject;
    [SerializeField] private float updateDelay = 0.1f;
    [SerializeField] private float updateRange = 0.1f;

    private NavMeshAgent navMeshAgent;
    private Coroutine updatePositionCoroutine;

    void Start () {
        navMeshAgent = GetComponent<NavMeshAgent>();
        updatePositionCoroutine = StartCoroutine(FollowTarget(trackedObject));
    }
    
    private IEnumerator FollowTarget (Transform target) {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);

        while (true) {
            if (Vector3.SqrMagnitude(previousTargetPosition - target.position) > updateRange) {
                navMeshAgent.SetDestination(target.position);
                previousTargetPosition = target.position;
                Debug.Log("update");
            }

            yield return new WaitForSeconds(updateDelay);
        }
    }
}
