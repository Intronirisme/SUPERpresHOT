using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanSeePlayer : BTNode
{
    public CanSeePlayer(ref Dictionary<string, object> data)
    {
        _dataContext = data;
    }
    public override NodeState Evaluate()
    {
        Transform bodyTrans = (Transform)GetData("transform");
        GameObject player = (GameObject)GetData("player");

        // verify that NO ONE is null
        _nodeState = NodeState.FAILURE;
        if(bodyTrans == null || player == null) return _nodeState;

        Vector3 traceStart = bodyTrans.position + new Vector3(0, .6f, 0);
        Vector3 direction = player.transform.position - traceStart;
        RaycastHit hit;

        if(Physics.Raycast(traceStart, direction, out hit, 30))
        {
            Debug.DrawRay(traceStart, direction.normalized * hit.distance, Color.yellow);
            _nodeState = NodeState.SUCCESS;
        }

        return _nodeState;
    }
}