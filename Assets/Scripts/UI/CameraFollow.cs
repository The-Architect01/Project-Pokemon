using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform Target;
    public float SmoothSpeed = .125f;
    public Vector3 Offset;

    private void LateUpdate() {
        transform.position = Target.position + Offset;
    }
}
