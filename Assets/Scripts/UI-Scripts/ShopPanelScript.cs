using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopPanelScript : MonoBehaviour {
    private bool _state;
    public bool state {
        get {
            return _state;
        }
        set {
            _state = value;
            Refresh();
        }
    }

    public void Refresh() {
    
    }
}
