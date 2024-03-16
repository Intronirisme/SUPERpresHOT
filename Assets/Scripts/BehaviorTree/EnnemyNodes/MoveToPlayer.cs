using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToPlayer : BTNode
{
    private Transform _transform;

    public MoveToPlayer(Transform transform)
    {
        _transform = transform;
    }
    public override NodeState Evaluate()
    {
        NavMeshAgent navAgent = (NavMeshAgent)GetData("navAgent");
        GameObject player = (GameObject)GetData("player");

        navAgent.destination = player.transform.position;

        _nodeState = NodeState.RUNNING;
        return nodeState;
    }
}
