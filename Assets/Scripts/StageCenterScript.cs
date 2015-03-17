using UnityEngine;
using System.Collections;

public class StageCenterScript : MonoBehaviour {
	void Start () {
        GetComponent<Renderer>().material.color = Color.clear;
	}
}
