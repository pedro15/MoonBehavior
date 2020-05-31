using System.Collections.Generic;
using UnityEngine;

namespace MoonBehavior.Navigation
{
    /// <summary>
    /// A waypoint path component with nice Gizmos editor
    /// </summary>
    public class MoonPath : MonoBehaviour
    {
        public List<Vector3> Waypoints = new List<Vector3>();
        
#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            for (int i = 0; i < Waypoints.Count; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(Waypoints[i], 0.15f);
                int j = i + 1;
                Gizmos.color = Color.red; 
                if (j < Waypoints.Count)
                {
                    Gizmos.DrawLine(Waypoints[i], Waypoints[j]);
                }
            }
        }

#endif
    }
}
