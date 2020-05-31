using System.Collections.Generic;
using UnityEngine;

namespace MoonBehavior.Perception
{
    /// <summary>
    /// Near SensorFilter 
    /// Orders the entitys from the nearest to the largest distance
    /// </summary>
    public class NearFilter : SensorFilter
    {
        public override List<GameObject> ApplyFilter(List<GameObject> DetectedEntitys, GameObject Agent)
        {
            for (int i = 0; i < DetectedEntitys.Count; i++)
            {
                for (int j = i + 1; j < DetectedEntitys.Count; j++)
                {
                    GameObject c = DetectedEntitys[i];
                    GameObject n = DetectedEntitys[j];

                    Vector3 Dir_c = (Agent.transform.position - c.transform.position);
                    Vector3 Dir_n = (Agent.transform.position - n.transform.position);
                    
                    if ((Dir_n.sqrMagnitude < Dir_c.sqrMagnitude))
                    {
                        DetectedEntitys[j] = c;
                        DetectedEntitys[i] = n;
                    }

                }
            }
            return DetectedEntitys;
        }
    }
}