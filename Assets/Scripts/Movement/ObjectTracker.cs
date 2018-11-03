using UnityEngine;

public class ObjectTracker : MonoBehaviour {

    [SerializeField] private Transform trackedObject;
    private Vector3 positionDelta;

    // Use this for initialization
    void Start () {
        if (trackedObject == null) {
            Destroy(this);
        } else {
            positionDelta = transform.position - trackedObject.position;
        }
    }

    // Update is called once per frame
    void Update () {
        transform.position = trackedObject.position + positionDelta;
    }
}
