using UnityEngine;
using System.Collections.Generic;

public interface IBoidsRule
{
    Vector3 GetDirection(Transform agent, List<Transform> neighbor);
}
