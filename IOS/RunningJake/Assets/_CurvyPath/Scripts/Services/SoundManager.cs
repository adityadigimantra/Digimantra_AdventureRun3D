using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CurvyPath
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [Header("Max number allowed of same sounds playing together")]
        public int maxSimultaneousSounds = 7;

        // List of sounds used in this game
        public Sound background;
        public Sound button;
        public Sound coin;
        public Sound gameOver;
        public Sound tick;
        public Sound rewarded;
        public Sound unlock;
        public Sound score;
        public Sound wrong;

        public delegate void MusicStatusChangedHandler(bool isOn);
        public static event MusicStatusChangedHandler MusicStatusChanged;

        public delegate void SoundStatusChangedHandler(bool isOn);
        public static event SoundStatusChangedHandler SoundStatusChanged;

        enum PlayingState
        {
            Playing,
            Paused,
            Stopped
        }

        public AudioSource bgmSource;
        public AudioSource sfxSource;
        [Tooltip(
            "Audio source to play special sound like 'unlock', 'reward', etc.\n" +
            "Sound played with this source will overwhelms others, including background music (ducking effect)\n" +
            "Call PlaySound() with isSpecialSound set to 'true'")]
        public AudioSource specialSfxSource;

        private PlayingState musicState = PlayingState.Stopped;
        private const string MUTE_PREF_KEY = "MutePreference";
        private const int MUTED = 1;
        private const int UN_MUTED = 0;
        private const string MUSIC_PREF_KEY = "MusicPreference";
        private const int MUSIC_OFF = 0;
        private const int MUSIC_ON = 1;
        private const string SOUND_PREF_KEY = "SoundPreference";
        private const int SOUND_OFF = 0;
        private const int SOUND_ON = 1;

        void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(gameObject);
            }
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        void Start()
        {
            SetMusicOn(!IsMusicOff());
            SetSoundOn(!IsSoundOff());
        }
        public void PlaySound(Sound sound, bool isSpecialSound = false, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            StartCoroutine(CRPlaySound(sound, isSpecialSound, autoScaleVolume, maxVolumeScale));
        }

        IEnumerator CRPlaySound(Sound sound, bool isSpecialSound = false, bool autoScaleVolume = true, float maxVolumeScale = 1f)
        {
            if (sound.simultaneousPlayCount >= maxSimultaneousSounds)
            {
                yield break;
            }

            sound.simultaneousPlayCount++;

            float vol = maxVolumeScale;

            // Scale down volume of same sound played subsequently
            if (autoScaleVolume && sound.simultaneousPlayCount > 0)
            {
                vol = vol / (float)(sound.simultaneousPlayCount);
            }

            AudioSource src = null;
            if (isSpecialSound)
                src = specialSfxSource;
            if (src == null)
                src = sfxSource;

            src.PlayOneShot(sound.clip, vol);

            // Wait til the sound almost finishes playing then reduce play count
            float delay = sound.clip.length * 0.2f;

            yield return new WaitForSeconds(delay);

            sound.simultaneousPlayCount--;
        }

        /// <summary>
        /// Plays the given music.
        /// </summary>
        /// <param name="music">Music.</param>
        /// <param name="loop">If set to <c>true</c> loop.</param>
        public void PlayMusic(Sound music, bool loop = true)
        {
            int randomMusic = Random.Range(0, 3);
            Debug.Log("Music Selected"+randomMusic);
            if(randomMusic==0)
            {
                bgmSource.clip = music.clips[0];
            }
            if(randomMusic==1)
            {
                bgmSource.clip = music.clips[1];
            }
            if (randomMusic == 2)
            {
                bgmSource.clip = music.clips[2];
            }
            if (randomMusic == 3)
            {
                bgmSource.clip = music.clips[3];
            }
            bgmSource.loop = loop;
            bgmSource.Play();
            musicState = PlayingState.Playing;
        }

        /// <summary>
        /// Pauses the music.
        /// </summary>
        public void PauseMusic()
        {
            if (musicState == PlayingState.Playing)
            {
                bgmSource.Pause();
                musicState = PlayingState.Paused;
            }
        }

        /// <summary>
        /// Resumes the music.
        /// </summary>
        public void ResumeMusic()
        {
            if (musicState == PlayingState.Paused)
            {
                bgmSource.UnPause();
                musicState = PlayingState.Playing;
            }
        }

        /// <summary>
        /// Stop music.
        /// </summary>
        public void StopMusic()
        {
            bgmSource.Stop();
            musicState = PlayingState.Stopped;
        }

        public bool IsMusicOff()
        {
            return PlayerPrefs.GetInt(MUSIC_PREF_KEY, MUSIC_ON) == MUSIC_OFF;
        }

        public void SetMusicOn(bool isOn)
        {
            int lastStatus = PlayerPrefs.GetInt(MUSIC_PREF_KEY, MUSIC_ON);
            int status = isOn ? 1 : 0;

            PlayerPrefs.SetInt(MUSIC_PREF_KEY, status);
            bgmSource.mute = !isOn;

            if (lastStatus != status)
            {
                if (MusicStatusChanged != null)
                    MusicStatusChanged(isOn);
            }
        }

        /// <summary>
        /// Toggles the mute status.
        /// </summary>
        public void ToggleMusic()
        {
            if (IsMusicOff())
            {
                // Turn music ON
                SetMusicOn(true);
                if (musicState == PlayingState.Paused)
                {
                    ResumeMusic();
                }
            }
            else
            {
                // Turn music OFF
                SetMusicOn(false);
                if (musicState == PlayingState.Playing)
                {
                    PauseMusic();
                }
            }
        }

        public bool IsSoundOff()
        {
            return PlayerPrefs.GetInt(SOUND_PREF_KEY, SOUND_ON) == SOUND_OFF;
        }

        public void SetSoundOn(bool isOn)
        {
            int lastStatus = PlayerPrefs.GetInt(SOUND_PREF_KEY, SOUND_ON);
            int status = isOn ? 1 : 0;

            PlayerPrefs.SetInt(SOUND_PREF_KEY, status);
            sfxSource.mute = !isOn;
            if (specialSfxSource)
                specialSfxSource.mute = !isOn;

            if (lastStatus != status)
            {
                if (SoundStatusChanged != null)
                    SoundStatusChanged(isOn);
            }
        }

        public void ToggleSound()
        {
            if (IsSoundOff())
            {
                SetSoundOn(true);
            }
            else
            {
                SetSoundOn(false);
            }
        }
    }
}
