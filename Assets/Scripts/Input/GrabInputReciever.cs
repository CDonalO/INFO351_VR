using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabInputReciever : InputReciever
{   
    private bool objGrabbed = false;
    private void Update() {
        GameObject grabbed = OVRGrabbable.currObject;
        if (grabbed != null & !objGrabbed){
            objGrabbed = true;
            Debug.Log("Aaaaa");
            OnInputRecieved();
        }
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