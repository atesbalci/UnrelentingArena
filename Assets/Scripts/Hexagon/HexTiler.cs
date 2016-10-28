using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HexTiler : MonoBehaviour {
    public GameObject hexPrefab;
    public float hexSideLength;
    public int width;
    public int height;
    public bool refresh;
    public Material hexMat;
    public Mesh hexMesh;

    public Transform trans { get; set; }

    private List<RadialRiseStruct> radialRises;
    private Hexagon[] hexagons;

    private struct RadialRiseStruct {
        public Vector3 loc;
        public float radius;
        public float closeHeight;
        public float farHeight;

        public RadialRiseStruct(Vector3 loc, float radius, float closeHeight, float farHeight) {
            this.loc = loc;
            this.radius = radius;
            this.closeHeight = closeHeight;
            this.farHeight = farHeight;
        }
    }

	void Start () {
        radialRises = new List<RadialRiseStruct>();
        RefreshHexagons();
        trans = transform;
	}

    public void RefreshHexagons() {
        List<Hexagon> hexagons = new List<Hexagon>();
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; i++)
            Destroy(transform.GetChild(i).gameObject);
        List<Hexagon> midLine = new List<Hexagon>();
        float curx = 0;
        for (int i = 0; i < width; i++) {
            if(i % 2 == 1)
                curx += (3 * hexSideLength);
            Hexagon newHex = new Hexagon(this, new Vector3(i % 2 == 0 ? curx : -curx, 0, 0));
            midLine.Add(newHex);
            hexagons.Add(newHex);
        }
        float xshift = Mathf.Cos(Mathf.PI / 3) * hexSideLength + hexSideLength;
        float zshift = Mathf.Sin(Mathf.PI / 3) * hexSideLength;
        for(int i = 0; i < height; i++) {
            int heightMult = (i / 2) + 1;
            foreach(Hexagon obj in midLine) {
                Hexagon newHex = new Hexagon(this, obj.position + new Vector3(i % 4 < 2 ? xshift : 0, 0, (i % 2 == 0 ? 1 : -1) * heightMult * zshift));
                hexagons.Add(newHex);
            }
        }
        this.hexagons = hexagons.ToArray();
    }
	
	void Update () {
	    if(refresh) {
            refresh = false;
            RefreshHexagons();
        }
        float dist;
        int radialCount = radialRises.Count;
        for (int i = 0; i < hexagons.Length; i++) {
            hexagons[i].Refresh();
            hexagons[i].Draw();
            hexagons[i].targetHeight = 1;
            for (int n = 0; n < radialCount; n++) {
                dist = Vector3.Distance(hexagons[i].position, radialRises[n].loc);
                hexagons[i].targetHeight = Mathf.Max(hexagons[i].targetHeight,
                    Mathf.Lerp(radialRises[n].closeHeight, radialRises[n].farHeight, Mathf.Min(dist / radialRises[n].radius, 1)));
            }
        }
        radialRises.Clear();
        //transform.GetChild(Random.Range(0, transform.childCount)).GetComponent<Hexagon>().targetHeight = Random.Range(1, 10);
    }

    public void RadialRise(Vector3 loc, float radius, float closeHeight, float farHeight) {
        radialRises.Add(new RadialRiseStruct(loc, radius, closeHeight, farHeight));
    }
}
