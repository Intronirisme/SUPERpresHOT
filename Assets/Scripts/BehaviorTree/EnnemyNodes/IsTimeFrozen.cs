using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IsTimeFrozen : BTNode
{
    private Transform _transform;
    public IsTimeFrozen(Transform transform, ref Dictionary<string, object> data)
    {
        _transform = transform;
        _dataContext = data;
    }

    public override NodeState Evaluate()
    {
        bool isFrozen = (bool)GetData("isFrozen");
        if (isFrozen == true)
        {
            _nodeState = NodeState.RUNNING;
            return _nodeState;
        }
        _nodeState = NodeState.SUCCESS;
        return _nodeState;
    }

}
