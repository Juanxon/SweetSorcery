using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAllyFirstTargets : MonoBehaviour
{
    public List<Transform> allysFirstTargets;
    public void DeleteTarget(Transform target)
    {
        if (allysFirstTargets.Contains(target))
        {
            allysFirstTargets.Remove(target);
        }
    }
}

