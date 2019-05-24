using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class OVRInput {
    public enum Button {
        PrimaryIndexTrigger = 0x00002000
    }
    public enum Controller {
		RTouch = 0x00002000,         ///< Right Oculus Touch controller. Virtual input mapping differs from the combined L/R Touch mapping.
		RTrackedRemote = 0x00003000,
        Active = 0x00004000///< Right GearVR tracked remote on Android.
	}
    public static bool GetDown(Button virtualMask, Controller controllerMask = Controller.Active) {
        return false;
    }
    public static bool Get(Button virtualMask, Controller controllerMask = Controller.Active) {
        return false;
    }
}
