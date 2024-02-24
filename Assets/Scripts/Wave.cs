using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Wave : MonoBehaviour
{
    [System.Serializable]       // Makes instances of this data structure appear in Unity Inspector
    public struct WavePart {
        public GameObject enemy;
        public float spawnDelay;
        public int repeats;
    }

    public static Wave instance;

    [Header("Attributes")]
    public WavePart[] waveEnc;     // Wave details given in Unity Inspector

    void Awake() {
        instance = this;
    }
}
