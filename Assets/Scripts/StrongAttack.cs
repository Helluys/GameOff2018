using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : MonoBehaviour {

    [SerializeField] private Material sphereMat;
    private Material mat;
    private new MeshRenderer renderer;
    private new SphereCollider collider;
    [SerializeField] private float effectTime = 5;
    [SerializeField] private float dissolveTime = 1;
    [SerializeField] private float effectSize = 20;
    [SerializeField] private float dissolveSize = 25;
    [SerializeField] private int damageAmount = 50;
    [SerializeField] private int knockbackStrength = 10;



    // Use this for initialization
    void Start () {

        mat = new Material(sphereMat);
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<SphereCollider>();
        renderer.material = mat;
        SphereAppear();
	}

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.agent.enabled = false;
            enemy.Damage(damageAmount);
        }
    }

    private void OnTriggerStay(Collider collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            enemy.agent.enabled = true;
        }
    }

    private void SphereAppear()
    {
        UpdateDissolveValue(0);
        LeanTween.scale(gameObject, Vector3.one * effectSize, effectTime);
        LeanTween.value(gameObject, UpdateMatRadius, 1, effectSize, effectTime);
        LeanTween.delayedCall(effectTime, SphereDissapear);
    }	

    private void SphereDissapear()
    {
        LeanTween.delayedCall(dissolveTime / 2, () => collider.radius = 0);
        LeanTween.scale(gameObject, Vector3.one * dissolveSize, dissolveTime);
        LeanTween.value(gameObject, UpdateMatRadius, effectSize, dissolveSize, dissolveTime);
        LeanTween.value(gameObject, UpdateDissolveValue, 0, 1, dissolveTime).setOnComplete(()=>Destroy(gameObject));
    }

    private void  UpdateMatRadius(float radius)
    {
        mat.SetFloat("_Radius", radius);
    }

    private void UpdateDissolveValue(float dissolve)
    {
        mat.SetFloat("_DissolveValue", dissolve);
    }
}
