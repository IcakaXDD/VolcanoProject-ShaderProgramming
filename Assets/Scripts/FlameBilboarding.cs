using UnityEngine;

public class FlameBilboarding : MonoBehaviour
{

    Camera cam;
    Vector3 worldUp = Vector3.up;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = Camera.main;
    }

    private void LateUpdate()
    {
        // Forward vector from the flame to the camera (Y locked)
        Vector3 toCam = cam.transform.position - transform.position;
        toCam.y = 0; // lock Y-axis
        Vector3 forward = toCam.normalized;

        // Use cross product to get right and up directions
        Vector3 up = Vector3.up;
        Vector3 right = Vector3.Cross(up, forward).normalized;
        Vector3 adjustedUp = Vector3.Cross(forward, right);

        // Create rotation from forward and up vectors
        transform.rotation = Quaternion.LookRotation(forward, adjustedUp);

    }
}
