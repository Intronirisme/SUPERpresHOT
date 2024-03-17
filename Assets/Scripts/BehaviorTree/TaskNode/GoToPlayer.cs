using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToPlayer : BTNode
{
    public GoToPlayer(ref Dictionary<string, object> data)
    {
        _dataContext = data;
    }
    public override NodeState Evaluate()
    {
        GameObject player = (GameObject)GetData("player");
        Transform bodyTrans = (Transform)GetData("transform");
        NavMeshAgent navAgent = (NavMeshAgent)GetData("navAgent");

        _nodeState = NodeState.FAILURE;
        if(player == null || navAgent == null) return _nodeState;

        if(Vector3.Distance(navAgent.destination, player.transform.position) > 2)
        {
            navAgent.SetDestination(player.transform.position);
        }

        if(Vector3.Distance(bodyTrans.position, player.transform.position) < 2)
        {
            _nodeState = NodeState.SUCCESS;
            navAgent.SetDestination(bodyTrans.position);
        }
        else _nodeState = NodeState.RUNNING;

        return _nodeState;
    }
}