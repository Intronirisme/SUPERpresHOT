using System.Collections;
using System.Collections.Generic;

public class Sequence : BTNode
{
    protected List<BTNode> _children = new List<BTNode>();

    public Sequence(List<BTNode> children)
    {
        this._children = children;
    }

    public override NodeState Evaluate()
    {
        bool anyChildIsRunning = false;

        foreach (var node in _children)
        {
            switch (node.Evaluate())
            {
                case NodeState.RUNNING:
                    anyChildIsRunning = true;
                    break;
                case NodeState.SUCCESS:
                    break;
                case NodeState.FAILURE:
                    _nodeState = NodeState.FAILURE;
                    return _nodeState;
                default:
                    break;

            }
        }

        _nodeState = anyChildIsRunning ? NodeState.RUNNING : NodeState.SUCCESS;
        return _nodeState;
    }
}
