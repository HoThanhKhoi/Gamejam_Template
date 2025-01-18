using UnityEngine;

public class CircleCastGroundCheck2DComponent : MonoBehaviour, ICheckGrounded
{
    [SerializeField] private float circleCastRadius = 0.2f;      // Radius of the circle
    [SerializeField] private float circleCastDistance = 0.1f;    // How far down we cast
    [SerializeField] private LayerMask groundLayer;              // Layers considered "ground"

    public bool IsGrounded()
    {
        // Perform a circle cast downward to check for ground
        RaycastHit2D hit = Physics2D.CircleCast(
            origin: transform.position,
            radius: circleCastRadius,
            direction: Vector2.down,
            distance: circleCastDistance,
            layerMask: groundLayer
        );

        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        // Visualize the CircleCast in the editor
        Gizmos.color = Color.green;

        Vector3 origin = transform.position;
        Vector3 endPosition = origin + Vector3.down * circleCastDistance;

        // Draw the circle at the start position
        Gizmos.DrawWireSphere(origin, circleCastRadius);

        // Draw the circle at the end position of the cast
        Gizmos.DrawWireSphere(endPosition, circleCastRadius);

        // Connect the two circles with a line
        Gizmos.DrawLine(origin, endPosition);
    }
}
