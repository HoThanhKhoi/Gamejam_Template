using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    public bool IsPlatformer = true;
    public IMoveable MoveComponent { get; set; }
    public IFacing FacingComponent { get; set; }
    public ICheckGrounded CheckGroundedComponent { get; set; }
    public IJumpable JumpComponent { get; set; }

    private void Start()
    {
        if(IsPlatformer)
        {
            InitiatePlatformer();
        }
    }

    private void InitiatePlatformer()
    {
        MoveComponent = GetComponent<PlatformerHorizontalMoveComponent>();
        FacingComponent = GetComponent<IFacing>();
        CheckGroundedComponent = GetComponent<ICheckGrounded>();
        JumpComponent = GetComponent<IJumpable>();
    }

    private void InitiateTopdown()
    {
        MoveComponent = GetComponent<TopDownMoveComponent>();
        FacingComponent = GetComponent<IFacing>();
    }

}
