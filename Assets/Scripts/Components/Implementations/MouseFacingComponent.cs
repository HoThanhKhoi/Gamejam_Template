using UnityEngine;

public class MouseFacingComponent : MonoBehaviour, IFacing
{
    private Vector2 mousePosition;
    private Vector2 direction;
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }
    public void SetFacingValue(float value)
    {
        mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        direction = (mousePosition - (Vector2)transform.position).normalized;
    }
    public void HandleFacing()
    {
        transform.right = direction;
    }
}

