﻿/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using System;
using UnityEngine;
using System.Collections;
using Tayx.Graphy.Audio;
using Tayx.Graphy.Fps;
using Tayx.Graphy.Ram;
using Tayx.Graphy.Utils;
using Tayx.Graphy.Advanced;

namespace Tayx.Graphy
{
    //[ExecuteInEditMode]
    public class GraphyManager : Singleton<GraphyManager>
    {
        /* ----- TODO: ----------------------------
         * Check if we can seal this class.
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Check if we can remove "using System.Collections;".
         * Check if we should add "private" to the Unity Callbacks.
         * --------------------------------------*/

        protected GraphyManager () { }

        //Enums
        #region Variables -> Public

        public enum Mode
        {
            FULL            = 0,
            LIGHT           = 1
        }

        public enum ModuleType
        {
            FPS             = 0,
            RAM             = 1,
            AUDIO           = 2,
            ADVANCED        = 3
        }

        public enum ModuleState
        {
            FULL            = 0,
            TEXT            = 1,
            BASIC           = 2,
            BACKGROUND      = 3,
            OFF             = 4
        }

        public enum ModulePosition
        {
            TOP_RIGHT       = 0,
            TOP_LEFT        = 1,
            BOTTOM_RIGHT    = 2,
            BOTTOM_LEFT     = 3
        }

        public enum LookForAudioListener
        {
            ALWAYS,
            ON_SCENE_LOAD,
            NEVER
        }

        public enum ModulePreset
        {
            FPS_BASIC   = 0,
            FPS_TEXT    = 1,
            FPS_FULL    = 2,

            FPS_TEXT_RAM_TEXT = 3,
            FPS_FULL_RAM_TEXT = 4,
            FPS_FULL_RAM_FULL = 5,

            FPS_TEXT_RAM_TEXT_AUDIO_TEXT = 6,
            FPS_FULL_RAM_TEXT_AUDIO_TEXT = 7,
            FPS_FULL_RAM_FULL_AUDIO_TEXT = 8,
            FPS_FULL_RAM_FULL_AUDIO_FULL = 9,

            FPS_FULL_RAM_FULL_AUDIO_FULL_ADVANCED_FULL = 10,
            FPS_BASIC_ADVANCED_FULL = 11
        }

        #endregion

        #region Variables -> Serialized Private

        [SerializeField] private    Mode                    m_graphyMode                        = Mode.FULL;

        [SerializeField] private    bool                    m_enableOnStartup                   = true;

        [SerializeField] private    bool                    m_keepAlive                         = true;
        
        [SerializeField] private    bool                    m_background                        = true;
        [SerializeField] private    Color                   m_backgroundColor                   = new Color(0, 0, 0, 0.3f);

        [SerializeField] private    bool                    m_enableHotkeys                     = true;

        [SerializeField] private    KeyCode                 m_toggleModeKeyCode                 = KeyCode.G;
        [SerializeField] private    bool                    m_toggleModeCtrl                    = true;
        [SerializeField] private    bool                    m_toggleModeAlt                     = false;

        [SerializeField] private    KeyCode                 m_toggleActiveKeyCode               = KeyCode.H;
        [SerializeField] private    bool                    m_toggleActiveCtrl                  = true;
        [SerializeField] private    bool                    m_toggleActiveAlt                   = false;
        
        [SerializeField] private    ModulePosition          m_graphModulePosition               = ModulePosition.TOP_RIGHT;
        
        // Fps ---------------------------------------------------------------------------

        [SerializeField] private    ModuleState             m_fpsModuleState                    = ModuleState.FULL;

        [Range(0, 200)]
        [Tooltip("Time (in seconds) to reset the minimum and maximum framerates if they don't change in the specified time. Set to 0 if you don't want it to reset.")]
        [SerializeField] private    int                     m_timeToResetMinMaxFps              = 10;

        [SerializeField] private    Color                   m_goodFpsColor                      = new Color32(118, 212, 58, 255);
        [SerializeField] private    int                     m_goodFpsThreshold                  = 60;

        [SerializeField] private    Color                   m_cautionFpsColor                   = new Color32(243, 232, 0, 255);
        [SerializeField] private    int                     m_cautionFpsThreshold               = 30;

        [SerializeField] private    Color                   m_criticalFpsColor                  = new Color32(220, 41, 30, 255);

        [Range(10, 300)]
        [SerializeField] private    int                     m_fpsGraphResolution                = 150;

        [Range(1, 200)]
        [SerializeField] private    int                     m_fpsTextUpdateRate                 = 3;  // 3 updates per sec.

        // Ram ---------------------------------------------------------------------------

        [SerializeField] private    ModuleState             m_ramModuleState                    = ModuleState.FULL;

        [SerializeField] private    Color                   m_allocatedRamColor                 = new Color32(255, 190, 60, 255);
        [SerializeField] private    Color                   m_reservedRamColor                  = new Color32(205, 84, 229, 255);
        [SerializeField] private    Color                   m_monoRamColor                      = new Color(0.3f, 0.65f, 1f, 1);

        [Range(10, 300)]
        [SerializeField] private    int                     m_ramGraphResolution                = 150;


        [Range(1, 200)]
        [SerializeField] private    int                     m_ramTextUpdateRate                 = 3;  // 3 updates per sec.

        // Audio -------------------------------------------------------------------------

        [SerializeField] private    ModuleState             m_audioModuleState                  = ModuleState.FULL;

        [SerializeField] private    LookForAudioListener    m_findAudioListenerInCameraIfNull   = LookForAudioListener.ON_SCENE_LOAD;

        [SerializeField] private    AudioListener           m_audioListener;
        
        [SerializeField] private    Color                   m_audioGraphColor                   = Color.white;

        [Range(10, 300)]
        [SerializeField] private    int                     m_audioGraphResolution              = 81;
        
        [Range(1, 200)]
        [SerializeField] private    int                     m_audioTextUpdateRate               = 3;  // 3 updates per sec.
        
        [SerializeField] private    FFTWindow               m_FFTWindow                         = FFTWindow.Blackman;

        [Tooltip("Must be a power of 2 and between 64-8192")]
        [SerializeField] private    int                     m_spectrumSize                      = 512;

        // Advanced ----------------------------------------------------------------------

        [SerializeField] private    ModulePosition          m_advancedModulePosition            = ModulePosition.BOTTOM_LEFT;

        [SerializeField] private    ModuleState             m_advancedModuleState               = ModuleState.FULL;

        #endregion

        #region Variables -> Private

        private                     bool                    m_initialized                       = false;
        private                     bool                    m_active                            = true;
        private                     bool                    m_focused                           = true;

        private                     FpsManager              m_fpsManager;
        private                     RamManager              m_ramManager;
        private                     AudioManager            m_audioManager;
        private                     AdvancedData            m_advancedData;

        private                     FpsMonitor              m_fpsMonitor;
        private                     RamMonitor              m_ramMonitor;
        private                     AudioMonitor            m_audioMonitor;

        private                     ModulePreset            m_modulePresetState                 = ModulePreset.FPS_BASIC_ADVANCED_FULL;

        #endregion

        //TODO: Maybe sort these into Get and GetSet sections.
        #region Properties -> Public

        public Mode GraphyMode                          { get { return m_graphyMode; }
                                                          set { m_graphyMode = value; UpdateAllParameters(); } }

        public bool EnableOnStartup                     { get { return m_enableOnStartup; } }

        public bool KeepAlive                           { get { return m_keepAlive; } }

        public bool Background                          { get { return m_background; } 
                                                          set { m_background = value; UpdateAllParameters(); } }

        public Color BackgroundColor                    { get { return m_backgroundColor; } 
                                                          set { m_backgroundColor = value; UpdateAllParameters(); } }

        public ModulePosition GraphModulePosition
        {
            get { return m_graphModulePosition; }
            set
            {
                m_graphModulePosition = value;
                m_fpsManager    .SetPosition(m_graphModulePosition);
                m_ramManager    .SetPosition(m_graphModulePosition);
                m_audioManager  .SetPosition(m_graphModulePosition);
            }
        }

        // Fps ---------------------------------------------------------------------------

        // Setters & Getters

        public ModuleState FpsModuleState               { get { return m_fpsModuleState; }             
                                                          set { m_fpsModuleState = value; m_fpsManager.SetState(m_fpsModuleState); } }

        public int TimeToResetMinMaxFps                 { get { return m_timeToResetMinMaxFps; }       
                                                          set { m_timeToResetMinMaxFps = value; m_fpsManager.UpdateParameters(); } }

        public Color GoodFPSColor                       { get { return m_goodFpsColor; } 
                                                          set { m_goodFpsColor = value; m_fpsManager.UpdateParameters(); } }
        public Color CautionFPSColor                    { get { return m_cautionFpsColor; } 
                                                          set { m_cautionFpsColor = value; m_fpsManager.UpdateParameters(); } }
        public Color CriticalFPSColor                   { get { return m_criticalFpsColor; } 
                                                          set { m_criticalFpsColor = value; m_fpsManager.UpdateParameters(); } }

        public int GoodFPSThreshold                     { get { return m_goodFpsThreshold; } 
                                                          set { m_goodFpsThreshold = value; m_fpsManager.UpdateParameters(); } }
        public int CautionFPSThreshold                  { get { return m_cautionFpsThreshold; } 
                                                          set { m_cautionFpsThreshold = value; m_fpsManager.UpdateParameters(); } }

        public int FpsGraphResolution                   { get { return m_fpsGraphResolution; } 
                                                          set { m_fpsGraphResolution = value; m_fpsManager.UpdateParameters(); } }

        public int FpsTextUpdateRate                    { get { return m_fpsTextUpdateRate; } 
                                                          set { m_fpsTextUpdateRate = value; m_fpsManager.UpdateParameters(); } }

        // Getters

        public float CurrentFPS                         { get { return m_fpsMonitor.CurrentFPS; } }
        public float AverageFPS                         { get { return m_fpsMonitor.AverageFPS; } }
        public float MinFPS                             { get { return m_fpsMonitor.MinFPS; } }
        public float MaxFPS                             { get { return m_fpsMonitor.MaxFPS; } }

        // Ram ---------------------------------------------------------------------------

        // Setters & Getters

        public ModuleState RamModuleState               { get { return m_ramModuleState; } 
                                                          set { m_ramModuleState = value; m_ramManager.SetState(m_ramModuleState); } }


        public Color AllocatedRamColor                  { get { return m_allocatedRamColor; } 
                                                          set { m_allocatedRamColor = value; m_ramManager.UpdateParameters(); } }
        public Color ReservedRamColor                   { get { return m_reservedRamColor; } 
                                                          set { m_reservedRamColor = value; m_ramManager.UpdateParameters(); } }
        public Color MonoRamColor                       { get { return m_monoRamColor; } 
                                                          set { m_monoRamColor = value; m_ramManager.UpdateParameters(); } }

        public int RamGraphResolution                   { get { return m_ramGraphResolution; } 
                                                          set { m_ramGraphResolution = value; m_ramManager.UpdateParameters(); } }

        public int RamTextUpdateRate                    { get { return m_ramTextUpdateRate; } 
                                                          set { m_ramTextUpdateRate = value; m_ramManager.UpdateParameters(); } }

        // Getters

        public float AllocatedRam                       { get { return m_ramMonitor.AllocatedRam; } }
        public float ReservedRam                        { get { return m_ramMonitor.ReservedRam; } }
        public float MonoRam                            { get { return m_ramMonitor.MonoRam; } }

        // Audio -------------------------------------------------------------------------

        // Setters & Getters

        public ModuleState AudioModuleState             { get { return m_audioModuleState; } 
                                                          set { m_audioModuleState = value; m_audioManager.SetState(m_audioModuleState); } }

        public AudioListener AudioListener              { get { return m_audioListener; } 
                                                          set { m_audioListener = value; m_audioManager.UpdateParameters(); } }
        
        public LookForAudioListener 
                    FindAudioListenerInCameraIfNull     { get { return m_findAudioListenerInCameraIfNull; } 
                                                          set { m_findAudioListenerInCameraIfNull = value; m_audioManager.UpdateParameters(); } }

        public Color AudioGraphColor                    { get { return m_audioGraphColor; } 
                                                          set { m_audioGraphColor = value; m_audioManager.UpdateParameters(); } }

        public int AudioGraphResolution                 { get { return m_audioGraphResolution; } 
                                                          set { m_audioGraphResolution = value; m_audioManager.UpdateParameters(); } }

        public int AudioTextUpdateRate                  { get { return m_audioTextUpdateRate; } 
                                                          set { m_audioTextUpdateRate = value; m_audioManager.UpdateParameters(); } }

        public FFTWindow FftWindow                      { get { return m_FFTWindow; } 
                                                          set { m_FFTWindow = value; m_audioManager.UpdateParameters(); } }

        public int SpectrumSize                         { get { return m_spectrumSize; } 
                                                          set { m_spectrumSize = value; m_audioManager.UpdateParameters(); } }

        // Getters

        /// <summary>
        /// Current audio spectrum from the specified AudioListener.
        /// </summary>
        public float[] Spectrum                         { get { return m_audioMonitor.Spectrum; } }

        /// <summary>
        /// Maximum DB registered in the current spectrum.
        /// </summary>
        public float MaxDB                              { get { return m_audioMonitor.MaxDB; } }


        // Advanced ---------------------------------------------------------------------

        // Setters & Getters

        public ModuleState AdvancedModuleState          { get { return m_advancedModuleState; } 
                                                          set { m_advancedModuleState = value; m_advancedData.SetState(m_advancedModuleState); } }
        
        public ModulePosition AdvancedModulePosition    { get { return m_advancedModulePosition; } 
                                                          set { m_advancedModulePosition = value; m_advancedData.SetPosition(m_advancedModulePosition); } }

        #endregion

        #region Methods -> Unity Callbacks

        void Start()
        {
            Init();
        }

        void Update()
        {
            if (m_focused && m_enableHotkeys)
            {
                CheckForHotkeyPresses();
            }
        }

        void OnApplicationFocus(bool isFocused)
        {
            m_focused = isFocused;

            if (m_initialized && isFocused)
            {
                RefreshAllParameters();
            }
        }

        #endregion

        #region Methods -> Public

        public void SetModulePosition(ModuleType moduleType, ModulePosition modulePosition)
        {
            switch (moduleType)
            {
                case ModuleType.FPS:
                case ModuleType.RAM:
                case ModuleType.AUDIO:
                    m_graphModulePosition = modulePosition;

                    m_ramManager.SetPosition(modulePosition);
                    m_fpsManager.SetPosition(modulePosition);
                    m_audioManager.SetPosition(modulePosition);
                    break;

                case ModuleType.ADVANCED:
                    m_advancedData.SetPosition(modulePosition);
                    break;
            }
        }

        public void SetModuleMode(ModuleType moduleType, ModuleState moduleState)
        {
            switch (moduleType)
            {
                case ModuleType.FPS:
                    m_fpsManager.SetState(moduleState);
                    break;

                case ModuleType.RAM:
                    m_ramManager.SetState(moduleState);
                    break;

                case ModuleType.AUDIO:
                    m_audioManager.SetState(moduleState);
                    break;

                case ModuleType.ADVANCED:
                    m_advancedData.SetState(moduleState);
                    break;
            }
        }

        public void ToggleModes()
        {
            if ((int)m_modulePresetState >= Enum.GetNames(typeof(ModulePreset)).Length - 1)
            {
                m_modulePresetState = 0;
            }
            else
            {
                m_modulePresetState++;
            }

            SetPreset(m_modulePresetState);
        }

        public void SetPreset(ModulePreset modulePreset)
        {
            m_modulePresetState = modulePreset;
            
            switch (m_modulePresetState)
            {
                case ModulePreset.FPS_BASIC:
                    m_fpsManager.SetState(ModuleState.BASIC);
                    m_ramManager.SetState(ModuleState.OFF);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_TEXT:
                    m_fpsManager.SetState(ModuleState.TEXT);
                    m_ramManager.SetState(ModuleState.OFF);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.OFF);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_TEXT_RAM_TEXT:
                    m_fpsManager.SetState(ModuleState.TEXT);
                    m_ramManager.SetState(ModuleState.TEXT);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL_RAM_TEXT:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.TEXT);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL_RAM_FULL:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.FULL);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_TEXT_RAM_TEXT_AUDIO_TEXT:
                    m_fpsManager.SetState(ModuleState.TEXT);
                    m_ramManager.SetState(ModuleState.TEXT);
                    m_audioManager.SetState(ModuleState.TEXT);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL_RAM_TEXT_AUDIO_TEXT:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.TEXT);
                    m_audioManager.SetState(ModuleState.TEXT);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL_RAM_FULL_AUDIO_TEXT:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.FULL);
                    m_audioManager.SetState(ModuleState.TEXT);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL_RAM_FULL_AUDIO_FULL:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.FULL);
                    m_audioManager.SetState(ModuleState.FULL);
                    m_advancedData.SetState(ModuleState.OFF);
                    break;

                case ModulePreset.FPS_FULL_RAM_FULL_AUDIO_FULL_ADVANCED_FULL:
                    m_fpsManager.SetState(ModuleState.FULL);
                    m_ramManager.SetState(ModuleState.FULL);
                    m_audioManager.SetState(ModuleState.FULL);
                    m_advancedData.SetState(ModuleState.FULL);
                    break;

                case ModulePreset.FPS_BASIC_ADVANCED_FULL:
                    m_fpsManager.SetState(ModuleState.BASIC);
                    m_ramManager.SetState(ModuleState.OFF);
                    m_audioManager.SetState(ModuleState.OFF);
                    m_advancedData.SetState(ModuleState.FULL);
                    break;

                default:
                    //throw new ArgumentOutOfRangeException();
                    break;
            }
        }

        public void ToggleActive()
        {
            if (!m_active)
            {
                Enable();
            }
            else
            {
                Disable();
            }
        }

        public void Enable()
        {
            m_fpsManager    .RestorePreviousState();
            m_ramManager    .RestorePreviousState();
            m_audioManager  .RestorePreviousState();
            m_advancedData  .RestorePreviousState();

            m_active = true;
        }

        public void Disable()
        {
            m_fpsManager    .SetState(ModuleState.OFF);
            m_ramManager    .SetState(ModuleState.OFF);
            m_audioManager  .SetState(ModuleState.OFF);
            m_advancedData  .SetState(ModuleState.OFF);

            m_active = false;
        }

        #endregion

        #region Methods -> Private

        private void Init()
        {
            if (m_keepAlive)
            {
                DontDestroyOnLoad(transform.root.gameObject);
            }
            
            m_fpsMonitor    = GetComponentInChildren(typeof(FpsMonitor),    true) as FpsMonitor;
            m_ramMonitor    = GetComponentInChildren(typeof(RamMonitor),    true) as RamMonitor;
            m_audioMonitor  = GetComponentInChildren(typeof(AudioMonitor),  true) as AudioMonitor;
            
            m_fpsManager    = GetComponentInChildren(typeof(FpsManager),    true) as FpsManager;
            m_ramManager    = GetComponentInChildren(typeof(RamManager),    true) as RamManager;
            m_audioManager  = GetComponentInChildren(typeof(AudioManager),  true) as AudioManager;
            m_advancedData  = GetComponentInChildren(typeof(AdvancedData),  true) as AdvancedData;

            m_fpsManager    .SetPosition(m_graphModulePosition);
            m_ramManager    .SetPosition(m_graphModulePosition);
            m_audioManager  .SetPosition(m_graphModulePosition);
            m_advancedData  .SetPosition(m_advancedModulePosition);

            m_fpsManager    .SetState   (m_fpsModuleState);
            m_ramManager    .SetState   (m_ramModuleState);
            m_audioManager  .SetState   (m_audioModuleState);
            m_advancedData  .SetState   (m_advancedModuleState);

            if (!m_enableOnStartup)
            {
                ToggleActive();
            }

            m_initialized = true;
        }

        private void CheckForHotkeyPresses()
        {
            // Toggle Mode ---------------------------------------

            if (m_toggleModeCtrl && m_toggleModeAlt)
            {
                if (CheckFor3KeyPress(m_toggleModeKeyCode, KeyCode.LeftControl, KeyCode.LeftAlt)
                    || CheckFor3KeyPress(m_toggleModeKeyCode, KeyCode.RightControl, KeyCode.LeftAlt)
                    || CheckFor3KeyPress(m_toggleModeKeyCode, KeyCode.RightControl, KeyCode.RightAlt)
                    || CheckFor3KeyPress(m_toggleModeKeyCode, KeyCode.LeftControl, KeyCode.RightAlt))
                {
                    ToggleModes();
                }
            }
            else if (m_toggleModeCtrl)
            {
                if (    CheckFor2KeyPress(m_toggleModeKeyCode, KeyCode.LeftControl)
                    ||  CheckFor2KeyPress(m_toggleModeKeyCode, KeyCode.RightControl))
                {
                    ToggleModes();
                }
            }
            else if (m_toggleModeAlt)
            {
                if (    CheckFor2KeyPress(m_toggleModeKeyCode, KeyCode.LeftAlt)
                    ||  CheckFor2KeyPress(m_toggleModeKeyCode, KeyCode.RightAlt))
                {
                    ToggleModes();
                }
            }
            else
            {
                if (CheckFor1KeyPress(m_toggleModeKeyCode))
                {
                    ToggleModes();
                }
            }

            // Toggle Active ---------------------------------------

            if (m_toggleActiveCtrl && m_toggleActiveAlt)
            {
                if (    CheckFor3KeyPress(m_toggleActiveKeyCode, KeyCode.LeftControl, KeyCode.LeftAlt)
                    ||  CheckFor3KeyPress(m_toggleActiveKeyCode, KeyCode.RightControl, KeyCode.LeftAlt)
                    ||  CheckFor3KeyPress(m_toggleActiveKeyCode, KeyCode.RightControl, KeyCode.RightAlt)
                    ||  CheckFor3KeyPress(m_toggleActiveKeyCode, KeyCode.LeftControl, KeyCode.RightAlt))
                {
                    ToggleActive();
                }
            }
            
            else if (m_toggleActiveCtrl)
            {
                if (    CheckFor2KeyPress(m_toggleActiveKeyCode, KeyCode.LeftControl)
                    ||  CheckFor2KeyPress(m_toggleActiveKeyCode, KeyCode.RightControl))
                {
                    ToggleActive();
                }
            }
            else if (m_toggleActiveAlt)
            {
                if (    CheckFor2KeyPress(m_toggleActiveKeyCode, KeyCode.LeftAlt)
                    ||  CheckFor2KeyPress(m_toggleActiveKeyCode, KeyCode.RightAlt))
                {
                    ToggleActive();
                }
            }
            else
            {
                if (CheckFor1KeyPress(m_toggleActiveKeyCode))
                {
                    ToggleActive();
                }
            }
        }

        private bool CheckFor1KeyPress(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        private bool CheckFor2KeyPress(KeyCode key1, KeyCode key2)
        {
            return Input.GetKeyDown(key1) && Input.GetKey(key2)
                || Input.GetKeyDown(key2) && Input.GetKey(key1);
        }

        private bool CheckFor3KeyPress(KeyCode key1, KeyCode key2, KeyCode key3)
        {
            return Input.GetKeyDown(key1) && Input.GetKey(key2) && Input.GetKey(key3)
                || Input.GetKeyDown(key2) && Input.GetKey(key1) && Input.GetKey(key3)
                || Input.GetKeyDown(key3) && Input.GetKey(key1) && Input.GetKey(key2);
        }

        private void UpdateAllParameters()
        {
            m_fpsManager    .UpdateParameters();
            m_ramManager    .UpdateParameters();
            m_audioManager  .UpdateParameters();
            m_advancedData  .UpdateParameters();
        }

        private void RefreshAllParameters()
        {
            m_fpsManager    .RefreshParameters();
            m_ramManager    .RefreshParameters();
            m_audioManager  .RefreshParameters();
            m_advancedData  .RefreshParameters();
        }
        
        #endregion
    }
}