using System;
using UnityEngine;

namespace Data
{
    [Serializable]
    public struct Sound
    {
        public string name;
        public AudioClip clip;
        public bool loop;
        [Range(0, 1)] public float volume;
    }
}
