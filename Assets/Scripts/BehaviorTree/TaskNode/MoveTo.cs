using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : BTNode
{
    private string _locationKey;


    MoveTo(ref Dictionary<string, object> data, string LocationKey)
    {
        _dataContext = data;
        _locationKey = LocationKey;
    }

    public override NodeState Evaluate()
    {
        Vector3 target = (Vector3)GetData(_locationKey);

        if (target == null)
        {

        }
        return NodeState.FAILURE;
    }
}
