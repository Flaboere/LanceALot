using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PlayfieldObject : GameScript {

    #region Fields

    // Object attributes, Should be able to set from editor

    public bool isBroadcastingTransform = true;

    public bool requiresVisibilityState = false;

    protected Transform ourTransform;

    [HideInInspector]
    public bool isInitialized = false;

    [HideInInspector]
    protected int instanceID;

    private List<PlayfieldComponent> components;

    private Vector3 currentPosition;
    private Vector3 previousPosition;
    private Quaternion currentRotation;
    private Quaternion previousRotation;

    private bool isStatic;
    private bool isVisible = false;

    #endregion

    #region Base

    public void UpdateBlackboardAttributes() {

        // Write private attributes to blackboard
        // BlackBoard.Write(name + instanceID, "AttributeName", true);
    }

    [SubscriptionDocumentation("", "", "", "SetPosition", "<Vector3> position")]
    [SubscriptionDocumentation("", "", "", "SetRotation", "<Vector3> position")]
    [SubscriptionDocumentation("", "", "", "SetVelocity", "<Vector3> position")]
    [SubscriptionDocumentation("", "", "", "SetAngularVelocity", "<Vector3> position")]

    void DidBecomeVisible() {
        isVisible = true;
    }

    void DidBecomeInvisible() {  // i.e. off-screen
        isVisible = false;
    }

    new void Start() {

        if (isInitialized)
            return;

        isInitialized = true;

        base.Start();

        ourTransform = transform;
        instanceID = GetInstanceID();

        // iterate over all our playfield components
        this.components = new List<PlayfieldComponent>();

        PlayfieldComponent[] comps = this.gameObject.GetComponents<PlayfieldComponent>();

        foreach (PlayfieldComponent comp in comps) {
            this.components.Add(comp);
        }

        UpdateBlackboardAttributes();

        isStatic = gameObject.isStatic;

        //if (!isStatic) {
        //    Subscribe(name, "SetPosition", SetPosition);
        //    Subscribe(name, "SetRotation", SetRotation);

        //    if(rigidbody) {
        //        Subscribe(name, "SetVelocity", SetVelocity);
        //        Subscribe(name, "SetAngularVelocity", SetAngularVelocity);
        //    }
        //}

        BroadcastTransform();

        if (Camera.main)
            isVisible = IsCurrentlyVisibile();
    }

    public bool IsCurrentlyVisibile() {
        Camera cam = Camera.main;

        Vector3 vp_pos = cam.WorldToViewportPoint(ourTransform.position);

        // check sides, just in case we want to know it...
        if ((vp_pos.x < 0.0f || vp_pos.x > 1.0f || vp_pos.y < 0.0f || vp_pos.y > 1.0f))
            return false;

        return true;

    }

    void RecalculateVisibility() {
        bool vis = IsCurrentlyVisibile();

        if (vis && !isVisible)
            DidBecomeVisible();

        if (!vis && isVisible)
            DidBecomeInvisible();
    }

    void Update() {

        if (isStatic)
            return;

        if (isBroadcastingTransform)
            BroadcastTransform();

        // TODO: don't do this each frame
        if (!Camera.main)
            return;

        if (requiresVisibilityState)
            RecalculateVisibility();
    }

    #endregion

    #region Private

    void BroadcastTransform() {

        currentPosition = ourTransform.position;

        if (currentPosition != previousPosition) {
            BlackBoard.Write(name, "Position", currentPosition);
            previousPosition = currentPosition;
        }
    }

    #endregion

    #region Public

    public void Initialize() {

        Start();
    }

    #endregion

    #region Subscription

    void SetPosition(Subscription subscription) {

        ourTransform.position = subscription.data[0].Read<Vector3>();
    }

    void SetRotation(Subscription subscription) {

        ourTransform.eulerAngles = subscription.data[0].Read<Vector3>();
    }

    void SetVelocity(Subscription subscription) {

        if (!rigidbody)
            return;

        rigidbody.velocity = subscription.data[0].Read<Vector3>();
    }

    void SetAngularVelocity(Subscription subscription) {

        if (!rigidbody)
            return;

        rigidbody.angularVelocity = subscription.data[0].Read<Vector3>();
    }

    #endregion
}