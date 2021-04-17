/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tayx.Graphy.Audio
{
    /// <summary>
    /// Note: this class only works with Unity's AudioListener.
    /// If you're using a custom audio engine (like FMOD or WWise) it won't work,
    /// although you can always adapt it.
    /// </summary>
    public class G_AudioMonitor : MonoBehaviour
    {
        #region Variables -> Private

        private const   float                               m_refValue                          = 1f;

        private         GraphyManager                       m_graphyManager                     = null;

        private         AudioListener                       m_audioListener                     = null;

        private         GraphyManager.LookForAudioListener  m_findAudioListenerInCameraIfNull   = GraphyManager.LookForAudioListener.ON_SCENE_LOAD;

        private         FFTWindow                           m_FFTWindow                         = FFTWindow.Blackman;

        private         int                                 m_spectrumSize                      = 512;

        #endregion

        #region Properties -> Public

        /// <summary>
        /// Current audio spectrum from the specified AudioListener.
        /// </summary>
        public float[] Spectrum { get; private set; }

        /// <summary>
        /// Highest audio spectrum from the specified AudioListener in the last few seconds.
        /// </summary>
        public float[] SpectrumHighestValues { get; private set; }

        /// <summary>
        /// Maximum DB registered in the current spectrum.
        /// </summary>
        public float MaxDB { get; private set; }

        /// <summary>
        /// Returns true if there is a reference to the audio listener.
        /// </summary>
        public bool SpectrumDataAvailable => m_audioListener != null;

        #endregion

        #region Methods -> Unity Callbacks

        private void Awake()
        {
            Init();
        }

        private void Update()
        {
            if (m_audioListener != null)
            {
                // Use this data to calculate the dB value

                AudioListener.GetOutputData(Spectrum, 0);

                float sum = 0;

                for (int i = 0; i < Spectrum.Length; i++)
                {
                    sum += Spectrum[i] * Spectrum[i]; // sum squared samples
                }

                float rmsValue = Mathf.Sqrt(sum / Spectrum.Length); // rms = square root of average

                MaxDB = 20 * Mathf.Log10(rmsValue / m_refValue); // calculate dB

                if (MaxDB < -80) MaxDB = -80; // clamp it to -80dB min

                // Use this data to draw the spectrum in the graphs

                AudioListener.GetSpectrumData(Spectrum, 0, m_FFTWindow);

                for (int i = 0; i < Spectrum.Length; i++)
                {
                    // Update the highest value if its lower than the current one
                    if (Spectrum[i] > SpectrumHighestValues[i])
                    {
                        SpectrumHighestValues[i] = Spectrum[i];
                    }

                    // Slowly lower the value 
                    else
                    {
                        SpectrumHighestValues[i] = Mathf.Clamp
                        (
                            value: SpectrumHighestValues[i] - SpectrumHighestValues[i] * Time.deltaTime * 2,
                            min: 0,
                            max: 1
                        );
                    }
                }
            }
            else if(     m_audioListener == null 
                     &&  m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ALWAYS)
            {
                m_audioListener = FindAudioListener();
            }
        }

        private void OnDestroy()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region Methods -> Public

        public void UpdateParameters()
        {
            m_findAudioListenerInCameraIfNull   = m_graphyManager.FindAudioListenerInCameraIfNull;

            m_audioListener                     = m_graphyManager.AudioListener;
            m_FFTWindow                         = m_graphyManager.FftWindow;
            m_spectrumSize                      = m_graphyManager.SpectrumSize;

            if (m_audioListener == null
                    && m_findAudioListenerInCameraIfNull != GraphyManager.LookForAudioListener.NEVER)
            {
                m_audioListener = FindAudioListener();
            }

            Spectrum              = new float[m_spectrumSize];
            SpectrumHighestValues = new float[m_spectrumSize];
        }

        /// <summary>
        /// Converts spectrum values to decibels using logarithms.
        /// </summary>
        /// <param name="linear"></param>
        /// <returns></returns>
        public float lin2dB(float linear)
        {
            return Mathf.Clamp(Mathf.Log10(linear) * 20.0f, -160.0f, 0.0f);
        }

        /// <summary>
        /// Normalizes a value in decibels between 0-1.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public float dBNormalized(float db)
        {
            return (db + 160f) / 160f;
        }

        #endregion

        #region Methods -> Private

        /// <summary>
        /// Tries to find an audio listener in the main camera.
        /// </summary>
        private AudioListener FindAudioListener()
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null && mainCamera.TryGetComponent(out AudioListener audioListener))
            {
                return audioListener;
            }

            return null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ON_SCENE_LOAD)
            {
                m_audioListener = FindAudioListener();
            }
        }

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();
            
            UpdateParameters();

            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }

        #endregion
    }
}