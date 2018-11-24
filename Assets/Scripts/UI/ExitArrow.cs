using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ExitArrow : MonoBehaviour {

    [SerializeField] private ExitPortal exitPortal;

    private Image arrowImage;

    private void Start () {
        arrowImage = GetComponent<Image>();
    }

    private void Update () {
        Vector2 cameraTargetPoint = Camera.main.WorldToScreenPoint(exitPortal.transform.position);
        if (!exitPortal.active || Camera.main.pixelRect.Contains(cameraTargetPoint)) {
            arrowImage.enabled = false;
        } else {
            arrowImage.enabled = true;

            // Recenter
            cameraTargetPoint = (cameraTargetPoint - 0.5f * Camera.main.pixelRect.size).normalized;
            arrowImage.transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, cameraTargetPoint));
        }
    }
}
