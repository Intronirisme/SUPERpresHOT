using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : BTNode
{
    private string _locationKey;
    private LevelManager _level;
    MoveTo(ref Dictionary<string, object> data, string LocationKey)
    {
        _dataContext = data;
        _locationKey = LocationKey;
        _level = GameObject.FindObjectOfType<LevelManager>();
    }

    public override NodeState Evaluate()
    {
        GameObject owner = (GameObject)_dataContext["self"];
        Vector2Int currentLoc = new Vector2Int(
            (int)owner.transform.position.x,
            (int)owner.transform.position.y
        );
        Vector2Int targetLoc = (Vector2Int)_dataContext[_locationKey];

        if (currentLoc == targetLoc) return NodeState.SUCCESS;
        if (_level == null) return NodeState.FAILURE;

        List<Node> path = new List<Node>();
        if (_level.FindPath(currentLoc, targetLoc, ref path))
        {
            return NodeState.RUNNING;
        }
        else return NodeState.FAILURE;
    }
}
