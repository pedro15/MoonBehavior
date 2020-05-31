using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace MoonBehavior.Perception
{
    /// <summary>
    /// Senfor filter collection asset
    /// Used to handle lists of filters to apply to an sensor
    /// </summary>
    [CreateAssetMenu(menuName = "MoonBehavior/Sensor Filter Collection" , fileName = "New FilterCollection" , order = 1000)]
    public class SensorFilterCollection : ScriptableObject
    {
        public SensorFilter[] Filters;
        
        public void Init()
        {
            List<SensorFilter> original_filters = Filters.ToList();
            for (int i = 0; i < original_filters.Count; i++)
            {
                SensorFilter currf = original_filters[i];
                Filters[i] = Instantiate(currf);
            }
            original_filters.Clear();
        }
    }
}