using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Shield : MonoBehaviour {

    [SerializeField] private Material sphereMat;

    private NavMeshObstacle obstacle;
    private Material mat;
    private new MeshRenderer renderer;

    private float sizeRatio = 1.33f;
    private float timeRatio = 4f;
    private float appearTime { get { return radius / 40; } }
    private float radius = 20;
    private float duration = 10;

    private float dissolveSize { get { return radius * sizeRatio; } }
    private float dissolveTime { get { return appearTime * timeRatio; } }

    private float currentRadius = 0;

    private void Update()
    {
        transform.position = GameManager.instance.GetPlayer().transform.position;
    }

    public void Init(float radius,float duration, Color color)
    {
        this.duration = duration;
        this.radius = radius;
        mat = new Material(sphereMat);
        renderer = GetComponent<MeshRenderer>();
        renderer.material = mat;
        mat.SetColor("_Color", color);
        SphereAppear();
        LeanTween.delayedCall(duration, SphereDisappear);
        obstacle = GetComponent<NavMeshObstacle>();
    }

    private void SphereAppear()
    {
        UpdateDissolveValue(0);
        LeanTween.scale(gameObject, Vector3.one * radius, appearTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateMatRadius, 1, radius, appearTime).setEaseOutSine();
    }

    private void SphereDisappear()
    {
        obstacle.enabled = false;
        LeanTween.scale(gameObject, Vector3.one * dissolveSize, dissolveTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateMatRadius, radius, dissolveSize, dissolveTime).setEaseOutSine();
        LeanTween.value(gameObject, UpdateDissolveValue, 0, 1, dissolveTime).setEaseOutSine().setOnComplete(() => Destroy(gameObject));
    }

    private void UpdateMatRadius(float radius)
    {
        mat.SetFloat("_Radius", radius);
        currentRadius = radius;
    }

    private void UpdateDissolveValue(float dissolve)
    {
        mat.SetFloat("_DissolveValue", dissolve);
    }
}
