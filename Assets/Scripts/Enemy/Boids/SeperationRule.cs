using UnityEngine;
using System.Collections.Generic;

public class SeperationRule : IBoidsRule
{
    public Vector3 GetDirection(Transform agent, List<Transform> neighbor)
    {
        if(agent == null || neighbor == null || neighbor.Count == 0)
        {
            return Vector3.zero;
        }

        Vector3 dir = Vector3.zero;

        foreach(var ne in neighbor)
        {
            dir += agent.position - ne.transform.position;
        }

        return dir.normalized;
    }
}
