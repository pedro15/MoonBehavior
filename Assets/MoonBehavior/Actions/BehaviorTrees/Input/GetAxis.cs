using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Input" , Description = "Saves the value of the virtual axis to the Memory component" , Name = "Get Axis")]
    public class GetAxis : Task
    {

        private enum AxisMode
        {
            Normal,Raw
        }

        [SerializeField]
        private AxisMode Mode;

        public MemoryItem AxisName = new MemoryItem(ItemType.STRING);

        public MemoryItem SaveKey = new MemoryItem(ItemType.STRING);
        
        public override TaskResult OnExecute(MoonAI ai)
        {
            string axisName = AxisName.GetValue<string>(ai.Memory);
            string savekey = SaveKey.GetValue<string>(ai.Memory);

            if (!string.IsNullOrEmpty(axisName) & !string.IsNullOrEmpty(savekey))
            {
                ai.Memory.SetValue(savekey, (Mode == AxisMode.Normal) ? Input.GetAxis(axisName) : Input.GetAxisRaw(axisName));
                return TaskResult.Success;
            }

            return TaskResult.Failure;

        }
    }
}