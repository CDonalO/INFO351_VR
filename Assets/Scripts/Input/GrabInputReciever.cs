using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInputReciever : InputReciever
{

    void OnEnable() {
        OVRGrabbable.OnGrab += OnInputRecieved;
        OVRGrabbable.OnRelease += OnInputRecieved;    
    }

    void OnDisable() {
        OVRGrabbable.OnGrab -= OnInputRecieved;
        OVRGrabbable.OnRelease -= OnInputRecieved;  
    }

    public override void OnInputRecieved()
    {
        GameObject objectGrabbed = OVRGrabbable.currObject;
        foreach (var handler in inputHandlers)
        {
            handler.ProcessInput(new Vector3(1,3,2), objectGrabbed, null);
        }
    }
}