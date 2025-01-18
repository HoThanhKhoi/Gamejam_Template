using UnityEngine;

public class BoxCast2DWallCheck : MonoBehaviour, IWallCheck
{
    [Header("BoxCast Settings")]
    [SerializeField] private Vector2 boxSize = new Vector2(0.5f, 1.0f);
    [SerializeField] private float castDistance = 0.1f;
    [SerializeField] private LayerMask wallLayer;

    [Header("Entity Facing")]
    [SerializeField] private bool isFacingRight = true;

    public int GetWallDirection()
    {
        // Determine the direction to cast
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        // Perform the BoxCast
        RaycastHit2D hit = Physics2D.BoxCast(
            origin: transform.position,
            size: boxSize,
            angle: 0f,
            direction: direction,
            distance: castDistance,
            layerMask: wallLayer
        );

        // If a wall is detected, return 1 for right, -1 for left
        if (hit.collider != null)
        {
            return isFacingRight ? 1 : -1;
        }

        // No wall detected
        return 0;
    }

    public bool IsFacingWall()
    {
        return GetWallDirection() != 0;
    }

    private void OnDrawGizmosSelected()
    {
        if (!enabled) return;

        Gizmos.color = Color.cyan;

        // Current center of the box (start position)
        Vector2 origin = transform.position;
        // Determine the direction
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;
        // Where the box will be after traveling castDistance
        Vector2 endPosition = origin + direction * castDistance;

        // Convert boxSize to half extents since Gizmos.DrawWireCube uses full size
        Vector2 halfSize = boxSize * 0.5f;

        // Draw the box at the start position
        Gizmos.DrawWireCube(origin, boxSize);
        // Draw the box at the end position
        Gizmos.DrawWireCube(endPosition, boxSize);
    }
}
