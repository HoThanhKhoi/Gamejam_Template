using UnityEngine;

public class PlayerState_Idle : State<Player, PlayerStateMachine.State>
{
    public PlayerState_Idle(Player owner, StateMachine<Player, PlayerStateMachine.State> stateMachine, Animator anim) : base(owner, stateMachine, anim)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (owner.isBusy)
        {
            if (owner.xInput != owner.transform.right.x)
            {
                owner.FaceTo(owner.xInput);
            }
        }
    }
}
