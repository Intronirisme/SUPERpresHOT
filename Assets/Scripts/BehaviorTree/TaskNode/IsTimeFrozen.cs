using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        
    }
}
