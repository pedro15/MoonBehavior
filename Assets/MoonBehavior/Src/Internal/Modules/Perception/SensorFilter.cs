using System.Collections.Generic;
using UnityEngine;

namespace MoonBehavior.Perception
{
    /// <summary>
    /// Sensor filter base class
    /// </summary>
    public abstract class SensorFilter : ScriptableObject 
    {
        /// <summary>
        /// Applies the filter to the base Detected enitys
        /// </summary>
        /// <param name="DetectedEntitys">Detected Entitys list</param>
        /// <param name="Agent">GameObject AI Agent</param>
        /// <returns>Filted Entityes</returns>
        public abstract List<GameObject> ApplyFilter(List<GameObject> DetectedEntitys, GameObject Agent);
    }
}