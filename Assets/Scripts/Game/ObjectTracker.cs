using UnityEngine;

public class ObjectTracker : MonoBehaviour {

    [SerializeField] private Transform trackedObject = null;
    private Vector3 positionDelta;

    // Use this for initialization
    private void Start () {
        if (trackedObject == null) {
            Debug.Log("No tracked object: deleting component");
            Destroy(this);
        } else {
            positionDelta = transform.position - trackedObject.position;
        }
    }

    // Update is called once per frame
    private void Update () {
        transform.position = trackedObject.position + positionDelta;
    }
}
