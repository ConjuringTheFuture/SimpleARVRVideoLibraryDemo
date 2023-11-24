using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Hands;
using UnityEngine.XR.Hands.Processing;
using UnityEngine.XR.OpenXR.Input;


[System.Serializable]
public class PoseGesture
{
    public string name;
    public List<Vector3> fingerJointPositions;
}

public class SimpleHandPoseRecognizer : MonoBehaviour
{
    public List<Vector3> liveFingerJointPositions;
    public List<PoseGesture> poses;
    public PoseGesture currentPose;
    public Transform handRoot;

    public UnityEvent<PoseGesture> OnHandPoseChanged;

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            SavePose();
        }
    }
    

    public void SavePose() 
    {
        PoseGesture newPose = new PoseGesture();
        newPose.name = "new pose";
        newPose.fingerJointPositions = new List<Vector3>();
        foreach(var joint in liveFingerJointPositions) 
        {
            newPose.fingerJointPositions.Add(joint);
        }

        poses.Add(newPose);
    }

    /****************************************************************
     * This is a naive and brute froce approach to pose dection
     * it works for a demo but wastes cycles and would need to be 
     * improved if used an a production application.
     * **************************************************************/
    public void OnHandUpdated(XRHandJointsUpdatedEventArgs eventArgs) 
    {
        liveFingerJointPositions.Clear();
        NativeArray<XRHandJoint> joints = XRHandProcessingUtility.GetRawJointArray(eventArgs.hand);

        foreach (XRHandJoint joint in joints)
        {
            UnityEngine.Pose pose = new UnityEngine.Pose();
            if(joint.TryGetPose(out pose))
            {
                liveFingerJointPositions.Add(handRoot.InverseTransformPoint(pose.position));
            }
        }

        float currentMin = Mathf.Infinity;
        int newPoseindex = -1;

        for (int i = 0; i < poses.Count; i++)
        {
            PoseGesture pose = poses[i];
            float sumDifference = 0;
            for (int j = 0; j < liveFingerJointPositions.Count; j++)
            {
                sumDifference += Vector3.Distance(pose.fingerJointPositions[j], liveFingerJointPositions[j]);
            }

            if(sumDifference < currentMin)
            {
                currentMin = sumDifference;
                newPoseindex = i;
            }
        }
        if(newPoseindex >= 0)
        {
            if (currentPose.name != poses[newPoseindex].name)
                OnHandPoseChanged.Invoke(poses[newPoseindex]);
            currentPose = poses[newPoseindex];
        }
    }
}
