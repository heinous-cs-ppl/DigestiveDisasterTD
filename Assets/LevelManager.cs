using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {
    // Use this reference to access stuff from this class
    public static LevelManager instance;   

    public GameObject[] path;

    // Like Start() but is called first
    private void Awake() {
        main = this;
    }

}
