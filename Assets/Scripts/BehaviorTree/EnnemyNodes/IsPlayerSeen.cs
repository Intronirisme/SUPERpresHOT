using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsPlayerSeen : BTNode
{
    private Transform _transform;
    private RaycastHit hit;

    public IsPlayerSeen(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        GameObject player = (GameObject)GetData("player");
        
        Collider[] colliders = Physics.OverlapSphere(_transform.position, 2f);

        foreach(Collider collider in colliders)
        {
            if (collider.gameObject.tag == "Player")
            {
                if(Physics.Raycast(_transform.position, (player.transform.position - _transform.position), out hit))
                {
                    if(hit.collider.gameObject.tag == "Player")
                    {
                        if(Vector3.Angle(player.transform.position-_transform.position, _transform.forward)<=45f)
                        {
                            _nodeState = NodeState.SUCCESS;
                            return nodeState;
                        }
                    }
                }
            }
        }
        _nodeState = NodeState.FAILURE;
        return nodeState;

    }
}
