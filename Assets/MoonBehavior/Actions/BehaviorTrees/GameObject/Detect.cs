using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using MoonBehavior.Memory;
using MoonBehavior.Perception;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "GameObject" , Description = "Detects an Entity and saves it to the Memory Component")]
    public class Detect : Task
    {
        [SerializeField]
        private MemoryItem _SensorName = new MemoryItem(ItemType.STRING);
        
        [SerializeField]
        private MemoryItem _SearchTag = new MemoryItem(ItemType.STRING);
        private string SearchTag;


        [SerializeField]
        private MemoryItem _SaveName = new MemoryItem(ItemType.STRING);
        private string SaveName;

        [SerializeField]
        private MemoryItem _DetectSelf = new MemoryItem(ItemType.BOOLEAN);
        private bool DetectSelf;

        [SerializeField]
        private MemoryItem _ResetOnExitSensor = new MemoryItem(ItemType.BOOLEAN);
        private bool ResetOnExit;
        
        [HideOnDebug]
        private float IterationTime = 0.25f; 

        private VisualSensor Vision = null;

        private bool DetectSomething = false;
        private bool Running = false; 

        public override void OnEnter(MoonAI ai)
        {
            DetectSomething = false;
            Running = false;

            DetectSelf = _DetectSelf.GetValue<bool>(ai.Memory);
            SearchTag = _SearchTag.GetValue<string>(ai.Memory);
            SaveName = _SaveName.GetValue<string>(ai.Memory);
            ResetOnExit = _ResetOnExitSensor.GetValue<bool>(ai.Memory);
            
            string SensorName = _SensorName.GetValue<string>(ai.Memory);
            if (!string.IsNullOrEmpty(SensorName))
            {
                if (!ai.GetComponent<MoonPerception>())
                {
                    Debug.LogError("[MoonBehavior] No MoonPerception component detected! aborting.");
                    return;
                }
                Vision = ai.GetComponent<MoonPerception>().GetSensor(SensorName) as VisualSensor;
                
            }
        }
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            if (!Running && Vision != null && !DetectSomething)
                ai.StartCoroutine(DoDetect(ai));

            if (string.IsNullOrEmpty(SearchTag) | string.IsNullOrEmpty(SaveName) | Vision == null) return TaskResult.Failure;

            return DetectSomething ? TaskResult.Success : TaskResult.Running;
        }

        IEnumerator DoDetect(MoonAI ai)
        {

            while (LastResult != TaskResult.Success)
            {
                Running = true;
                List<GameObject> DetectedEntitys = Vision.Detect((GameObject obj) => obj.CompareTag(SearchTag) && (!DetectSelf ? obj != ai.gameObject : true) );
                
                if (DetectedEntitys.Count > 0 )
                {
                    DetectSomething = true;
                    ai.Memory.SetValue(SaveName, DetectedEntitys[0]);
                }else if (ResetOnExit)
                {
                    ai.Memory.SetValue(SaveName, null);
                }

                yield return new WaitForSeconds(IterationTime);
            }
        }

    }
}