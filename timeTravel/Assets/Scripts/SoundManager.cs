using UnityEngine;
using System.Collections;

namespace Level6Scripts
{
    /// <summary>
    /// Central sound manager for Level 6.
    /// Handles: background music, boss music, combat SFX, UI sounds.
    /// Usage from any script: SoundManager.Instance.PlayBossMusic();
    ///                        SoundManager.Instance.PlaySFX(SoundManager.SFX.SwordSwing);
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;

        // ── Music ─────────────────────────────────────────────────────
        [Header("Music")]
        public AudioClip backgroundMusic;   // Calm ambient music before boss
        public AudioClip bossMusic;         // Plays when boss is alive
        public AudioClip victoryMusic;      // Plays after boss dies

        [Range(0f, 1f)] public float musicVolume = 0.5f;

        // ── SFX ───────────────────────────────────────────────────────
        [Header("Sound Effects")]
        public AudioClip swordSwingClip;
        public AudioClip swordHitClip;
        public AudioClip golemHurtClip;
        public AudioClip golemDeathClip;
        public AudioClip golemAttackClip;
        public AudioClip playerHurtClip;
        public AudioClip pickupSwordClip;
        public AudioClip pickupTerraClip;
        public AudioClip lavaDeathClip;
        public AudioClip levelCompleteClip;
        public AudioClip footstepClip;

        [Range(0f, 1f)] public float sfxVolume = 1f;

        // ── Internal ──────────────────────────────────────────────────
        private AudioSource _musicSource;
        private AudioSource _sfxSource;

        public enum SFX
        {
            SwordSwing,
            SwordHit,
            GolemHurt,
            GolemDeath,
            GolemAttack,
            PlayerHurt,
            PickupSword,
            PickupTerra,
            LavaDeath,
            LevelComplete,
            Footstep
        }

        // ─────────────────────────────────────────────────────────────

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SetupAudioSources();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void SetupAudioSources()
        {
            // Music source — looping, lower volume
            _musicSource = gameObject.AddComponent<AudioSource>();
            _musicSource.loop = true;
            _musicSource.volume = musicVolume;
            _musicSource.playOnAwake = false;

            // SFX source — one-shot, full volume
            _sfxSource = gameObject.AddComponent<AudioSource>();
            _sfxSource.loop = false;
            _sfxSource.volume = sfxVolume;
            _sfxSource.playOnAwake = false;
        }

        // ── Music Control ─────────────────────────────────────────────

        public void PlayBackgroundMusic()
        {
            PlayMusic(backgroundMusic);
        }

        public void PlayBossMusic()
        {
            PlayMusic(bossMusic);
        }

        public void PlayVictoryMusic()
        {
            PlayMusic(victoryMusic);
        }

        public void StopMusic()
        {
            _musicSource.Stop();
        }

        /// <summary>Crossfades from current music to new clip over fadeDuration seconds.</summary>
        public void CrossfadeMusic(AudioClip newClip, float fadeDuration = 1.5f)
        {
            StartCoroutine(CrossfadeCoroutine(newClip, fadeDuration));
        }

        private IEnumerator CrossfadeCoroutine(AudioClip newClip, float duration)
        {
            float startVolume = _musicSource.volume;

            // Fade out
            float timer = 0f;
            while (timer < duration / 2f)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / (duration / 2f));
                yield return null;
            }

            _musicSource.clip = newClip;
            _musicSource.Play();

            // Fade in
            timer = 0f;
            while (timer < duration / 2f)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0f, musicVolume, timer / (duration / 2f));
                yield return null;
            }

            _musicSource.volume = musicVolume;
        }

        private void PlayMusic(AudioClip clip)
        {
            if (clip == null)
            {
                Debug.LogWarning("SoundManager: Music clip not assigned.");
                return;
            }

            if (_musicSource.clip == clip && _musicSource.isPlaying) return;

            _musicSource.clip = clip;
            _musicSource.volume = musicVolume;
            _musicSource.Play();
        }

        // ── SFX Control ───────────────────────────────────────────────

        public void PlaySFX(SFX sfx)
        {
            AudioClip clip = GetClip(sfx);

            if (clip == null)
            {
                Debug.LogWarning($"SoundManager: SFX clip for '{sfx}' not assigned in Inspector.");
                return;
            }

            _sfxSource.PlayOneShot(clip, sfxVolume);
        }

        /// <summary>Play SFX at a world position (3D spatial sound).</summary>
        public void PlaySFXAt(SFX sfx, Vector3 position)
        {
            AudioClip clip = GetClip(sfx);
            if (clip == null) return;
            AudioSource.PlayClipAtPoint(clip, position, sfxVolume);
        }

        private AudioClip GetClip(SFX sfx)
        {
            return sfx switch
            {
                SFX.SwordSwing    => swordSwingClip,
                SFX.SwordHit      => swordHitClip,
                SFX.GolemHurt     => golemHurtClip,
                SFX.GolemDeath    => golemDeathClip,
                SFX.GolemAttack   => golemAttackClip,
                SFX.PlayerHurt    => playerHurtClip,
                SFX.PickupSword   => pickupSwordClip,
                SFX.PickupTerra   => pickupTerraClip,
                SFX.LavaDeath     => lavaDeathClip,
                SFX.LevelComplete => levelCompleteClip,
                SFX.Footstep      => footstepClip,
                _                 => null
            };
        }

        // ── Volume Control ────────────────────────────────────────────

        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            _musicSource.volume = musicVolume;
        }

        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
        }
    }
}