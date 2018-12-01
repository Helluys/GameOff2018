using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "JumperBehaviour", menuName = "Game data/AI/Movement/Jumper")]
public class JumperBehaviour : EnemyBehaviour {

    [SerializeField] private GameObject landAttackPrefab;
    [SerializeField] private GameObject aoeTargetPrefab;
    [SerializeField] private float jumpingDistance;
    [SerializeField] private float jumpTime;
    [SerializeField] private AnimationCurve jumpHeightCurve;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private LayerMask playerLayer;

    private Player player;
    private Enemy enemy;

    private NavMeshPath path;
    private int areaMask;

    private GameObject aoeTarget;

    private Vector3 jumpStartPosition, jumpTargetPosition, jumpStartDirection;
    private float jumpStartTime;
    private WaitUntil waitForJumping;
    private WaitUntil waitForIdle;

    public enum State {
        IDLE, JUMPING
    }

    public State state { get; private set; }

    public override void OnStart (Enemy enemy) {
        player = GameManager.instance.GetPlayer();
        this.enemy = enemy;

        state = State.IDLE;
        waitForJumping = new WaitUntil(() => state.Equals(State.JUMPING));
        waitForIdle = new WaitUntil(() => state.Equals(State.IDLE));

        path = new NavMeshPath();
        areaMask = 1 << NavMesh.GetAreaFromName("Walkable");

        enemy.animationManager.OnAnimationEvent += AnimationManager_OnAnimationEvent;
        enemy.animationManager.OnEnter += AnimationManager_OnEnter;
        enemy.OnDeath += Enemy_OnDeath;
    }

    public override IEnumerator Run () {
        while (true) {
            switch (state) {
                case State.IDLE:
                    yield return new WaitForSeconds(jumpCooldown * (Random.value * 0.2f + 0.9f));
                    enemy.animationManager.animator.SetTrigger("Jump");

                    yield return waitForJumping;
                    while (!Jump())
                        yield return null;

                    break;
                case State.JUMPING:
                    enemy.rigidbody.isKinematic = true;
                    float jumpTimeRatio = (Time.time - jumpStartTime) / jumpTime;
                    enemy.transform.position = Vector3.Lerp(jumpStartPosition, jumpTargetPosition, jumpTimeRatio) + jumpHeightCurve.Evaluate(jumpTimeRatio) * Vector3.up;
                    enemy.transform.rotation = Quaternion.LookRotation(Vector3.Lerp(jumpStartDirection, jumpTargetPosition - jumpStartPosition, jumpTimeRatio));

                    if (jumpTimeRatio > 1) {
                        enemy.rigidbody.isKinematic = false;
                        Fall();
                        yield return waitForIdle;
                    }

                    break;
            }
            yield return null;
        }
    }

    public bool FindLandingSpot () {
        NavMeshHit navMeshHit = new NavMeshHit();
        if (NavMesh.CalculatePath(enemy.transform.position, player.transform.position, areaMask, path)) {
            // Go through path backwards to find landing spot at desired distance (find point closest to player as possible)
            for (int i = path.corners.Length - 1; i >= 0; i--) {
                if ((path.corners[i] - enemy.transform.position).magnitude < jumpingDistance) {
                    jumpTargetPosition = path.corners[i];

                    return true;
                } else if (i > 0 && (path.corners[i - 1] - enemy.transform.position).magnitude < jumpingDistance) {
                    float previousCornerDistance = (path.corners[i - 1] - enemy.transform.position).magnitude;
                    float deltaCornerDistance = (path.corners[i] - path.corners[i - 1]).magnitude;
                    float cos_theta = Mathf.Cos(Vector3.Angle(path.corners[i - 1] - enemy.transform.position, path.corners[i] - path.corners[i - 1]) * Mathf.Deg2Rad);
                    float jumpDistSq = jumpingDistance * jumpingDistance;
                    float prevDistSq = previousCornerDistance * previousCornerDistance;

                    // Because math
                    float x = (previousCornerDistance / deltaCornerDistance) * (Mathf.Sqrt(cos_theta - 1 + (jumpDistSq / prevDistSq)) - cos_theta);

                    // Protect from bugs
                    if (float.IsNaN(x))
                        x = 1f;

                    jumpTargetPosition = path.corners[i - 1] + (path.corners[i] - path.corners[i - 1]) * Mathf.Clamp01(x);

                    return true;
                }
            }
        } else if (NavMesh.FindClosestEdge (enemy.transform.position, out navMeshHit, areaMask)) {
            jumpStartPosition = navMeshHit.position;

            return true;
        }

        return false;
    }

    private bool Jump () {
        if (FindLandingSpot()) {
            aoeTarget = Instantiate(aoeTargetPrefab, jumpTargetPosition, Quaternion.identity);
            aoeTarget.transform.localScale = Vector3.one * enemy.sharedStatistics.attackRange;

            jumpStartPosition = enemy.transform.position;
            jumpStartDirection = enemy.transform.forward;
            jumpStartTime = Time.time;

            return true;
        }

        return false;
    }

    private void Fall () {
        Destroy(aoeTarget);
        aoeTarget = null;

        enemy.animationManager.animator.SetTrigger("Fall");
        enemy.transform.position = Vector3.ProjectOnPlane(enemy.transform.position, Vector3.up);

        Instantiate(landAttackPrefab, enemy.transform.position, Quaternion.identity);

        Collider[] array = Physics.OverlapSphere(enemy.transform.position, enemy.sharedStatistics.attackRange, playerLayer.value);
        if (array.Length > 0) {
            player.Damage(enemy.sharedStatistics.attackDamage);
            player.GetComponent<Rigidbody>().AddForce(3f * enemy.sharedStatistics.attackDamage * (player.transform.position - enemy.transform.position).normalized, ForceMode.Impulse);
        }
    }

    private void AnimationManager_OnEnter (string clipName) {
        if (clipName.Equals("Idle"))
            state = State.IDLE;
    }

    private void AnimationManager_OnAnimationEvent (string eventName) {
        if (eventName.Equals("Jump"))
            state = State.JUMPING;
    }

    private void Enemy_OnDeath (Enemy enemy) {
        if (aoeTarget != null) {
            Destroy(aoeTarget);
            aoeTarget = null;
        }
    }
}
