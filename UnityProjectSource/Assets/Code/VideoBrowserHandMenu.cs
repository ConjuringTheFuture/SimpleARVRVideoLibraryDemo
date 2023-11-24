using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.XR.CoreUtils.Bindings;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.Primitives;
using UnityEngine.XR.Interaction.Toolkit.Utilities.Tweenables.SmartTweenableVariables;

public class VideoBrowserHandMenu : MonoBehaviour
{
    public bool showVideoBrowser = false;
    public string openWithPose = "L";

    public Vector3 menuLocalOffsetPosition;
    public Vector3 menuLocalOffsetRotation;

    public float scaleSmoothingValue = 0.5f;

    public GameObject menuUIRoot;
    public GameObject menuLocalOffsetHandel;

    [Header("Palm anchor")]
    [SerializeField]
    [Tooltip("Anchor associated with the left palm pose for the hand.")]
    Transform leftPalmAnchor;


    [Header("Position follow config.")]
    [SerializeField]
    [Tooltip("Minimum distance in meters from target before which tween starts.")]
    float _minFollowDistance = 0.005f;
    [SerializeField]
    [Tooltip("Maximum distance in meters from target before tween targets, when time threshold is reached.")]
    float _maxFollowDistance = 0.03f;
    [SerializeField]
    [Tooltip("Time required to elapse before the max distance allowed goes from the min distance to the max.")]
    float minToMaxDelaySeconds = 1f;

    public float followSmoothing = 0.5f;

    public float minFollowDistance
    {
        get => _minFollowDistance;
        set
        {
            _minFollowDistance = value;
            handPositionSmartFollow.minDistanceAllowed = value;
        }
    }

    public float maxFollowDistance
    {
        get => _maxFollowDistance;
        set
        {
            _maxFollowDistance = value;
            handPositionSmartFollow.maxDistanceAllowed = value;
        }
    }

    readonly SmartFollowVector3TweenableVariable handPositionSmartFollow = new SmartFollowVector3TweenableVariable();
    readonly QuaternionTweenableVariable handRotationSmoothedFollow = new QuaternionTweenableVariable();
    readonly Vector3TweenableVariable menuScaleTweenable = new Vector3TweenableVariable();
    readonly BindingsGroup bindingsGroup = new BindingsGroup();

    protected void OnEnable()
    {
        handPositionSmartFollow.minDistanceAllowed = minFollowDistance;
        handPositionSmartFollow.maxDistanceAllowed = maxFollowDistance;
        handPositionSmartFollow.minToMaxDelaySeconds = minToMaxDelaySeconds;


        handPositionSmartFollow.Value = menuUIRoot.transform.position;
        bindingsGroup.AddBinding(handPositionSmartFollow.Subscribe(newPosition => menuUIRoot.transform.position = newPosition));

        handRotationSmoothedFollow.Value = menuUIRoot.transform.rotation;
        bindingsGroup.AddBinding(handRotationSmoothedFollow.Subscribe(newRot => menuUIRoot.transform.rotation = newRot));

        menuScaleTweenable.Value = menuUIRoot.transform.localScale;
        bindingsGroup.AddBinding(menuScaleTweenable.Subscribe(value => menuUIRoot.transform.localScale = value));

    }

    protected void OnDestroy()
    {
        handPositionSmartFollow.Dispose();
    }

    public void OnHandPoseChanged(PoseGesture pose)
    {

        if(showVideoBrowser)
        {
            if (pose.name != openWithPose)
            {
                showVideoBrowser = false;
                Debug.Log("Close Video Browser");
                menuScaleTweenable.target = Vector3.zero;
            }
        }
        else
        {
            if (pose.name == openWithPose)
            {
                showVideoBrowser = true;
                menuScaleTweenable.target = Vector3.one;
                menuLocalOffsetHandel.transform.localPosition = menuLocalOffsetPosition;
                menuLocalOffsetHandel.transform.localRotation = Quaternion.Euler(menuLocalOffsetRotation);

                var targetPos = leftPalmAnchor.position;
                var targetRot = leftPalmAnchor.rotation;

                handPositionSmartFollow.target = targetPos;
                handRotationSmoothedFollow.target = targetRot;
                handPositionSmartFollow.HandleTween(1);
                handRotationSmoothedFollow.HandleTween(1);
            }
        }
    }

    protected void LateUpdate()
    {
        menuLocalOffsetHandel.transform.localPosition = menuLocalOffsetPosition;
        menuLocalOffsetHandel.transform.localRotation = Quaternion.Euler(menuLocalOffsetRotation);

        var targetPos = leftPalmAnchor.position; 
        var targetRot = leftPalmAnchor.rotation;

        if (showVideoBrowser)
        {
            handPositionSmartFollow.target = targetPos;
            handRotationSmoothedFollow.target = targetRot;
        }

        handPositionSmartFollow.HandleTween(Time.deltaTime * followSmoothing);
        handRotationSmoothedFollow.HandleTween(Time.deltaTime * followSmoothing);
        menuScaleTweenable.HandleTween(Time.deltaTime * scaleSmoothingValue * (showVideoBrowser ? 1:10));
    }
}
