using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : BaseInteractactable
{


    [SerializeField] bool hasEngineProblem = false;

    //steam animation

    public override void Interact()
    {
        base.Interact();

    }

    //timer to end game can be on the manager
    protected override void StopEvent()
    {
        m_shipManager.CancelCountdown();
    }
}
