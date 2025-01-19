using UnityEngine;

public class ShowHideGOComponent : MonoBehaviour, IShowHide
{
    public GameObject GO;
    public void Show()
    {
        Debug.Log("Show Indicator");
        GO.SetActive(true);
    }
    public void Hide()
    {
        Debug.Log("Hide Indicator");
        GO.SetActive(false);
    }

}
