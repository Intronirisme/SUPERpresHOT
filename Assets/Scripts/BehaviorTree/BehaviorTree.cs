using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour
{
    protected Dictionary<string, object> _blackboard = new Dictionary<string, object>();
    protected BTNode _root;

    // Start is called before the first frame update
    private void Awake()
    {
        InitBlackboard();
        ConstructTree();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_root != null) _root.Evaluate();
        else Debug.Log("Tree not constructed");
    }

    protected virtual void InitBlackboard()
    {
        _blackboard["self"] = gameObject;
    }

    protected virtual void ConstructTree()
    {

    }
}
