using UnityEngine;

public class HorizonalFacingComponent : MonoBehaviour, IFacing
{
    private float xValue;
    // value >0 face right, value <0 face left
    public void SetFacingValue(float value)
    {
        xValue = value;
    }

    public void HandleFacing()
    {
        if (xValue != 0)
        {
            xValue = xValue > 0 ? -1 : 1;
            transform.right = new Vector2(xValue, 0);
        }
    }
}
