using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float dragMultiplierRotate = 0;
    public float dragMultiplierTranslate = 0;
    public float scrollMultiplierTranslate = 0;
    private Vector3 dragOrigin;
    private Vector3 latestDragPosition;

    enum e_Mousebutton
    {
        NONE,
        LEFT,
        RIGHT
    }

    e_Mousebutton buttonPressed = e_Mousebutton.NONE;

    void Update()
    {
        //handle mouse wheel scroll in and out
        float scrollDistance = Input.GetAxis("Mouse ScrollWheel");

        if (scrollDistance != 0)
        {
            transform.Translate(new Vector3(0, 0, scrollDistance * scrollMultiplierTranslate), Space.Self);
        }

        //check if the left or right mouse button are held down
        //LEFT moves camera
        //RIGHT rotates camera
        if (buttonPressed == e_Mousebutton.NONE && Input.GetMouseButtonDown(1))
        {
            dragOrigin = Input.mousePosition;
            buttonPressed = e_Mousebutton.RIGHT;
            return;
        }
        else if (buttonPressed == e_Mousebutton.NONE && Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            buttonPressed = e_Mousebutton.LEFT;
            return;
        }

        if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            buttonPressed = e_Mousebutton.NONE;
            return;
        }

        latestDragPosition = Input.mousePosition;
        Vector3 dragDistance = new Vector3(0,0,0);

        if (buttonPressed == e_Mousebutton.RIGHT)
        {
            dragDistance = new Vector3((latestDragPosition.y - dragOrigin.y), (latestDragPosition.x - dragOrigin.x), 0) * dragMultiplierRotate;
            transform.Rotate(dragDistance);
        }
        else if (buttonPressed == e_Mousebutton.LEFT)
        {
            dragDistance = new Vector3((latestDragPosition.x - dragOrigin.x) * -dragMultiplierTranslate, (latestDragPosition.y - dragOrigin.y) * -dragMultiplierTranslate, 0);

            transform.Translate(dragDistance, Space.Self);

        }

        dragOrigin = latestDragPosition;
    }
}
