using UnityEngine;
using MoonBehavior.Memory; 

namespace MoonBehavior.BehaviorTrees
{
	public class MoonAI : MonoBehaviour
	{
		/// <summary>
		/// Stored Behavior Tree Instance
		/// </summary>
		private MoonBT BTInstance = null;

		/// <summary>
		/// Current executing Behavior Tree
		/// </summary>
		public MoonBT CurrentBT
		{
			get
			{
				return BTInstance;
			}
		}

		/// <summary>
		/// Behavior tree TickMode
		/// </summary>
		public enum TickMode : int
		{
		   SmartUpdate = 0 , FixedUpdate = 1 , LateUpdate = 2
		}

		[SerializeField]
		private TickMode _TickMode;
		
		public TickMode tickMode { get { return _TickMode; } }

		/// <summary>
		/// User defined behavior tree
		/// </summary>
		public MoonBT BehaviorTree;


		/// <summary>
		/// Stored MoonMemory Instance
		/// </summary>
		private MoonMemory Mem;
		
		/// <summary>
		/// Memory Component 
		/// </summary>
		public MoonMemory Memory
		{
			get
			{
                if (!Mem) Mem = gameObject.AddComponent<MoonMemory>();
                return Mem;
			}
		}

		void Awake()
		{
            Mem = GetComponent<MoonMemory>();
            if (Mem != null)
			    Mem.Init();
			LoadBTInstance();
		}

		/// <summary>
		/// Loads behavior tree instance
		/// </summary>
		public void LoadBTInstance()
		{
			if (BehaviorTree != null && BehaviorTree.IsValid())
			{
				BTInstance = MoonBT.CopyBT(BehaviorTree);
			}else
            {
                Debug.LogWarning( "[MoonBehavior] " +  name +  ": Invalid BehaviorTree");
                enabled = false;
                return;
            }
		}

		/// <summary>
		/// Destroys behavior tree instance
		/// </summary>
		public void RemoveBTInstance()
		{
			if (BTInstance != null && BTInstance.IsValid())
			{
				RemoveTask(BTInstance.Root);
			}
		}

		/// <summary>
		/// Destroys an task instance
		/// </summary>
		/// <param name="t">Task instance</param>
		private void RemoveTask(Task t)
		{
			if (t != null )
			{
				if (t is Decision)
				{
					Decision tdec = (Decision)t;

					for (int i = 0; i < tdec.Childs.Length; i++)
					{
						RemoveTask(tdec.Childs[i]);
					}
                    Destroy(tdec);
				}
				else
				{
                    
					Destroy(t);
				}
			}
		}

		/// <summary>
		/// Runs the behavior tree
		/// </summary>
		/// <param name="mode">Current tick mode</param>
		private void RunBehaviorTree(TickMode mode)
		{
			if (gameObject.activeInHierarchy & isActiveAndEnabled)
			{
				if (mode == tickMode & BTInstance.IsValid())
					BTInstance.ExecuteBT(this);
				else return;
			}
		}
		
		private void Update()
		{
			RunBehaviorTree(TickMode.SmartUpdate);
		}

		private void LateUpdate()
		{
			RunBehaviorTree(TickMode.LateUpdate);
		}

		private void FixedUpdate()
		{
			RunBehaviorTree(TickMode.FixedUpdate);
		}
        
        private void OnDestroy()
		{
			RemoveBTInstance();
		}
	}
}