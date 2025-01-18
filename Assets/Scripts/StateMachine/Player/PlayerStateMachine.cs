using UnityEngine;

public class PlayerStateMachine : StateMachine<Player, PlayerStateMachine.State>
{
    protected override void SetUpStateMachine()
    {
        
    }

    public enum State
    {
        Idle,
        Move,
        Shoot
    }
}
