using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : BTNode
{
    private Transform _transform;

    public MoveToPlayer(Transform transform, ref Dictionary<string, object> data)
    {
        _transform = transform;
        _dataContext = data;
    }
    public override NodeState Evaluate()
    {
        NavMeshAgent navMeshAgent = (NavMeshAgent)GetData("navMeshAgent");
        GameObject player = (GameObject)GetData("player");

        navMeshAgent.destination = player.transform.position;

        _nodeState = NodeState.RUNNING;
        return nodeState;
    }
}
