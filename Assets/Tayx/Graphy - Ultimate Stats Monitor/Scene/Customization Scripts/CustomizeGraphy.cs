﻿/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            28-Feb-18
 * Studio:          Tayx
 * 
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
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we can remove "using Random = UnityEngine.Random;".
         * Check if we can remove the UnityEngine prefix on the PlayRandomSFX function
         * --------------------------------------*/

        #region Variables -> Serialized Private
        
        [Header("Customize Graphy")]

        [SerializeField] private    CUIColorPicker      m_colorPicker;
        
        [SerializeField] private    Toggle              m_backgroundToggle;

        [SerializeField] private    Dropdown            m_graphyModeDropdown;

        [SerializeField] private    Button              m_backgroundColorButton;
        
        [SerializeField] private    Dropdown            m_graphModulePositionDropdown;

        #region Section -> FPS

        [Header("Fps")]

        [SerializeField] private    Dropdown            m_fpsModuleStateDropdown;

        [SerializeField] private    InputField          m_goodInputField;
        [SerializeField] private    InputField          m_cautionInputField;
        
        [SerializeField] private    Button              m_goodColorButton;
        [SerializeField] private    Button              m_cautionColorButton;
        [SerializeField] private    Button              m_criticalColorButton;
        
        [SerializeField] private    Slider              m_timeToResetMinMaxSlider;
        [SerializeField] private    Slider              m_fpsGraphResolutionSlider;
        [SerializeField] private    Slider              m_fpsTextUpdateRateSlider;

        #endregion

        #region Section -> RAM

        [Header("Memory")]

        [SerializeField] private    Dropdown            m_ramModuleStateDropdown;
        
        [SerializeField] private    Button              m_reservedColorButton;
        [SerializeField] private    Button              m_allocatedColorButton;
        [SerializeField] private    Button              m_monoColorButton;

        [SerializeField] private    Slider              m_ramGraphResolutionSlider;
        [SerializeField] private    Slider              m_ramTextUpdateRateSlider;

        #endregion

        #region Section -> Audio

        [Header("Audio")]
        
        [SerializeField] private    Dropdown            m_audioModuleStateDropdown;
        
        [SerializeField] private    Button              m_audioGraphColorButton;
        
        [SerializeField] private    Dropdown            m_findAudioListenerDropdown;
        [SerializeField] private    Dropdown            m_fttWindowDropdown;
        
        [SerializeField] private    Slider              m_spectrumSizeSlider;
        [SerializeField] private    Slider              m_audioGraphResolutionSlider;
        [SerializeField] private    Slider              m_audioTextUpdateRateSlider;

        #endregion

        #region Section -> Advanced

        [Header("Advanced")]

        [SerializeField] private    Dropdown            m_advancedModulePositionDropdown;

        [SerializeField] private    Toggle              m_advancedModuleToggle;

        #endregion

        #region Section -> Other

        [Header("Other")]
        
        [SerializeField] private    Button              m_musicButton;
        [SerializeField] private    Button              m_sfxButton;
        
        [SerializeField] private    Slider              m_musicVolumeSlider;
        [SerializeField] private    Slider              m_sfxVolumeSlider;


        [SerializeField] private    AudioSource         m_musicAudioSource;
        [SerializeField] private    AudioSource         m_sfxAudioSource;
        
        [SerializeField] private    List<AudioClip>     m_sfxAudioClips = new List<AudioClip>();

        #endregion

        #endregion

        #region Variables -> Private

        private GraphyManager m_graphyManager;

        #endregion

        #region Methods -> Unity Callbacks

        private void Start()
        {
            m_graphyManager = GraphyManager.Instance;
            
            SetupCallbacks();
        }

        #endregion

        #region Methods -> Private

        private void SetupCallbacks()
        {
            m_backgroundToggle.onValueChanged.AddListener(
                value => m_graphyManager.Background = value);
            
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

            m_graphyModeDropdown.onValueChanged.AddListener(value => 
            {
                switch ((GraphyManager.Mode)value)
                {
                    case GraphyManager.Mode.FULL:
                        m_fpsGraphResolutionSlider.maxValue     = 300f;
                        m_ramGraphResolutionSlider.maxValue     = 300f;
                        m_audioGraphResolutionSlider.maxValue   = 300f;
                        break;

                    case GraphyManager.Mode.LIGHT:
                        m_fpsGraphResolutionSlider.maxValue     = 128f;
                        m_ramGraphResolutionSlider.maxValue     = 128f;
                        m_audioGraphResolutionSlider.maxValue   = 128f;
                        break;
                }

                m_graphyManager.GraphyMode = (GraphyManager.Mode)value;                    
            });

            m_graphModulePositionDropdown.onValueChanged.AddListener(
                value => m_graphyManager.GraphModulePosition = (GraphyManager.ModulePosition)value);

            #region Section -> FPS

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
            
            m_timeToResetMinMaxSlider.onValueChanged.AddListener(
                value => m_graphyManager.TimeToResetMinMaxFps = (int)value);

            m_fpsGraphResolutionSlider.onValueChanged.AddListener(
                value => m_graphyManager.FpsGraphResolution = (int)value);

            m_fpsTextUpdateRateSlider.onValueChanged.AddListener(
                value => m_graphyManager.FpsTextUpdateRate = (int)value);

            #endregion

            #region Section -> RAM

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
            
            m_ramGraphResolutionSlider.onValueChanged.AddListener(
                value => m_graphyManager.RamGraphResolution = (int)value);

            m_ramTextUpdateRateSlider.onValueChanged.AddListener(
                value => m_graphyManager.RamTextUpdateRate = (int)value);

            #endregion

            #region Section -> Audio

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
            
            m_spectrumSizeSlider.onValueChanged.AddListener(
                value => m_graphyManager.SpectrumSize = (int)value);
            
            m_audioGraphResolutionSlider.onValueChanged.AddListener(
                value => m_graphyManager.AudioGraphResolution = (int)value);

            m_audioTextUpdateRateSlider.onValueChanged.AddListener(
                value => m_graphyManager.AudioTextUpdateRate = (int)value);

            #endregion

            #region Section -> Advanced

            m_advancedModulePositionDropdown.onValueChanged.AddListener(
                value => m_graphyManager.AdvancedModulePosition = (GraphyManager.ModulePosition)value);
            
            m_advancedModuleToggle.onValueChanged.AddListener(
                value => m_graphyManager.AdvancedModuleState = value ? GraphyManager.ModuleState.FULL : GraphyManager.ModuleState.OFF);

            #endregion

            #region Section -> Other

            m_musicButton.onClick.AddListener(ToggleMusic);
            m_sfxButton.onClick.AddListener(PlayRandomSFX);
            
            m_musicVolumeSlider.onValueChanged.AddListener(
                value => m_musicAudioSource.volume = value / 100f);

            m_sfxVolumeSlider.onValueChanged.AddListener(
                value =>  m_sfxAudioSource.volume = value / 100f);

            #endregion
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

        #endregion
    }
}