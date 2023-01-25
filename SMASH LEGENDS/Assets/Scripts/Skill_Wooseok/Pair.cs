using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.ComponentModel;
//using UnityEditor.UIElements;

namespace Wooseok
{
    [System.Serializable]
    public struct Pair<TKey, TValue> 
    {
        public Pair(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }
        public TKey Key { get; set; }
        public TValue Value { get; set; }
    
        
        public void Deconstruct(out TKey key, out TValue value)
        {
            key = this.Key;
            value = this.Value;
        }
        public override string ToString()
        {
            return "Key: " + this.Key + ", Value: " + this.Value;
        }
    }
    
    [System.Serializable]
    public struct SFPair
    {
        public SFPair(string key, float value)
        {
            this.Key = key;
            this.Value = value;
        }
        public string Key { get; set; }
        public float Value { get; set; }
    
    
        public void Deconstruct(out string key, out float value)
        {
            key = this.Key;
            value = this.Value;
        }
        public override string ToString()
        {
            return this.Key + this.Value;
        }
    }
}