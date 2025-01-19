using System.Collections;
using UnityEngine;

public class BossStoneGolemState_Zip : State<BossStoneGolem, BossStoneGolemStateMachine.State>
{
    float velocityScale;
    float delay;

    private IShowHide playerShowHideIndicatorComponent;
    public BossStoneGolemState_Zip(BossStoneGolem owner, StateMachine<BossStoneGolem, BossStoneGolemStateMachine.State> stateMachine, Animator anim) : base(owner, stateMachine, anim)
    {
    }

    public override void Enter()
    {
        base.Enter();
        owner.Rb.bodyType = RigidbodyType2D.Dynamic;
        owner.ShowAttackIndicatorOnPlayer(true);

        stateTimer = owner.ZipShootCooldown - animationLength;
        delay = 1f;
        velocityScale = 1;
    }

    public override void Update()
    {
        base.Update();

        delay -= Time.deltaTime;

        owner.FaceToPlayer();

        if (TimeOut())
        {
            if (!owner.IsZipShootCountFull())
            {
                stateTimer = owner.ZipShootCooldown;
                owner.SetActiveZipIndicator(false);
                owner.ShootSelfToPlayer();
                velocityScale = 1;
            }
        }

        if (owner.Rb.linearVelocity.magnitude <= 2f && delay <= 0)
        {
            if (owner.IsZipShootCountFull())
            {
                owner.SetActiveZipIndicator(false);
                stateMachine.ChangeState(BossStoneGolemStateMachine.State.FlyToCenter);
            }
            else
            {
                owner.SetActiveZipIndicator(false);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        velocityScale -= Time.deltaTime * 0.01f;

        owner.Rb.linearVelocity *= velocityScale;
    }

    override public void Exit()
    {
        base.Exit();

        owner.StopMoving();
        owner.ShowAttackIndicatorOnPlayer(false);
        owner.Rb.bodyType = RigidbodyType2D.Kinematic;
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if(!other.collider.CompareTag("PlayerAttack"))
        {
            ObjectPoolingManager.Instance.SpawnFromPool("DamageImpact", other.contacts[0].point, Quaternion.identity);
        }
    }
}