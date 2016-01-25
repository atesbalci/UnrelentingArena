using UnityEngine;
using System.Collections;

public class DestroyScript : MonoBehaviour {
    public void DestroyObject() {
        Destroy(gameObject);
    }

    public void DestroyParent() {
        Destroy(GetComponentInParent<Transform>().gameObject);
    }
}
