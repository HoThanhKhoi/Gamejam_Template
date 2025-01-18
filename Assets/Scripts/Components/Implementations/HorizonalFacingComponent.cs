using UnityEngine;

public class HorizonalFacingComponent : MonoBehaviour, IHorizontalFacing
{
    public void FaceTo(float xValue)
    {
        if (xValue != 0)
        {
            xValue = xValue > 0 ? 1 : -1;
            transform.right = new Vector2(xValue, 0);
        }
    }
}
