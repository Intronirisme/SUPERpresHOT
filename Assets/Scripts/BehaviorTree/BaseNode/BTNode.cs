using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class BTNode
{
    protected Dictionary<string, object> _dataContext;
    protected NodeState _nodeState;
    public NodeState nodeState { get { return _nodeState; } }

    public abstract NodeState Evaluate();
    public void SetData(string key, object value)
    {
        _dataContext[key] = value;
    }

    public object GetData(string key)
    {
        object val = null;
        if(_dataContext.TryGetValue(key, out val))
        {
            return val;
        }
        else
        {
            Debug.Log(key + " not found");
            return null;
        }
    }
}

public enum NodeState
{
    RUNNING, SUCCESS, FAILURE,
}