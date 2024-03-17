public class Inverter : BTNode
{
    protected BTNode _node;

    public Inverter (BTNode node)
    {
        this._node = node;
    }

    public override NodeState Evaluate()
    {
        switch (_node.Evaluate())
        {
            case NodeState.RUNNING:
                _nodeState = NodeState.RUNNING;

                break;
            case NodeState.SUCCESS:
                _nodeState = NodeState.FAILURE;
                break;
            case NodeState.FAILURE:
                _nodeState = NodeState.SUCCESS;
                break;
            default:
                break;
        }
        return _nodeState;
    }
}