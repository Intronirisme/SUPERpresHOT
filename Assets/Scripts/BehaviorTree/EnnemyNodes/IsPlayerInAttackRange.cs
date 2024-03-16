using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerInAttackRange : BTNode
{
    private Transform _transform;

    public IsPlayerInAttackRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        GameObject player = (GameObject)GetData("player");

        if(Vector3.Distance(_transform.position, player.transform.position) <= 2f) //!! A CHANGER AVEC LA VALEUR DE RANGE VARIABLE !!
        {
            _nodeState = NodeState.SUCCESS;
            return nodeState;
        }
        _nodeState = NodeState.FAILURE;
        return nodeState;
    }
}
