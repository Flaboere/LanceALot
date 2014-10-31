using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

[RequireComponent(typeof(PlayfieldObject))]
public class PlayfieldComponent : GameScript {

    protected PlayfieldObject ourObject;
    protected PlayfieldComponent _signalTarget;

    protected Transform ourTransform;
    protected int instanceID;

    public PlayfieldComponent signalTarget {
        get {
            return _signalTarget;
        }
    }

    FieldInfo[] fieldInfo;
    PropertyInfo[] propInfo;

    protected void RegisterAttributes() {
        //Type ourtype = this.GetType();

        for (int i = 0; i < fieldInfo.Length; i++) {

            if (fieldInfo[i].FieldType == typeof(System.Single)) {
                SendMessagePrivate("RegisterAttribute", fieldInfo[i].Name, (float)fieldInfo[i].GetValue(this));
            }
        }

        for (int j = 0; j < propInfo.Length; j++) {

            if (propInfo[j].PropertyType == typeof(System.Single)) {
                SendMessagePrivate("RegisterAttribute", propInfo[j].Name, (float)propInfo[j].GetValue(this, null));
            }
        }

        SubscribePrivate("AttributeDidChange", this.AttributeDidChange);
    }

    protected void AttributeDidChange(Subscription sub) {
        string attrib = sub.data[0].Read<string>();
        float new_value = sub.data[1].Read<float>();

        //Type ourtype = this.GetType();
        for (int i = 0; i < fieldInfo.Length; i++) {

            if (fieldInfo[i].FieldType == typeof(System.Single) && fieldInfo[i].Name == attrib) {
                fieldInfo[i].SetValue(this, new_value);
            }
        }

        for (int j = 0; j < propInfo.Length; j++) {
            if (propInfo[j].PropertyType == typeof(System.Single) && propInfo[j].Name == attrib) {
                propInfo[j].SetValue(this, new_value, null);
            }
        }
    }

    new public void Start() {
        base.Start();

        ourObject = this.gameObject.GetComponent<PlayfieldObject>();
        if (ourObject != null)
            ourObject.Initialize();

        ourTransform = transform;
        instanceID = GetInstanceID();

        fieldInfo = this.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
        propInfo = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
    }

    public virtual void OnInputTriggered() {
        // default action is just to pass the signal forward

        this.SendSignal();
    }

    protected virtual void SendSignal() {
        if (_signalTarget != null)
            _signalTarget.OnInputTriggered();
    }

    public bool ConnectToComponent(PlayfieldComponent comp) {
        if (comp != null) {
            _signalTarget = comp;
        }

        return true;
    }
}
