using UnityEngine;

namespace Yaku.UI
{
    [RequireComponent(typeof(GameMenu))]
    public sealed class GameAudio : MonoBehaviour
    {
        public enum Cue { None, Interact, Collect, Deliver, Complete }
        public enum Channel { Master, Music, Effects }

        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip communityMusic;
        [SerializeField] private AudioClip interact;
        [SerializeField] private AudioClip collect;
        [SerializeField] private AudioClip deliver;
        [SerializeField] private AudioClip complete;

        private static readonly string[] VolumeKeys = { "Yaku.Audio.Master", "Yaku.Audio.Music", "Yaku.Audio.Effects" };
        private readonly float[] volumes = new float[VolumeKeys.Length];
        private GameMenu menu;
        private AudioSource menuTrack;
        private AudioSource communityTrack;
        private AudioSource effects;
        private float blend;
        private Cue pending;

        private void Awake()
        {
            menu = GetComponent<GameMenu>();
            for (int i = 0; i < volumes.Length; i++)
                volumes[i] = Mathf.Clamp01(PlayerPrefs.GetFloat(VolumeKeys[i], i == 1 ? 0.72f : 1f));
            menuTrack = CreateSource(menuMusic, true);
            communityTrack = CreateSource(communityMusic, true);
            effects = CreateSource(null, false);
        }

        public float GetVolume(Channel channel) => volumes[(int)channel];

        public void SetVolume(Channel channel, float normalized)
        {
            int index = (int)channel;
            volumes[index] = Mathf.Clamp01(normalized);
            PlayerPrefs.SetFloat(VolumeKeys[index], volumes[index]);
            UpdateGains();
        }

        public void Queue(Cue cue)
        {
            if (cue > pending) pending = cue;
        }

        private void LateUpdate()
        {
            blend = Mathf.MoveTowards(blend, menu.IsPlaying ? 1f : 0f, Time.unscaledDeltaTime / 1.4f);
            UpdateGains();
            SetPlaying(menuTrack, blend < 1f);
            SetPlaying(communityTrack, blend > 0f);

            var clip = pending switch
            {
                Cue.Interact => interact,
                Cue.Collect => collect,
                Cue.Deliver => deliver,
                Cue.Complete => complete,
                _ => null
            };
            if (clip != null) effects.PlayOneShot(clip);
            pending = Cue.None;
        }

        private AudioSource CreateSource(AudioClip clip, bool loop)
        {
            var source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.spatialBlend = 0f;
            source.loop = loop;
            source.clip = clip;
            source.volume = 0f;
            return source;
        }

        private static void SetPlaying(AudioSource source, bool active)
        {
            if (active && !source.isPlaying && source.clip != null)
                source.Play();
            else if (!active && source.isPlaying)
                source.Stop();
        }

        private void UpdateGains()
        {
            float master = GetVolume(Channel.Master);
            float music = master * GetVolume(Channel.Music);
            menuTrack.volume = music * Mathf.Sqrt(1f - blend);
            communityTrack.volume = music * Mathf.Sqrt(blend);
            effects.volume = master * GetVolume(Channel.Effects);
        }

        private void OnDestroy() => PlayerPrefs.Save();
    }
}
