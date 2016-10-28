using UnityEngine;
using System.Collections;

public class Hexagon {
    public Vector3 position;
    public float targetHeight;

    private float currentHeight;
    private HexTiler parent;

    public Hexagon(HexTiler parent, Vector3 position) {
        this.parent = parent;
        this.position = position;
        targetHeight = 1;
        currentHeight = 1;
	}
	
	public void Refresh () {
        if (Mathf.Abs(currentHeight - targetHeight) > 0.01f) {
            currentHeight = Mathf.Lerp(currentHeight, targetHeight, Time.deltaTime * 10);
        }
    }

    public void Draw() {
        Graphics.DrawMesh(parent.hexMesh,
            Matrix4x4.TRS(position + parent.trans.position, Quaternion.identity, new Vector3(1, currentHeight, 1)),
            parent.hexMat, 0);

    }
}
