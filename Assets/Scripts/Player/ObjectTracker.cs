using UnityEngine;

public class ObjectTracker : MonoBehaviour {

    [SerializeField] private Transform trackedObject;
    private Vector3 positionDelta;

    // Use this for initialization
    private void Start () {
        if (this.trackedObject == null) {
            Destroy(this);
        } else {
            this.positionDelta = this.transform.position - this.trackedObject.position;
        }
    }

    // Update is called once per frame
    private void Update () {
        this.transform.position = this.trackedObject.position + this.positionDelta;
    }
}
