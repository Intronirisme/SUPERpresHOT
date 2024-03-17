using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerInRange : BTNode
{
    private float _range;
    public PlayerInRange(ref Dictionary<string, object> data, float Range)
    {
        _dataContext = data;
        _range = Range;
    }
    public override NodeState Evaluate()
    {
        GameObject myself = (GameObject)GetData("self");
        GameObject player = (GameObject)GetData("player");

        if (myself == null || player == null) return NodeState.FAILURE;

        float distance = Vector3.Distance(myself.transform.position, player.transform.position);
        return distance <= _range ? NodeState.SUCCESS : NodeState.FAILURE;
    }
}
