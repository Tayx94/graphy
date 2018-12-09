/* ---------------------------------------
 * Author:          Martin Pane (martintayx@gmail.com) (@tayx94)
 * Collaborators:   Lars Aalbertsen (@Rockylars)
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            15-Dec-17
 * Studio:          Tayx
 * 
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tayx.Graphy.Audio
{
    public class G_AudioMonitor : MonoBehaviour
    {
        /* ----- TODO: ----------------------------
         * Add summaries to the variables.
         * Add summaries to the functions.
         * Make the "FindAudioListener" not constantly use "Camera.main".
         * --------------------------------------*/

        #region Variables -> Private

        private const   float                               m_refValue                          = 1f;

        private         GraphyManager                       m_graphyManager                     = null;

        private         AudioListener                       m_audioListener                     = null;

        private         GraphyManager.LookForAudioListener  m_findAudioListenerInCameraIfNull   = GraphyManager.LookForAudioListener.ON_SCENE_LOAD;

        private         FFTWindow                           m_FFTWindow                         = FFTWindow.Blackman;

        private         int                                 m_spectrumSize                      = 512;

        private         float[]                             m_spectrum;
        private         float[]                             m_spectrumHighestValues;

        private         float                               m_maxDB;

        #endregion

        #region Properties -> Public

        /// <summary>
        /// Current audio spectrum from the specified AudioListener.
        /// </summary>
        public float[] Spectrum                 { get { return m_spectrum; } }

        /// <summary>
        /// Highest audio spectrum from the specified AudioListener in the last few seconds.
        /// </summary>
        public float[] SpectrumHighestValues    { get { return m_spectrumHighestValues; } }

        /// <summary>
        /// Maximum DB registered in the current spectrum.
        /// </summary>
        public float MaxDB                      { get { return m_maxDB; } }

        /// <summary>
        /// Returns true if there is a reference to the audio listener.
        /// </summary>
        public bool SpectrumDataAvailable       {  get { return m_audioListener != null;} }

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

                AudioListener.GetOutputData(m_spectrum, 0);

                float sum = 0;

                for (int i = 0; i < m_spectrum.Length; i++)
                {
                    sum += m_spectrum[i] * m_spectrum[i]; // sum squared samples
                }

                float rmsValue = Mathf.Sqrt(sum / m_spectrum.Length); // rms = square root of average

                m_maxDB = 20 * Mathf.Log10(rmsValue / m_refValue); // calculate dB

                if (m_maxDB < -80) m_maxDB = -80; // clamp it to -80dB min

                // Use this data to draw the spectrum in the graphs

                AudioListener.GetSpectrumData(m_spectrum, 0, m_FFTWindow);

                for (int i = 0; i < m_spectrum.Length; i++)
                {
                    // Update the highest value if its lower than the current one
                    if (m_spectrum[i] > m_spectrumHighestValues[i])
                    {
                        m_spectrumHighestValues[i] = m_spectrum[i];
                    }

                    // Slowly lower the value 
                    else
                    {
                        m_spectrumHighestValues[i] = Mathf.Clamp
                        (
                            value: m_spectrumHighestValues[i] - m_spectrumHighestValues[i] * Time.deltaTime * 2,
                            min: 0,
                            max: 1
                        );
                    }
                }
            }
            else if(     m_audioListener == null 
                     &&  m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ALWAYS)
            {
                FindAudioListener();
            }
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
                FindAudioListener();
            }

            m_spectrum              = new float[m_spectrumSize];
            m_spectrumHighestValues = new float[m_spectrumSize];
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
        private void FindAudioListener()
        {
            m_audioListener = Camera.main.GetComponent<AudioListener>();
        }

        private void Init()
        {
            m_graphyManager = transform.root.GetComponentInChildren<GraphyManager>();
            
            UpdateParameters();

            SceneManager.sceneLoaded += (scene, loadMode) =>
            {
                if (m_findAudioListenerInCameraIfNull == GraphyManager.LookForAudioListener.ON_SCENE_LOAD)
                {
                    FindAudioListener();
                }
            };
        }

        #endregion
    }
}