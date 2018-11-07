using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereTest : MonoBehaviour {

    [SerializeField] private Material sphereMat;
    private Material mat;
    private new MeshRenderer renderer;
    [SerializeField] private float effectTime = 5;
    [SerializeField] private float dissolveTime = 1;
    [SerializeField] private float effectSize = 20;
    [SerializeField] private float dissolveSize = 25;

	// Use this for initialization
	void Start () {
        mat = new Material(sphereMat);
        renderer = GetComponent<MeshRenderer>();
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
