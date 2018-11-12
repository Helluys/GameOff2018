using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongAttack : Attack {

    [SerializeField] private Material sphereMat;
    private Material mat;
    private new MeshRenderer renderer;
    private new SphereCollider collider;
    [SerializeField] private float effectTime = 5;
    [SerializeField] private float dissolveTime = 1;
    [SerializeField] private float effectSize = 20;
    [SerializeField] private float dissolveSize = 25;

    private float currentRadius = 0;

    // Use this for initialization
    void Start () {

        mat = new Material(sphereMat);
        renderer = GetComponent<MeshRenderer>();
        collider = GetComponent<SphereCollider>();
        renderer.material = mat;
        SphereAppear();
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
        collider.radius = 0;
        LeanTween.scale(gameObject, Vector3.one * dissolveSize, dissolveTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateMatRadius, effectSize, dissolveSize, dissolveTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateDissolveValue, 0, 1, dissolveTime).setEaseOutSine().setOnComplete(()=>Destroy(gameObject));
    }

    public override void OnEnter(Enemy enemy)
    {
        base.OnEnter(enemy);
        enemy.rigidbody.AddTorque(Vector3.one * Random.value, ForceMode.VelocityChange);
    }

    private void  UpdateMatRadius(float radius)
    {
        mat.SetFloat("_Radius", radius);
        currentRadius = radius;
    }

    private void UpdateDissolveValue(float dissolve)
    {
        mat.SetFloat("_DissolveValue", dissolve);
    }
}
