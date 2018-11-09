using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAttack : MonoBehaviour {

    [SerializeField] private new BoxCollider collider;
    [SerializeField] private float detectionRadius = 20;
    [SerializeField] private float speed = 30;
    [SerializeField] private LayerMask mask;
    [SerializeField] private int damageAmount = 20;

    private Transform ennemyTarget;
    private Vector3 direction;
    private bool init = false;
    private bool aimBot = true;
    private float lifeTime = 10;


	// Use this for initialization
	public void Init (bool aimBot, Vector3 direction) {

        Destroy(gameObject, 10);
        this.aimBot = aimBot;
        if (aimBot)
        {
            ennemyTarget = GetClosestEnnemy();
            if (ennemyTarget != null)
            {
                init = true;
                this.direction = (ennemyTarget.position - transform.position).normalized;
                return;
            }
            aimBot = false;
        }
        this.direction = direction;
        init = true;
	}

    void Update () {
        if (!init)
            return;
        // Tête chercheuse ?
        /*if (aimBot)
          {
              if (ennemyTarget != null)
                  direction = (ennemyTarget.position - transform.position).normalized;  
          }
        */
        transform.position += direction * Time.deltaTime * speed;
	}

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.Damage(damageAmount);
        }
        
        Destroy(gameObject);
    }

    private Transform GetClosestEnnemy()
    {
        float radius = 1;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius,mask);
        while (colliders.Length < 1 && radius < detectionRadius)
        {
            radius++;
            colliders = Physics.OverlapSphere(transform.position, radius,mask);
        }

        if (colliders.Length < 1)
            return null;

        float minDist = Mathf.Infinity;
        int index = -1;

        for (int i= 0;i < colliders.Length;i++)
        {
            float dist = Vector3.Distance(transform.position, colliders[i].transform.position);
            if ( dist < minDist)
            {
                minDist = dist;
                index = i;
            }
        }
        return colliders[index].transform;
    }

    private void OnSelectedDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
