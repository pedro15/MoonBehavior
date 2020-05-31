using UnityEngine;
using System.Collections.Generic;

namespace MoonBehavior.Perception
{
    /// <summary>
    /// Sensor to detect Visual Entitys
    /// </summary>
    [System.Serializable]
    public class VisualSensor : Sensor
    {
        /// <summary>
        /// Maximun angle from target 
        /// </summary>
        public float Angle;
        /// <summary>
        /// Maximun distance from target
        /// </summary>
        public float Distance = 10;
        
        /// <summary>
        /// Sensor position offset
        /// </summary>
        public Vector3 PositionOffset = Vector3.zero;
        
        /// <summary>
        /// Sensor Rotation offset
        /// </summary>
        public Vector3 RotationOffset = Vector3.zero;

        /// <summary>
        /// Mask for enity Targets
        /// </summary>
        public LayerMask TargetMask;

        /// <summary>
        /// Mask for target obstacles
        /// </summary>
        public LayerMask ObstacleMask;
        
        /// <summary>
        /// Sould use deep detection algoritm ? 
        /// </summary>
        public bool DeepDetect;
        
        /// <summary>
        /// Should ignore Triggers
        /// </summary>
        public bool IgnoreTriggers = true; 

        /// <summary>
        /// Forward direction (Like the 'transform.forward') of this sensor
        /// </summary>
        public Vector3 Forward
        {
            get
            {
                return Quaternion.Euler(RotationOffset) * Agent.transform.forward; 
            }
        }
        
        /// <summary>
        /// Left position corner for deep detect
        /// </summary>
        public Vector3 LeftAnchor
        {
            get
            {
                return Origin + ((Quaternion.AngleAxis(-Angle * 0.5f , Vector3.up) * Forward).normalized * Distance ); 
            }
        }

        /// <summary>
        /// Right position corner for deeep detect
        /// </summary>
        public Vector3 RightAnchor
        {
            get
            {
                return Origin + ((Quaternion.AngleAxis(Angle * 0.5f, Vector3.up) * Forward).normalized * Distance);
            }
        }

        /// <summary>
        /// Sensor origin Position
        /// </summary>
        public Vector3 Origin
        {
            get
            {
                return Agent.transform.position + Agent.transform.TransformDirection(PositionOffset);
            }
        }

        protected override IEnumerable<GameObject> DoDetect(System.Func<GameObject,bool> IsValid)
        {
            
            Collider[] Colls = Physics.OverlapSphere(Origin, Distance , TargetMask , IgnoreTriggers ? QueryTriggerInteraction.Ignore : QueryTriggerInteraction.Collide );
            
            for (int i = 0; i < Colls.Length; i++)
            {
                Collider currentColl = Colls[i];
                
                if (IsInside(currentColl.bounds.center) && IsValid(currentColl.gameObject))
                {
                    yield return currentColl.gameObject;
                }else if (DeepDetect)
                {
                    Vector3 LP = currentColl.ClosestPoint(LeftAnchor);
                    bool VisibleLeft = IsInside(LP) ;

                    Vector3 RP = currentColl.ClosestPoint(RightAnchor);

                    bool VisibleRight = IsInside(RP);

                    if (VisibleLeft | VisibleRight)
                    {
                        if (IsValid(currentColl.gameObject)) yield return currentColl.gameObject; 
                    }
                }
            }
        }

        /// <summary>
        /// Are the point inside the vision ?
        /// </summary>
        /// <param name="Point">Point</param>
        /// <returns>True if the point is inside, otherwise false</returns>
        public bool IsInside(Vector3 Point)
        {
            Vector3 Dir = (Point - Origin);
            
            if (Dir.magnitude > Distance) return false;  
            
            float m_angle = Vector3.Angle(Forward, Dir.normalized);
            
            if (m_angle <= Angle * 0.5f)
                return !Physics.Raycast(Origin, Dir.normalized, Dir.magnitude, ObstacleMask);
            
            return false; 
        }
    }
}