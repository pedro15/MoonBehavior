using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace MoonBehavior.Perception
{
    /// <summary>
    /// AI Perception class that handles sensors and Enity Detection
    /// </summary>
    public class MoonPerception : MonoBehaviour
    {
        /// <summary>
        /// Visual Sensors
        /// </summary>
        public List<VisualSensor> VisualSensors = new List<VisualSensor>();
        
        private void OnEnable()
        {
            VisualSensors.ForEach((VisualSensor sen) => sen.Init(gameObject));
        }
        
        /// <summary>
        /// Returns an sensor found with the given name
        /// </summary>
        /// <param name="sensorName">Sensor name</param>
        /// <returns>Sensor found</returns>
        public Sensor GetSensor(string sensorName)
        {
            return VisualSensors.FirstOrDefault((VisualSensor vi) => string.Equals(vi.SensorName, sensorName));
        }
        
    }
}