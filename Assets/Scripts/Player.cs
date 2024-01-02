using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle, Move, Jump, Sit, Attack, Hit, Stunned, LeanedEdge, GrabEdge, Fall, LookUp, Dead
}

public class Player : MonoBehaviour
{
    PlayerState curState;
    StateBase<Player>[] states = new StateBase<Player>[System.Enum.GetValues(typeof(PlayerState)).Length];

    SpriteRenderer sprite;



    private void Awake()
    {
        states[(int)PlayerState.Idle] = new IdleState(this);
        states[(int)PlayerState.Move] = new MoveState(this);
        states[(int)PlayerState.Jump] = new JumpState(this);
        states[(int)PlayerState.Sit] = new SitState(this);
        states[(int)PlayerState.Attack] = new AttackState(this);
        states[(int)PlayerState.Hit] = new HitState(this);
        states[(int)PlayerState.Stunned] = new StunnedState(this);
        states[(int)PlayerState.LeanedEdge] = new LeanedEdgeState(this);
        states[(int)PlayerState.GrabEdge] = new GrapEdgeState(this);
        states[(int)PlayerState.Fall] = new FallState(this);
        states[(int)PlayerState.LookUp] = new LookUpState(this);
        states[(int)PlayerState.Dead] = new DeadState(this);

        curState = PlayerState.Idle;
        states[(int)curState].Enter();
    }

    void Update()
    {
        states[(int)curState].Update();
    }
}

public class IdleState : StateBase<Player>
{
    public IdleState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class MoveState : StateBase<Player>
{
    public MoveState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class JumpState : StateBase<Player>
{
    public JumpState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class SitState : StateBase<Player>
{
    public SitState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class AttackState : StateBase<Player>
{
    public AttackState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class HitState : StateBase<Player>
{
    public HitState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class StunnedState : StateBase<Player>
{
    public StunnedState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class LeanedEdgeState : StateBase<Player>
{
    public LeanedEdgeState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class GrapEdgeState : StateBase<Player>
{
    public GrapEdgeState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}

public class FallState : StateBase<Player>
{
    public FallState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class LookUpState : StateBase<Player>
{
    public LookUpState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}

public class DeadState : StateBase<Player>
{
    public DeadState(Player owner) : base(owner)
    {
    }

    public override void Enter()
    {

    }

    public override void Exit()
    {

    }

    public override void Update()
    {

    }
}
