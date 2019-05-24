using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DroneBehaviour : ExpanseBehaviour {
    public Rigidbody Drone;
    private Vector3 DroneRotation;
    private Vector3 torque = Vector3.zero;
    private float translateOffset = 0;
    private float rotateOffset = 0;
    private Transform RightHand;
    private OVRInput.Controller controller;

    void Start () {
        var go = transform.Find("Container/DronePrefab").gameObject;
        Drone = go.GetComponent<Rigidbody>();
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);
        for (int i = 0; i < rootObjects.Count; i++) {
            if (rootObjects[i].name == "Cockpit") {
                RightHand = rootObjects[i].transform.Find("OVRCameraRig/TrackingSpace/RightHandAnchor");
            }
        }
        controller = OVRInput.Controller.RTouch | OVRInput.Controller.RTrackedRemote;
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        DroneRotation = Drone.transform.localEulerAngles;

        if (DroneRotation.z > 10 && DroneRotation.z <= 180) { Drone.AddRelativeTorque(0, 0, -20); }//if tilt too big(stabilizes drone on z-axis)
        if (DroneRotation.z > 180 && DroneRotation.z <= 350) { Drone.AddRelativeTorque(0, 0, 20); }//if tilt too big(stabilizes drone on z-axis)
        if (DroneRotation.z > 1 && DroneRotation.z <= 10) { Drone.AddRelativeTorque(0, 0, -3); }//if tilt not very big(stabilizes drone on z-axis)
        if (DroneRotation.z > 350 && DroneRotation.z < 359) { Drone.AddRelativeTorque(0, 0, 3); }//if tilt not very big(stabilizes drone on z-axis)

        if (DroneRotation.x > 10 && DroneRotation.x <= 180) { Drone.AddRelativeTorque(-20, 0, 0); }//if tilt too big(stabilizes drone on x-axis)
        if (DroneRotation.x > 180 && DroneRotation.x <= 350) { Drone.AddRelativeTorque(20, 0, 0); }//if tilt too big(stabilizes drone on x-axis)
        if (DroneRotation.x > 1 && DroneRotation.x <= 10) { Drone.AddRelativeTorque(-3, 0, 0); }//if tilt not very big(stabilizes drone on x-axis)
        if (DroneRotation.x > 350 && DroneRotation.x < 359) { Drone.AddRelativeTorque(3, 0, 0); }//if tilt not very big(stabilizes drone on x-axis)  

        bool rightTriggerDown = OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, controller);
        bool rightTrigger = OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger, controller);
        if (rightTriggerDown) {
            translateOffset = RightHand.position.y;
            rotateOffset = RightHand.eulerAngles.y;
        }
        if (rightTrigger) {
            Drone.AddRelativeForce(0, (RightHand.position.y - translateOffset) * 10, 0);
            var rotY = RightHand.eulerAngles.y - rotateOffset;
            torque.z = RightHand.eulerAngles.z >= 180 && RightHand.eulerAngles.z < 360 ? Mathf.Clamp(-(360 - RightHand.eulerAngles.z), -45, 0) : Mathf.Clamp(RightHand.eulerAngles.z, 0, 45);
            torque.y = rotY >= 180 && rotY < 360 ? Mathf.Clamp(-(360 - rotY), -45, 0) : Mathf.Clamp(rotY, 0, 45);
            torque.x = RightHand.eulerAngles.x >= 180 && RightHand.eulerAngles.x < 360 ? Mathf.Clamp(-(360 - RightHand.eulerAngles.x), -45, 0) : Mathf.Clamp(RightHand.eulerAngles.x, 0, 45);
            Drone.AddRelativeTorque(torque.z / 2.25f, torque.y, torque.x / 2.25f);
            Drone.AddForce(0, 9, 0);//9.80665f
        }
    }
}
