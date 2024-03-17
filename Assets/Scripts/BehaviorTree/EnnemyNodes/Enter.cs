using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enter : BTNode
{
    bool _hasAlreadyEnter;
    bool _isFrozen;

    Transform _transform;
    Transform _trajectoryStartWaypointTransform;
    Transform _trajectoryEndWaypointTransform;

    float _arrivalDistance = 1f; // Room to manoeuvre value

    float _movingSpeed;

    public Enter(ref Dictionary<string, object> blackBoard)
    {
        _dataContext = blackBoard;

        _transform = (Transform)GetData("transform");
        _hasAlreadyEnter = (bool)GetData("hasAlreadyEnter");
        

        // Warning do not change order in scene it must be : Enemy -> StartWaypoint, EndWaypoint, EnemyBody
        _trajectoryStartWaypointTransform = _transform.parent.GetChild(0); 
        _trajectoryEndWaypointTransform = _transform.parent.GetChild(1);

        _movingSpeed = 
            Vector3.Distance(_trajectoryStartWaypointTransform.position, _trajectoryEndWaypointTransform.position)
            / GameMaster.Instance.AssaultDuration;

        // Teleport enemy to the first waypoint
        _transform.SetPositionAndRotation(_trajectoryStartWaypointTransform.position, Quaternion.identity);
    }

    public override NodeState Evaluate()
    {
        _isFrozen = (bool)GetData("isFrozen");

        if (!_isFrozen)
        {
            if (!_hasAlreadyEnter)
            {
                // Make the enemy move to the second waypoint in N time at X speed
                Vector3 direction = (_trajectoryEndWaypointTransform.position - _trajectoryStartWaypointTransform.position).normalized;

                _transform.position += _movingSpeed * Time.fixedDeltaTime * direction;

                // If we finished moving to the end waypoint
                if (Vector3.Distance(_transform.position, _trajectoryEndWaypointTransform.position) <= _arrivalDistance)
                {
                    _hasAlreadyEnter = true;
                    SetData("hasAlreadyEnter", _hasAlreadyEnter);

                    SetData("isFrozen", true); // Because the script finished faster than the function  To avoid the behavior to begin is patrol 

                    return NodeState.SUCCESS; // We past to other behavior
                }

                return NodeState.RUNNING;
            }

            return NodeState.FAILURE; 
        }
        else
        {
            return NodeState.RUNNING; // We let the algorithm loop in the script
        }
    }

    IEnumerator LittleWait()
    {
        yield return new WaitForSeconds(0.1f);
    }
}