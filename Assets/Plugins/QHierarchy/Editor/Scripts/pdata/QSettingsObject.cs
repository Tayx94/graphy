using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace qtools.qhierarchy.pdata
{
    [System.Serializable]
    class QSettingsObject: ScriptableObject
    {
        [SerializeField] private List<string> settingStringNames  = new List<string>();
        [SerializeField] private List<string> settingStringValues = new List<string>();

        [SerializeField] private List<string> settingFloatNames   = new List<string>();
        [SerializeField] private List<float>  settingFloatValues  = new List<float>();

        [SerializeField] private List<string> settingIntNames     = new List<string>();
        [SerializeField] private List<int>    settingIntValues    = new List<int>();

        [SerializeField] private List<string> settingBoolNames   = new List<string>();
        [SerializeField] private List<bool>   settingBoolValues  = new List<bool>();

        public void clear()
        {
            settingStringNames.Clear();
            settingStringValues.Clear();
            settingFloatNames.Clear();
            settingFloatValues.Clear();
            settingIntNames.Clear();
            settingIntValues.Clear();
            settingBoolNames.Clear();
            settingBoolValues.Clear();
        }

        public void set(string settingName, object value)
        {
            if (value is bool)
            {
                settingBoolValues[settingBoolNames.IndexOf(settingName)] = (bool)value;
            }
            else if (value is string)
            {
                settingStringValues[settingStringNames.IndexOf(settingName)] = (string)value;
            }
            else if (value is float)
            {
                settingFloatValues[settingFloatNames.IndexOf(settingName)] = (float)value;
            }
            else if (value is int)
            {
                settingIntValues[settingIntNames.IndexOf(settingName)] = (int)value;
            }
            EditorUtility.SetDirty(this);
        }

        public object get(string settingName, object defaultValue)
        {
            if (defaultValue is bool)
            {
                int id = settingBoolNames.IndexOf(settingName);
                if (id == -1) 
                {
                    settingBoolNames.Add(settingName);
                    settingBoolValues.Add((bool)defaultValue);
                    return defaultValue;
                }
                else return settingBoolValues[id];
            }
            else if (defaultValue is string)
            {
                int id = settingStringNames.IndexOf(settingName);
                if (id == -1) 
                {
                    settingStringNames.Add(settingName);
                    settingStringValues.Add((string)defaultValue);
                    return defaultValue;
                }
                else return settingStringValues[id];
            }
            else if (defaultValue is float)
            {
                int id = settingFloatNames.IndexOf(settingName);
                if (id == -1) 
                {
                    settingFloatNames.Add(settingName);
                    settingFloatValues.Add((float)defaultValue);
                    return defaultValue;
                }
                else return settingFloatValues[id];
            }
            else if (defaultValue is int)
            {
                int id = settingIntNames.IndexOf(settingName);
                if (id == -1) 
                {
                    settingIntNames.Add(settingName);
                    settingIntValues.Add((int)defaultValue);
                    return defaultValue;
                }
                else return settingIntValues[id];
            }
            return null;
        }
        
        public object get<T>(string settingName)
        {
            if (typeof(T) == typeof(bool))
            {
                int id = settingBoolNames.IndexOf(settingName);
                if (id == -1) return null;
                else return settingBoolValues[id];
            }
            else if (typeof(T) == typeof(string))
            {
                int id = settingStringNames.IndexOf(settingName);
                if (id == -1) return null;
                else return settingStringValues[id];
            }
            else if (typeof(T) == typeof(float))
            {
                int id = settingFloatNames.IndexOf(settingName);
                if (id == -1) return null;
                else return settingFloatValues[id];
            }
            else if (typeof(T) == typeof(int))
            {
                int id = settingIntNames.IndexOf(settingName);
                if (id == -1) return null;
                else return settingIntValues[id];
            }
            return null;
        }
    }
}

