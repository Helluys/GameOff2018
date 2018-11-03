using UnityEngine;
using System.Collections;
using UnityEngine.AI;

[CreateAssetMenu(fileName ="TrackerBehaviour", menuName ="Game data/AI/Movement/TrackerBehaviour")]
public class TrackerBehaviour : AIBehaviour {

    [SerializeField] private float updateDelay = 0.1f;
    [SerializeField] private float updateRange = 0.1f;
    private Transform trackedObject;
    private NavMeshAgent navMeshAgent;

    public override void OnStart (GameObject gameobject) {
        this.navMeshAgent = gameobject.GetComponent<NavMeshAgent>();
        this.trackedObject = GameManager.instance.GetPlayer().transform;
    }

    public override IEnumerator Run () {
        Vector3 previousTargetPosition = new Vector3(float.PositiveInfinity, float.PositiveInfinity);

        while (true) {
            if (Vector3.SqrMagnitude(previousTargetPosition - this.trackedObject.position) > this.updateRange) {
                this.navMeshAgent.SetDestination(this.trackedObject.position);
                previousTargetPosition = this.trackedObject.position;
            }

            yield return new WaitForSeconds(this.updateDelay);
        }
    }
}
