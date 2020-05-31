using UnityEngine;
using System.Linq;
using System.Collections.Generic;

namespace MoonBehavior.Perception
{
    /// <summary>
    /// Sensor base class
    /// </summary>
    [System.Serializable]
    public abstract class Sensor : System.Object
    {
        /// <summary>
        /// Sensor name
        /// </summary>
        public string SensorName;

        /// <summary>
        /// Sensor Gizmos color
        /// </summary>
        public Color SensorColor;

        /// <summary>
        /// Sensor filters
        /// </summary>
        [SerializeField]
        private SensorFilterCollection FilterCollection;

        /// <summary>
        /// Cached sensor filters copy
        /// </summary>
        private SensorFilterCollection copycoll = null;

        /// <summary>
        /// Cached GameObject agent
        /// </summary>
        protected GameObject Agent
        {
            get; private set;
        }

        /// <summary>
        /// Sensor initial setup
        /// </summary>
        /// <param name="agent"></param>
        public void Init(GameObject agent)
        {
            Agent = agent;
            if (FilterCollection != null)
            {
                copycoll = Object.Instantiate(FilterCollection);
                copycoll.Init();
            }
        }

        /// <summary>
        /// Detects the entitys with based on the given condition
        /// </summary>
        /// <param name="IsValid">Condition to validate detected entitys</param>
        /// <returns>Detected GameObjects list</returns>
        public List<GameObject> Detect(System.Func<GameObject,bool> IsValid )
        {
            List<GameObject> Detected = DoDetect(IsValid).ToList();
            if (copycoll != null )
            {
                for (int i = 0; i < copycoll.Filters.Length; i++)
                {
                    if (copycoll.Filters[i] != null)
                        Detected = copycoll.Filters[i].ApplyFilter(Detected, Agent);
                }
            }
            return Detected;
        }

        /// <summary>
        /// Detects the entitys
        /// </summary>
        /// <param name="IsValid">Validation condition</param>
        /// <returns></returns>
        protected abstract IEnumerable<GameObject> DoDetect(System.Func<GameObject, bool> IsValid); 
    }
}