using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private IShowHide showHideIndicator;

    private void Start()
    {
        showHideIndicator = GetComponent<IShowHide>();

        showHideIndicator.Hide();
    }
}
