using UnityEngine;
using System.Collections;

public class StageMainScript : MonoBehaviour {
    public float timeInterval;
    private float remainingTime;
    private StageScript[] stageScripts;
    private bool _running;
    public bool running {
        get {
            return _running;
        }
        set {
            _running = value;
            if (running) {
                remainingTime = timeInterval;
            } else {
                foreach (StageScript ss in stageScripts) {
                    ss.state = StageState.normal;
                }
            }
        }
    }

    void Start() {
        stageScripts = GetComponentsInChildren<StageScript>();
        running = false;
    }

    void Update() {
        if (running) {
            remainingTime -= Time.deltaTime;
            if (remainingTime <= 0) {
                remainingTime = timeInterval;
                foreach (StageScript ss in stageScripts) {
                    if (ss.state == StageState.normal) {
                        ss.state = StageState.unstable;
                        break;
                    }
                }
            }
        }
    }
}
