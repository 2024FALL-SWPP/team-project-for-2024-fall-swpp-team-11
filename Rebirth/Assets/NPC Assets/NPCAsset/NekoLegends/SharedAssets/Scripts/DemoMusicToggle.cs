using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

namespace NekoLegends
{
    [RequireComponent(typeof(AudioSource))]
    public class DemoMusicToggle : MonoBehaviour
    {
        [Space]
        [SerializeField] public List<AudioClip> audioClips;
        [SerializeField] private Button MusicBtn;
        [SerializeField] private TextMeshProUGUI SongText;
        [SerializeField] private Toggle AutoPlayToggle; // Added toggle for auto-play

        private int currentMusicIndex = 0;
        private AudioSource audioSource;
        private bool isAutoPlayEnabled = false;
        private bool isSongFinished = false;

        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void Start()
        {
            PlayMusicAtIndex(currentMusicIndex);
        }

        protected void OnEnable()
        {
            if (MusicBtn)
                MusicBtn.onClick.AddListener(NextMusic);

            if (AutoPlayToggle != null)
            {
                isAutoPlayEnabled = AutoPlayToggle.isOn;
                AutoPlayToggle.onValueChanged.AddListener(OnAutoPlayToggleChanged);
            }
        }

        protected void OnDisable()
        {
            if (MusicBtn)
                MusicBtn.onClick.RemoveListener(NextMusic);

            if (AutoPlayToggle != null)
                AutoPlayToggle.onValueChanged.RemoveListener(OnAutoPlayToggleChanged);
        }

        private void OnAutoPlayToggleChanged(bool value)
        {
            isAutoPlayEnabled = value;
        }

        private void Update()
        {
            if (isAutoPlayEnabled)
            {
                if (!audioSource.isPlaying && !isSongFinished)
                {
                    isSongFinished = true;
                    NextMusic();
                }
            }
        }

        protected void NextMusic()
        {
            currentMusicIndex++;

            if (currentMusicIndex >= audioClips.Count)
            {
                currentMusicIndex = 0;
            }

            PlayMusicAtIndex(currentMusicIndex);
        }

        private void PlayMusicAtIndex(int index)
        {
            if (audioClips.Count > 0 && index < audioClips.Count)
            {
                AudioClip clipToPlay = audioClips[index];
                if (audioSource != null)
                {
                    audioSource.clip = clipToPlay;
                    audioSource.Play();
                    if (SongText)
                        SongText.SetText(audioSource.clip.name);

                    isSongFinished = false; // Reset the flag when a new song starts
                }
                else
                {
                    Debug.LogWarning("AudioSource component missing on this GameObject.");
                }
            }
            else
            {
                Debug.LogWarning("No audio clips available to play or index out of range.");
            }
        }
    }
}
