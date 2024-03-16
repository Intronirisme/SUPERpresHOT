using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : BTNode
{
    private Transform _transform;

    public AttackPlayer(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        // A compléter
        Debug.Log("Attacked Player !")
        _nodeState = NodeState.RUNNING;
        return nodeState;
    }
}
