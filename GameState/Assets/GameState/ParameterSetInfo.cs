using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameState
{
    [System.Serializable]
    public class ParameterSetInfo
    {
        public string Name;
        public AnimatorControllerParameterType Type;
        [HideInInspector] public int Hash;

        public bool BooleanValue;
        public int IntegerValue;
        public float FloatValue;
        public bool TriggerValue;
    }
}
