using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public GameObject settingsMenu;
    public Slider volumeSlider;  // Reference to the slider
    public AudioSource audioSource;  // Reference to the AudioSource
    public Slider masterVolumeSlider; // Reference to the master volume slider

    void Start()
    {
        // Check if the sliders and audioSource are assigned
        if (volumeSlider == null || audioSource == null || masterVolumeSlider == null)
        {
            Debug.LogError("One or more sliders or AudioSource are not assigned.");
            return;
        }

        // Initialize the slider values
        volumeSlider.value = audioSource.volume;
        masterVolumeSlider.value = AudioListener.volume;

        // Add listeners to handle the volume change events
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        masterVolumeSlider.onValueChanged.AddListener(OnVolumeForAllSoundsChanged);
    }

    public void OnVolumeChanged(float value)
    {
        // Change the audio source volume to match the slider value
        audioSource.volume = value;
    }

    public void OnVolumeForAllSoundsChanged(float value)
    {
        // Change the overall volume to match the slider value
        AudioListener.volume = value;
    }

    public void OnEnableSettingBTN() 
    {
        settingsMenu.gameObject.SetActive(true);
    }
}
