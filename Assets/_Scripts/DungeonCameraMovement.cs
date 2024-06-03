using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonCameraMovement : MonoBehaviour
{
    public bool canMoveCamera = true;
    [SerializeField] private float PanSpeed = 20f;
    [SerializeField] private float ZoomSpeedTouch = 0.1f;

    [SerializeField] float[] BoundsX = new float[] { -10f, 5f };
    [SerializeField] float[] BoundsZ = new float[] { -18f, -4f };
    [SerializeField] float[] ZoomBounds = new float[] { 10f, 85f };

    private Camera cam;

    private bool panActive;
    private Vector3 lastPanPosition;
    private int panFingerId; // Touch mode only

    [SerializeField] private bool zoomActive;
    private Vector2[] lastZoomPositions; // Touch mode only


    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    { 
        HandleTouch();
    }

    void HandleTouch()
    {
        switch (Input.touchCount)
        {

            case 1: // Panning
                zoomActive = false;

                // If the touch began, capture its position and its finger ID.
                // Otherwise, if the finger ID of the touch doesn't match, skip it.
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began)
                {
                    lastPanPosition = touch.position;
                    panFingerId = touch.fingerId;
                    panActive = true;
                }
                else if (touch.fingerId == panFingerId && touch.phase == TouchPhase.Moved)
                {
                    PanCamera(touch.position);
                }
                break;

            case 2: // Zooming
                panActive = false;

                Vector2[] newPositions = new Vector2[] { Input.GetTouch(0).position, Input.GetTouch(1).position };
                if (!zoomActive)
                {
                    lastZoomPositions = newPositions;
                    zoomActive = true;
                }
                else
                {
                    // Zoom based on the distance between the new positions compared to the 
                    // distance between the previous positions.
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastZoomPositions[0], lastZoomPositions[1]);
                    float offset = newDistance - oldDistance;

                    ZoomCamera(offset, ZoomSpeedTouch);

                    lastZoomPositions = newPositions;
                }
                break;

            default:
                panActive = false;
                zoomActive = false;
                break;
        }
    }

    void PanCamera(Vector3 newPanPosition)
    {
        if (!canMoveCamera) return;

        if (!panActive)
        {
            return;
        }
        
        // Translate the camera position based on the new input position
        Vector3 offset = cam.ScreenToViewportPoint(lastPanPosition - newPanPosition);
        Vector3 move = new Vector3(offset.x, 0, offset.y) * PanSpeed;
        Vector3 targetPosition = transform.position + move;
        targetPosition.x = Mathf.Clamp(targetPosition.x, BoundsX[0], BoundsX[1]);
        targetPosition.z = Mathf.Clamp(targetPosition.z, BoundsZ[0], BoundsZ[1]);
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 10f);

        lastPanPosition = newPanPosition;
    }

    void ZoomCamera(float offset, float speed)
    {
        if (!zoomActive || offset == 0)
        {
            return;
        }

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - (offset * speed), ZoomBounds[0], ZoomBounds[1]);
    }
}
