using UnityEngine;
using Utils;
using static UnityEngine.Rendering.DebugUI;

public class MouseFacingComponent : MonoBehaviour, IFacing
{
    private Vector2 mousePosition;
    private Vector2 direction;
    private Camera mainCamera;
    private IFacing facingComponent;
    private float facingValue;

    private void Start()
    {
        mainCamera = Camera.main;
        facingComponent = ComponentCache.GetInterface<IFacing>(gameObject);
    }

    public float GetMouseRelativePosition()
    {
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found. Ensure the camera is tagged 'MainCamera'.");
            return 0f;
        }

        // Get the mouse position in screen space and set the correct z-axis value
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);

        // Convert the screen position to world space
        Vector2 mouseWorldPosition = mainCamera.ScreenToWorldPoint(mouseScreenPosition);

        Debug.Log($"Mouse World Position: {mouseWorldPosition} | Object Position: {transform.position}");

        // Calculate the relative position (mouse x - object x)
        float relativePosition = mouseWorldPosition.x - transform.position.x;

        return relativePosition;
    }


    public void SetFacingValue(float value = 0.0f)
    {
        facingValue = GetMouseRelativePosition();
    }

    public void HandleFacing()
    {
        transform.right = new Vector2(-facingValue, 0);
    }

    private void Update()
    {
        SetFacingValue();
        HandleFacing();
    }
}

