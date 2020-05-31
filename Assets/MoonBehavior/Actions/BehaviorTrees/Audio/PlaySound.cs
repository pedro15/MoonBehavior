using UnityEngine;
using MoonBehavior.Memory;

namespace MoonBehavior.BehaviorTrees.Actions
{
    [Info(Category = "Audio" , Description = "Plays an audio from the assigned AudioSource" , Name = "Play Sound")]
    public class PlaySound : Task
    {
        enum PlayMode: int
        {
            Normal = 0,
            OneShot = 1
        }

        [SerializeField]
        private PlayMode Mode;

        public MemoryItem SoundClip = new MemoryItem(ItemType.OBJECT);

        public MemoryItem AudioSoruceKey = new MemoryItem(ItemType.STRING);

        private AudioSource Source;

        private AudioClip clip;

        public override void OnEnter(MoonAI ai)
        {
            string key = AudioSoruceKey.GetValue<string>(ai.Memory);
            Source = ai.Memory.GetValue<AudioSource>(key);
            clip = SoundClip.GetValue<AudioClip>(ai.Memory);
        }

        public override TaskResult OnExecute(MoonAI ai)
        {
            if (clip != null && Source != null)
            {
                if (Mode == PlayMode.Normal)
                {
                    if (Source.clip != clip)
                        Source.clip = clip;
                    if (!Source.isPlaying)
                        Source.Play();
                }else if (Mode == PlayMode.OneShot)
                {
                    Source.PlayOneShot(clip);
                }
                return TaskResult.Success;
            }
            return TaskResult.Failure;
        }
    }
}