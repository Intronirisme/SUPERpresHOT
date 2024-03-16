using System.Collections;
using System.Collections.Generic;

public class Selector : BTNode
{
    protected List<BTNode> _children = new List<BTNode>();

    public Selector(List<BTNode> Children)
    {
        this._children = Children;
    }

    public override NodeState Evaluate()
    {
        foreach (var node in _children)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    _nodeState = NodeState.RUNNING;
                    return _nodeState;
                case NodeState.SUCCESS:
                    _nodeState = NodeState.SUCCESS;
                    return _nodeState;
                case NodeState.FAILURE:
                    break;
                default:
                    break;
            }
        }
        _nodeState = NodeState.FAILURE;
        return _nodeState;
    }
}
