using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextComponent : MonoBehaviour
{
    [HideInInspector]
    public bool isTmpTextComp; //If text mesh pro component

    public bool hideParent;

    [HideInInspector]
    public Text textComponent;

    [HideInInspector]
    public TextMeshProUGUI tmpTextComponent;
    
    string _message = "";
    bool beingHandled = false;
    bool update = false;

    GameObject parentObject;
    
    public string Message {
        get {return _message;}
        set {
            _message = value;
            // Debug.Log(tmpTextComponent.transform.parent.gameObject.name);
            // Debug.Log("Updating the text");
            // tmpTextComponent.text = value;
            update  = true;
            // SetText(value);
            // SetActive(true);
        }
    }

    void Start() {
        if (hideParent){
            Debug.Log("Hide parent");
            parentObject = tmpTextComponent.transform.parent.gameObject;
            Debug.Log($"parent name: {parentObject.name}");
        }
        
        Message = GetText();
        SetActive(false);
        update = false;
    }

    void Update()
    {
        if (update) {
            SetText(_message);
            SetActive(true);
            update = false;
            StartCoroutine(Wait(10));
        }
    }

    IEnumerator Wait(int seconds) {
        beingHandled = true;
        yield return new WaitForSeconds(seconds);
        Debug.Log("Finished waiting");
        SetActive(false);

        beingHandled = false;
    }

    void SetActive(bool active) {
        Debug.Log($"Active: {active}");
        if (hideParent) {
            parentObject.transform.localScale = (active)? new Vector3(1, 1, 1) : new Vector3(0, 0, 0);
        }

        if (!isTmpTextComp && textComponent != null) {
            textComponent.gameObject.transform.localScale = (active)? new Vector3(1, 1, 1) : new Vector3(0, 0, 0);
        }

        if (isTmpTextComp && tmpTextComponent != null) {
            tmpTextComponent.gameObject.transform.localScale = (active)? new Vector3(1, 1, 1) : new Vector3(0, 0, 0);
        }
    }

    bool GetActive() {
        if (hideParent) {
            return parentObject.GetComponent<Renderer>().enabled;
        }

        if (!isTmpTextComp && textComponent != null) {
            return textComponent.gameObject.GetComponent<Renderer>().enabled;
        }

        if (isTmpTextComp && tmpTextComponent != null) {
            return tmpTextComponent.gameObject.GetComponent<Renderer>().enabled;
        }

        return false;
    }

    void SetText(string text) {
        if (!isTmpTextComp && textComponent != null) {
            textComponent.text = text;
        }

        if (isTmpTextComp && tmpTextComponent != null) {
            tmpTextComponent.SetText(text);
        }
    }

    string GetText() {
        if (!isTmpTextComp && textComponent != null) {
            return textComponent.text;
        }

        if (isTmpTextComp && tmpTextComponent != null) {
            return tmpTextComponent.text;
        }

        return "";
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(TextComponent))]
public class TextComponent_Editor: Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();

        TextComponent script = (TextComponent)target;

        script.isTmpTextComp = EditorGUILayout.Toggle("Is Text Mesh Pro Comp.", script.isTmpTextComp);
        if (script.isTmpTextComp) {
            script.tmpTextComponent = EditorGUILayout.ObjectField("Text Mesh Pro Comp", script.tmpTextComponent, typeof(TextMeshProUGUI), true) as TextMeshProUGUI;
        } else {
            script.textComponent = EditorGUILayout.ObjectField("Text Comp", script.textComponent, typeof(Text), true) as Text;
        }
    }
}
#endif