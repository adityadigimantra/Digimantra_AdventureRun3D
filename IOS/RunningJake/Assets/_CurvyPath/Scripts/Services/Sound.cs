using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CurvyPath
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        public AudioClip[] clips;
        [HideInInspector]
        public int simultaneousPlayCount = 0;
    }
}