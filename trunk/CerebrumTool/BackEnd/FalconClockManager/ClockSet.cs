﻿/******************************************************************** 
 * ClockSet.cs
 * Name: Matthew Cotter
 * Date: 11 Oct 2010 
 * Description: Representation of a named set of clock signals.
 * History: 
 * >> (17 May 2010) Matthew Cotter: Implemented support for capping the number of signals driven by any one clock signal.  While supported, this feature may not be used.
 * >> (15 Oct 2010) Matthew Cotter: Added support for creating new clocks, new clocks dependent on others in the set, and searching for existing clocks.
 * >> (11 Oct 2010) Matthew Cotter: Defines a named set of clock signals that are required by a core or have been generated by a clock generator.
 * >> (11 Oct 2010) Matthew Cotter: Source file created -- Initial version.
 ********************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FalconClockManager
{
    /// <summary>
    /// Class that defines a named set of clock signals associated with either a clock generator or a core requiring clock signals
    /// </summary>
    public class ClockSet
    {
        private Dictionary<string, ClockSignal> _Clocks;

        /// <summary>
        /// Default constructor.  Creates an empty set of clocks.
        /// </summary>
        public ClockSet()
        {
            _Clocks = new Dictionary<string, ClockSignal>();
        }
        /// <summary>
        /// Gets a saved clock signal by its name
        /// </summary>
        /// <param name="ClockName">The name of the clock to find in the set</param>
        /// <returns>The desired clock signal, if it was found.</returns>
        public ClockSignal GetClock(string ClockName)
        {
            if (_Clocks.ContainsKey(ClockName))
                return _Clocks[ClockName];
            else
                return null;
        }
        /// <summary>
        /// Searches for the existence of a saved clock signal by its name
        /// </summary>
        /// <param name="ClockName">The name of the clock to find in the set</param>
        /// <returns>True if the signal was found, false otherwise.</returns>
        public bool HasClock(string ClockName)
        {
            return (_Clocks.ContainsKey(ClockName));
        }
        /// <summary>
        /// Adds a new clock to the set with the specified parameters and name.
        /// </summary>
        /// <param name="ClockName">The name of the new clock</param>
        /// <param name="Frequency">The frequency of the new clock</param>
        /// <param name="Phase">The phase of the new clock</param>
        /// <param name="Group">The group of the new clock</param>
        /// <param name="Buffered">The buffer-state of the new clock</param>
        public void AddClock(string ClockName, long Frequency, int Phase, ClockGroup Group, bool Buffered)
        {
            if (!_Clocks.ContainsKey(ClockName))
            {
                ClockSignal NewClock = new ClockSignal();
                NewClock.Buffered = Buffered;
                NewClock.Group = Group;
                NewClock.Frequency = Frequency;
                NewClock.Phase = Phase;
                _Clocks.Add(ClockName, NewClock);
            }
        }
        
        /// <summary>
        /// Adds a new clock to the set which is a copy of the specified clock signal.
        /// </summary>
        /// <param name="ClockName">The name of the new clock</param>
        /// <param name="CopyClock">The clock signal to copy as a new clock in the set</param>
        public void AddClock(string ClockName, ClockSignal CopyClock)
        {
            if (!_Clocks.ContainsKey(ClockName))
            {
                ClockSignal NewClock = new ClockSignal();
                NewClock.Buffered = CopyClock.Buffered;
                NewClock.Group = CopyClock.Group;
                NewClock.Frequency = CopyClock.Frequency;
                NewClock.Phase = CopyClock.Phase;
                _Clocks.Add(ClockName, NewClock);
            }
        }

        /// <summary>
        /// Adds a new clock to the set whose properties depend on another clock in the set.  The Buffered state and group are copied directly, while frequency and phase are adjusted by
        /// the specified ratio and phase adjustment.
        /// </summary>
        /// <param name="ClockName">The name of the new clock</param>
        /// <param name="DependentClockName">The name of the clock to base the new clock's properties on</param>
        /// <param name="Ratio">The ratio of new clock frequency to existing clock frequency</param>
        /// <param name="Phase">The phase adjustment of new clock signal to existing clock frequency</param>
        public bool AddDependentClock(string ClockName, string DependentClockName, double Ratio, int Phase)
        {
            ClockSignal DependencyClock = GetClock(DependentClockName);
            if (DependencyClock != null)
            {
                AddClock(ClockName, (long)((long)DependencyClock.FrequencyValue * Ratio), DependencyClock.Phase + Phase, DependencyClock.Group, DependencyClock.Buffered);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Get a list of all clocks defined in the set
        /// </summary>
        /// <returns>A List of all clock signals in the set</returns>
        public Dictionary<string, ClockSignal> GetClocks()
        {
            return _Clocks;
        }

        /// <summary>
        /// Searches the set of clocks for a pre-existing clock with the same parameters
        /// </summary>
        /// <param name="Frequency">The frequency of the clock to match</param>
        /// <param name="Phase">The phase of the clock to match</param>
        /// <param name="Group">The group of the clock to match</param>
        /// <param name="Buffered">The buffer-state of the clock to match</param>
        /// <returns>True if a clock with matching parameters was found, false otherwise</returns>
        public bool HasMatchingClock(long Frequency, uint Phase, ClockGroup Group, bool Buffered)
        {
            foreach (ClockSignal clock in _Clocks.Values)
            {
                if (((long)clock.FrequencyValue == (long)Frequency) &&
                    (clock.Phase == Phase) &&
                    (clock.Group == Group) &&
                    (clock.Buffered == Buffered))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Searches the set of clocks for a pre-existing clock with the same parameters
        /// </summary>
        /// <param name="RequiredClock">A clock signal whose properties are to be matched within the set.</param>
        /// <returns>True if a clock with matching parameters was found, false otherwise</returns>
        public bool HasMatchingClock(ClockSignal RequiredClock)
        {
            foreach (ClockSignal clock in _Clocks.Values)
            {
                if (((long)clock.FrequencyValue == (long)RequiredClock.FrequencyValue) &&
                    (clock.Phase == RequiredClock.Phase) &&
                    (clock.Group == RequiredClock.Group) &&
                    (clock.Buffered == RequiredClock.Buffered))
                    return true;
            }
            return false;
        }
    }
}