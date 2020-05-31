using MoonBehavior.Memory;
using UnityEngine;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Vector" , Description = "Builds a vector and save it to Memory component" , Name = "Build Vector")]
    public class BuildVector : Task
    {
        public enum VectorType
        {
            Vector2, Vector3
        }

        public VectorType BuildType;

        public MemoryItem _x = new MemoryItem(ItemType.FLOAT);

        public MemoryItem _y = new MemoryItem(ItemType.FLOAT);

        public MemoryItem _z = new MemoryItem(ItemType.FLOAT);
        
        public MemoryItem _SaveKey = new MemoryItem(ItemType.STRING);

        [HideOnDebug]
        private string SaveKey { get; set; }

        public override void OnEnter(MoonAI ai)
        {
            SaveKey = _SaveKey.GetValue<string>(ai.Memory);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {

            float x = _x.GetValue<float>(ai.Memory);
            float y = _y.GetValue<float>(ai.Memory);
            float z = _z.GetValue<float>(ai.Memory);
            
            if (!string.IsNullOrEmpty(SaveKey))
            {
                switch (BuildType)
                {
                    case VectorType.Vector2:
                        
                        ai.Memory.SetValue(SaveKey, new Vector2(x, y));

                        break;

                    case VectorType.Vector3:
                        
                        ai.Memory.SetValue(SaveKey, new Vector3(x, y, z));

                        break;
                }

                return TaskResult.Success;

            }

            return TaskResult.Failure;
        }
    }
}