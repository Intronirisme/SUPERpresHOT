/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BTNode
{
    private bool _attacking = false;
    private bool _completedThisFrame = false;
    public Attack(ref Dictionary<string, object> data)
    {
        _dataContext = data;
    }
    public override NodeState Evaluate()
    {
        Bot bot = (Bot)GetData("botScript");
        if (bot == null) return NodeState.FAILURE;

        if (_completedThisFrame)
        {
            _completedThisFrame = false;
            _attacking = false;
            bot.AttackCompleteDelegate -= End;
            return NodeState.SUCCESS;
        }
        else if (!_attacking)
        {
            _attacking = true;
            bot.AttackCompleteDelegate += End;
            bot.Attack();
        }
        return NodeState.RUNNING;
    }

    public void End()
    {
        //Fin de l'attaque
        _completedThisFrame = true;
    }
}
*/