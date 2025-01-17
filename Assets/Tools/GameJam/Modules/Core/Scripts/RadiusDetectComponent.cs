using UnityEngine;

public class RadiusDetectComponent : MonoBehaviour, IRadiusDetect
{
    public string tags = "Player";
    private bool isDetected = false;

    public bool IsDetected()
    {
        return isDetected;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(tags))
        {
            isDetected = true;
        }
    }
}
