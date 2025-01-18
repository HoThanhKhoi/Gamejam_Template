using UnityEngine;

public class PlayerPlatformerController : MonoBehaviour
{
    [field: SerializeField] public InputReader InputReader { get; private set; }

    //public bool IsPlatformer = true;
    public IMoveable MoveComponent { get; set; }
    public IFacing FacingComponent { get; set; }
    public ICheckGrounded CheckGroundedComponent { get; set; }
    public IJumpable JumpComponent { get; set; }

    private void Start()
    {
        InitiatePlatformer();
    }

    private void InitiatePlatformer()
    {
        MoveComponent = GetComponent<PlatformerHorizontalMoveComponent>();
        FacingComponent = GetComponent<HorizonalFacingComponent>();
        CheckGroundedComponent = GetComponent<ICheckGrounded>();
        JumpComponent = GetComponent<IJumpable>();
    }

    //private void InitiateTopdown()
    //{
    //    MoveComponent = GetComponent<TopDownMoveComponent>();
    //    FacingComponent = GetComponent<MouseFacingComponent>();
    //    if (FacingComponent == null)
    //    {
    //        Debug.LogError("MouseFacingComponent not found on the GameObject.");
    //    }
    //}

}
