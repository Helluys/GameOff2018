using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;

public class Swarm : MonoBehaviour {
    [SerializeField] private float targetFactor, directionFactor, avoidanceFactor, gamma;

    private List<NavMeshAgent> agents = new List<NavMeshAgent>();

    public void AddAgent (NavMeshAgent agent) {
        agents.Add(agent);
    }

    public void RemoveAgent (NavMeshAgent agent) {
        agents.Remove(agent);
    }

    public void UpdateNextPosition (NavMeshAgent agent) {
        Vector3 nextDelta = agent.nextPosition - agent.transform.position;

        nextDelta = targetFactor * nextDelta + directionFactor * GetSwarmDirection(agent) + avoidanceFactor * GetAvoidanceDirection(agent);
        nextDelta = Vector3.ClampMagnitude(nextDelta, agent.speed * Time.deltaTime);

        if (!CustomUtils.IsValidVector(nextDelta))
            return;

        agent.nextPosition = agent.transform.position + nextDelta;
        agent.transform.position = agent.nextPosition;
    }

    private Vector3 GetSwarmDirection (NavMeshAgent currentAgent) {
        Vector3 swarmDirection = Vector3.zero;
        foreach (NavMeshAgent agent in agents) {
            if (agent != currentAgent) {
                Vector3 delta = currentAgent.nextPosition - agent.nextPosition;
                swarmDirection += (1f / Mathf.Pow(delta.magnitude, gamma)) * agent.velocity;
            }
        }
        return (1f / agents.Count) * swarmDirection;
    }

    private Vector3 GetAvoidanceDirection(NavMeshAgent currentAgent)
    {
        Vector3 avoidanceDirection = Vector3.zero;
        foreach (NavMeshAgent agent in agents)
        {
            if (agent != currentAgent)
            {
                Vector3 delta = currentAgent.nextPosition - agent.nextPosition;
                avoidanceDirection += (1f / Mathf.Pow(delta.magnitude, gamma)) * delta;
            }
        }
        return (1f / agents.Count) * avoidanceDirection;
    }
}
