using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class TunaController : MonoBehaviour
{

    StateMachine stateMachine;
    Animator animator;

    // give the tuna a state machine


    void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);


    void Start()
    {
        stateMachine = new StateMachine();
        animator = GetComponent<Animator>();


        var swimState = new EnemySwimState(this, animator);

        Any(swimState, new FuncPredicate(() => true));


        stateMachine.SetState(swimState);

    }

    void Update()
    {
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.FixUpdate();
    }
}
