using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;

public class Wave : MonoBehaviour
{
    [System.Serializable]       // Makes instances of this data structure appear in Unity Inspector
    public struct WavePart
    {
        public float oneTimeDelay;
        public GameObject enemy;
        public float spawnDelay;
        public int repeats;
    }

    public static Wave instance;

    [Header("Attributes")]
    // Wave details given in Unity Inspector
    public WavePart[] Spawner0WaveEnc;     
    public WavePart[] Spawner1WaveEnc;
    public WavePart[] Spawner2WaveEnc;

    void Awake()
    {
        instance = this;
    }
}
