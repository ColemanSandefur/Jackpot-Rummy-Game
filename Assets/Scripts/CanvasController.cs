using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    private bool update;

    private bool _active;
    public bool Active {
        get {return _active;}
        set {
            _active = value;
            update = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        Active = gameObject.GetComponent<Canvas>().enabled;
    }

    // Update is called once per frame
    void Update()
    {
        if (update) {
            update = false;
            Debug.Log($"Setting {gameObject.name}'s active state to {_active}");
            gameObject.GetComponent<Canvas>().enabled = (_active);
        }
    }
}
