using UnityEngine;

public class Enemy : MonoBehaviour {

    [SerializeField] private AIBehaviour movementBehaviour;
    [SerializeField] private AIBehaviour combatBehaviour;

    private Coroutine movementCoroutine;
    private Coroutine combatCoroutine;

    private void Start () {
        this.movementBehaviour.OnStart(this.gameObject);
        this.movementCoroutine = StartCoroutine(this.movementBehaviour.Run());

        this.combatBehaviour.OnStart(this.gameObject);
        this.combatCoroutine = StartCoroutine(this.combatBehaviour.Run());
    }
}
