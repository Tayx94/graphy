/* ---------------------------------------
 * Author:          Paul Sinnett (paul.sinnett@gmail.com) (@paulsinnett)
 * Contributors:    https://github.com/Tayx94/graphy/graphs/contributors
 * Project:         Graphy - Ultimate Stats Monitor
 * Date:            06-Sep-24
 * Studio:          Powered Up Games
 *
 * Git repo:        https://github.com/Tayx94/graphy
 *
 * This project is released under the MIT license.
 * Attribution is not required, but it is always welcomed!
 * -------------------------------------*/

using UnityEngine.Assertions;

namespace Tayx.Graphy.Utils
{
    public class G_DoubleEndedQueue
    {
        #region Variables -> Private

        /// <summary>
        /// Fixed size array for holding the values.
        /// </summary>
        private short[] m_values;

        /// <summary>
        /// Index of the head element.
        /// </summary>
        private short m_head;

        /// <summary>
        /// Index of the entry after the tail element.
        /// </summary>
        private short m_tail;

        /// <summary>
        /// Number of items in the queue.
        /// </summary>
        private short m_count;

        /// <summary>
        /// Programming error messages for assert failures.
        /// </summary>
        private const string m_errorEmpty = "queue is empty";
        private const string m_errorFull = "queue is full";

        #endregion

        #region Properties -> Public

        /// <summary>
        /// The current number of items in the queue.
        /// </summary>
        public short Count => m_count;

        /// <summary>
        /// True if the queue is currently at full capacity.
        /// </summary>
        public bool Full => m_count == m_values.Length;

        #endregion

        #region Methods -> Public

        /// <summary>
        /// Construct a queue.
        /// </summary>
        /// <param name="capacity">
        /// Maximum number of values in the queue.
        /// </param>
        public G_DoubleEndedQueue( short capacity )
        {
            m_values = new short[ capacity ];
            m_head = 0;
            m_tail = 0;
            m_count = 0;
        }

        /// <summary>
        /// Clear the content of the queue, O(1).
        /// </summary>
        public void Clear()
        {
            m_head = 0;
            m_tail = 0;
            m_count = 0;
        }

        /// <summary>
        /// Add a value to the front of the queue, O(1).
        /// Asserts that the queue is not already full.
        /// </summary>
        /// <param name="value">
        /// The value of the entry.
        /// </param>
        public void PushFront( short value )
        {
            AssertNotFull();
            m_head = Previous( m_head );
            m_values[ m_head ] = value;
            m_count++;
        }

        /// <summary>
        /// Add a value to the back of the queue, O(1).
        /// Asserts that the queue is not already full.
        /// </summary>
        /// <param name="value">
        /// The value of the entry.
        /// </param>
        public void PushBack( short value )
        {
            AssertNotFull();
            m_values[ m_tail ] = value;
            m_tail = Next( m_tail );
            m_count++;
        }

        /// <summary>
        /// Removes the value at the front of the queue, O(1).
        /// Asserts that the queue is not empty.
        /// </summary>
        /// <returns>the removed value</returns>
        public short PopFront()
        {
            AssertNotEmpty();
            short value = m_values[ m_head ];
            m_head = Next( m_head );
            m_count--;
            return value;
        }

        /// <summary>
        /// Removes the value at the back of the queue, O(1).
        /// Asserts that the queue is not empty.
        /// </summary>
        /// <returns>the removed value</returns>
        public short PopBack()
        {
            AssertNotEmpty();
            m_tail = Previous( m_tail );
            short value = m_values[ m_tail ];
            m_count--;
            return value;
        }

        /// <summary>
        /// Returns the value at the front of the queue, O(1).
        /// Asserts that the queue is not empty.
        /// </summary>
        /// <returns>the value at the front of the queue</returns>
        public short PeekFront()
        {
            AssertNotEmpty();
            return m_values[ m_head ];
        }

        /// <summary>
        /// Returns the value at the back of the queue, O(1).
        /// Asserts that the queue is not empty.
        /// </summary>
        /// <returns>the value at the back of the queue</returns>
        public short PeekBack()
        {
            AssertNotEmpty();
            return m_values[ Previous( m_tail ) ];
        }

        #endregion

        #region Methods -> Private

        void AssertNotEmpty()
        {
            Assert.IsTrue( m_count > 0, m_errorEmpty );
        }

        void AssertNotFull()
        {
            Assert.IsTrue( m_count < m_values.Length, m_errorFull );
        }

        short LastIndex => (short) ( m_values.Length - 1 );
        
        short Next( short index )
        {
            return (short) ( index < LastIndex? index + 1 : 0 );
        }

        short Previous( short index )
        {
            return (short) ( index > 0? index - 1 : LastIndex );
        }

        #endregion
    }
}