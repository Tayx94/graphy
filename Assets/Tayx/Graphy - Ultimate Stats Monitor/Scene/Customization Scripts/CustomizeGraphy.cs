/* ---------------------------------------
 * Author: Martin Pane (martintayx@gmail.com) (@tayx94)
 * Project: Graphy - Ultimate Stats Monitor
 * Date: 28-Feb-18
 * Studio: Tayx
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using System;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Tayx.Graphy.CustomizationScene
{
    public class CustomizeGraphy : MonoBehaviour
    {
        [Header("Customize Graphy")]

        [SerializeField] private CUIColorPicker m_colorPicker;
        
        [SerializeField] private Toggle m_backgroundToggle;

        [SerializeField] private Dropdown m_graphyModeDropdown;

        [SerializeField] private Button m_backgroundColorButton;
        
        [SerializeField] private Dropdown m_graphModulePositionDropdown;

        // FPS -------------------------------------------
        
        [Header("Fps")]

        [SerializeField] private Dropdown m_fpsModuleStateDropdown;

        [SerializeField] private InputField m_goodInputField;
        [SerializeField] private InputField m_cautionInputField;
        
        [SerializeField] private Button m_goodColorButton;
        [SerializeField] private Button m_cautionColorButton;
        [SerializeField] private Button m_criticalColorButton;
        
        [SerializeField] private Slider m_timeToResetMinMaxSlider;
        [SerializeField] private Slider m_fpsGraphResolutionSlider;
        [SerializeField] private Slider m_fpsTextUpdateRateSlider;
        
        // MEMORY -------------------------------------------

        [Header("Memory")]

        [SerializeField] private Dropdown m_ramModuleStateDropdown;
        
        [SerializeField] private Button m_reservedColorButton;
        [SerializeField] private Button m_allocatedColorButton;
        [SerializeField] private Button m_monoColorButton;

        [SerializeField] private Slider m_ramGraphResolutionSlider;
        [SerializeField] private Slider m_ramTextUpdateRateSlider;
        
        // AUDIO -------------------------------------------

        [Header("Audio")]
        
        [SerializeField] private Dropdown m_audioModuleStateDropdown;
        
        [SerializeField] private Button m_audioGraphColorButton;
        
        [SerializeField] private Dropdown m_findAudioListenerDropdown;
        [SerializeField] private Dropdown m_fttWindowDropdown;
        
        [SerializeField] private Slider m_spectrumSizeSlider;
        [SerializeField] private Slider m_audioGraphResolutionSlider;
        [SerializeField] private Slider m_audioTextUpdateRateSlider;
        
        // ADVANCED -------------------------------------------

        [Header("Advanced")]

        [SerializeField] private Dropdown m_advancedModulePositionDropdown;

        [SerializeField] private Toggle m_advancedModuleToggle;
        
        // OTHER ---------------------------------------------

        [Header("Other")]
        
        [SerializeField] private Button m_musicButton;
        [SerializeField] private Button m_sfxButton;
        
        [SerializeField] private Slider m_musicVolumeSlider;
        [SerializeField] private Slider m_sfxVolumeSlider;


        [SerializeField] private AudioSource m_musicAudioSource;
        [SerializeField] private AudioSource m_sfxAudioSource;
        
        [SerializeField] private List<AudioClip> m_sfxAudioClips = new List<AudioClip>();

        private GraphyManager m_graphyManager;

        // Unity Methods --------------------------------

        private void Start()
        {
            m_graphyManager = GraphyManager.Instance;
            
            SetupCallbacks();
        }

        // Private Methods -------------------------------

        private void SetupCallbacks()
        {
            m_backgroundToggle.onValueChanged.AddListener(value => m_graphyManager.Background = value);
            
            m_backgroundColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_backgroundColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_backgroundColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.BackgroundColor = color;
                });
            });

            m_graphyModeDropdown.onValueChanged.AddListener
            (
                value => 
                {
                    switch ((GraphyManager.Mode)value)
                    {
                        case GraphyManager.Mode.FULL:
                            m_fpsGraphResolutionSlider.maxValue = 300;
                            m_ramGraphResolutionSlider.maxValue = 300;
                            m_audioGraphResolutionSlider.maxValue = 300;
                            break;

                        case GraphyManager.Mode.LIGHT:
                            m_fpsGraphResolutionSlider.maxValue = 128;
                            m_ramGraphResolutionSlider.maxValue = 128;
                            m_audioGraphResolutionSlider.maxValue = 128;
                            break;
                    }

                    m_graphyManager.GraphyMode = (GraphyManager.Mode)value;                    
                }
               
            );


            m_graphModulePositionDropdown.onValueChanged.AddListener(
                value => m_graphyManager.GraphModulePosition = (GraphyManager.ModulePosition)value);
            
            // FPS
            
            m_fpsModuleStateDropdown.onValueChanged.AddListener(
                value => m_graphyManager.FpsModuleState = (GraphyManager.ModuleState)value);

            m_goodInputField.onValueChanged.AddListener(value =>
            {
                int threshold;
                if (Int32.TryParse(value, out threshold))
                {
                    m_graphyManager.GoodFPSThreshold = threshold;
                }
            });
            m_cautionInputField.onValueChanged.AddListener(value =>
            {
                int threshold;
                if (Int32.TryParse(value, out threshold))
                {
                    m_graphyManager.CautionFPSThreshold = threshold;
                }
            });
            
            m_goodColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_goodColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_goodColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.GoodFPSColor = color;
                });
            });
            
            m_cautionColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_cautionColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_cautionColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.CautionFPSColor = color;
                });
            });
            
            m_criticalColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_criticalColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_criticalColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.CriticalFPSColor = color;
                });
            });
            
            m_timeToResetMinMaxSlider.onValueChanged.AddListener(value => m_graphyManager.TimeToResetMinMaxFps = (int)value);
            m_fpsGraphResolutionSlider.onValueChanged.AddListener(value => m_graphyManager.FpsGraphResolution = (int)value);
            m_fpsTextUpdateRateSlider.onValueChanged.AddListener(value => m_graphyManager.FpsTextUpdateRate = (int)value);
            
            // RAM
            
            m_ramModuleStateDropdown.onValueChanged.AddListener(
                value => m_graphyManager.RamModuleState = (GraphyManager.ModuleState)value);

           
            m_reservedColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_reservedColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_reservedColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.ReservedRamColor = color;
                });
            });
            
            m_allocatedColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_allocatedColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_allocatedColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.AllocatedRamColor = color;
                });
            });
            
            m_monoColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_monoColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_monoColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.MonoRamColor = color;
                });
            });
            
            m_ramGraphResolutionSlider.onValueChanged.AddListener(value => m_graphyManager.RamGraphResolution = (int)value);
            m_ramTextUpdateRateSlider.onValueChanged.AddListener(value => m_graphyManager.RamTextUpdateRate = (int)value);
            
            // AUDIO
            
            m_audioModuleStateDropdown.onValueChanged.AddListener(
                value => m_graphyManager.AudioModuleState = (GraphyManager.ModuleState)value);

           
            m_audioGraphColorButton.onClick.AddListener(() =>
            {
                m_colorPicker.SetOnValueChangeCallback(null);
                m_colorPicker.Color = m_audioGraphColorButton.GetComponent<Image>().color;
                m_colorPicker.SetOnValueChangeCallback(color =>
                {
                    m_audioGraphColorButton.GetComponent<Image>().color = color;
                    m_graphyManager.AudioGraphColor = color;
                });
            });
            
            m_findAudioListenerDropdown.onValueChanged.AddListener(
                value => m_graphyManager.FindAudioListenerInCameraIfNull = (GraphyManager.LookForAudioListener)value);
            
            m_fttWindowDropdown.onValueChanged.AddListener(
                value => m_graphyManager.FftWindow = (FFTWindow)value);
            
            m_spectrumSizeSlider.onValueChanged.AddListener(value => m_graphyManager.SpectrumSize = (int)value);           
            m_audioGraphResolutionSlider.onValueChanged.AddListener(value => m_graphyManager.AudioGraphResolution = (int)value);
            m_audioTextUpdateRateSlider.onValueChanged.AddListener(value => m_graphyManager.AudioTextUpdateRate = (int)value);
            
            // ADVANCED
            
            m_advancedModulePositionDropdown.onValueChanged.AddListener(
                value => m_graphyManager.AdvancedModulePosition = (GraphyManager.ModulePosition)value);
            
            m_advancedModuleToggle.onValueChanged.AddListener(
                value => m_graphyManager.AdvancedModuleState = value ? GraphyManager.ModuleState.FULL : GraphyManager.ModuleState.OFF);
            
            // OTHER
            
            m_musicButton.onClick.AddListener(ToggleMusic);
            m_sfxButton.onClick.AddListener(PlayRandomSFX);
            
            m_musicVolumeSlider.onValueChanged.AddListener(value => m_musicAudioSource.volume = value / 100f);
            m_sfxVolumeSlider.onValueChanged.AddListener(value =>  m_sfxAudioSource.volume = value / 100f);
        }

        private void ToggleMusic()
        {
            if (m_musicAudioSource.isPlaying)
            {
                m_musicAudioSource.Pause();
            }
            else
            {
                m_musicAudioSource.Play();
            }
        }

        private void PlayRandomSFX()
        {
            if (m_sfxAudioClips.Count > 0)
            {
                m_sfxAudioSource.clip = m_sfxAudioClips[UnityEngine.Random.Range(0, m_sfxAudioClips.Count)];
                m_sfxAudioSource.Play();
            }
        }
    }

}