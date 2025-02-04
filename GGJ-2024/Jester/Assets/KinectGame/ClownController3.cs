using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Windows.Kinect;
using static Assets.KinectGame.Enums;

public class ClownController3 : MonoBehaviour
{
    public GameObject LeftHand;
    public GameObject LeftArm;
    public GameObject RightHand;
    public GameObject RightArm;
    public GameObject LeftLeg;
    public GameObject LeftFoot;
    public GameObject RightLeg;
    public GameObject RightFoot;
    public GameObject Body;

    public GameObject BodySourceManager;
    private BodySourceManager _BodyManager;
    protected Transform bodyRoot;
    // A required variable if you want to rotate the model in space.
    protected GameObject offsetNode;

    // Rotations of the bones when the Kinect tracking starts.
    protected Quaternion[] initialRotations;
    protected Quaternion[] initialLocalRotations;

    protected Transform[] bones;

    private float translationScale = 3f;
    ulong currentTrackedId = 0;

    protected virtual void MapBones()
    {
        // make OffsetNode as a parent of model transform.
        //offsetNode = new GameObject(name + "Ctrl") { layer = transform.gameObject.layer, tag = transform.gameObject.tag };
        //offsetNode.transform.position = transform.position;
        //offsetNode.transform.rotation = transform.rotation;
        //offsetNode.transform.parent = transform.parent;

        //transform.parent = offsetNode.transform;
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;

        // take model transform as body root
        bodyRoot = transform;

        for (int i = 0; i != supportedTypes.Count; i++)
        {
            var supportedType = supportedTypes[i];
            switch (supportedType)
            {
                case JointType.HandLeft:
                    bones[i] = LeftHand.transform;
                    break;
                case JointType.HandRight:
                    bones[i] = RightHand.transform;
                    break;
                case JointType.AnkleLeft:
                    bones[i] = LeftLeg.transform;
                    break;
                case JointType.AnkleRight:
                    bones[i] = RightLeg.transform;
                    break;
                case JointType.ElbowLeft:
                    bones[i] = LeftArm.transform;
                    break;
            }

            //Limb newLimb = bones[(int)jt].gameObject.AddComponent<Limb>();
            //switch (jt)
            //{
            //        case JointType.HandLeft:
            //            newLimb.LimbType = LimbType.LeftHand;
            //            break;
            //        case JointType.HandRight:
            //            newLimb.LimbType = LimbType.RightHand;
            //            break;
            //        case JointType.FootLeft:
            //            newLimb.LimbType = LimbType.LeftFoot;
            //            break;
            //        case JointType.FootRight:
            //            newLimb.LimbType = LimbType.RightFoot;
            //            break;
            //    }
            //}
        }
    }
    public void Awake()
    {
        // check for double start
        if (bones != null)
            return;

        // inits the bones array
        bones = new Transform[10];

        // Initial rotations and directions of the bones.
        initialRotations = new Quaternion[bones.Length];
        initialLocalRotations = new Quaternion[bones.Length];

        // Map bones to the points the Kinect tracks
        MapBones();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BodySourceManager == null)
        {
            return;
        }

        _BodyManager = BodySourceManager.GetComponent<BodySourceManager>();
        if (_BodyManager == null)
        {
            return;
        }

        Body[] data = _BodyManager.GetData();
        if (data == null)
        {
            return;
        }

        List<ulong> trackedIds = new List<ulong>();
        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                trackedIds.Add(body.TrackingId);
            }
        }

        // First delete untracked bodies
        if (!trackedIds.Contains(currentTrackedId))
        {
            currentTrackedId = 0;
            Body.SetActive(false);
        }

        foreach (var body in data)
        {
            if (body == null)
            {
                continue;
            }

            if (body.IsTracked)
            {
                currentTrackedId = body.TrackingId;
                Body.SetActive(true);
                //MoveAvatar(1);
                MoveAvatar(body);
            }
        }

        return;
    }

    private static List<JointType> supportedTypes = new List<JointType>() { JointType.HandLeft, JointType.HandRight, JointType.AnkleLeft, JointType.AnkleRight, JointType.ElbowLeft, JointType.ElbowRight, JointType.KneeLeft, JointType.KneeRight };

    private void MoveAvatar(Body body)
    {
        foreach (var supportedType in supportedTypes)
        {
            Windows.Kinect.Joint sourceJoint = body.Joints[supportedType];
            Windows.Kinect.Joint originJoint = body.Joints[supportedType];
            Vector3 translatedRotations = new Vector3();
            switch (supportedType)
            {
                case JointType.HandLeft:
                    originJoint = body.Joints[JointType.ElbowLeft];
                    float a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    float b = a;
                    float c = CalculateLength(originJoint.Position.X - a, sourceJoint.Position.X, originJoint.Position.Y, sourceJoint.Position.Y);
                    float newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.Y > originJoint.Position.Y)
                    {
                        newZRotation *= -1;
                    }
                    LeftHand.transform.localEulerAngles = new Vector3(LeftHand.transform.localEulerAngles.x, LeftHand.transform.localEulerAngles.y, newZRotation - LeftArm.transform.localEulerAngles.z);
                    LeftHand.transform.localScale = new Vector3(a * translationScale /*/ LeftArm.transform.localScale.x*/, 1, 1);
                    //LeftHand.transform.position = GetVector3FromJoint(sourceJoint) * 3f - new Vector3(0, 2);
                    break;
                case JointType.ElbowLeft:
                    originJoint = body.Joints[JointType.ShoulderLeft];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X - a, sourceJoint.Position.X, originJoint.Position.Y, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.Y > originJoint.Position.Y)
                    {
                        newZRotation *= -1;
                    }
                    //a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Z, originJoint.Position.Z);
                    //b = a;
                    //c = CalculateLength(originJoint.Position.X - a, sourceJoint.Position.X, originJoint.Position.Z, sourceJoint.Position.Z);
                    //float newYRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    LeftArm.transform.localEulerAngles = new Vector3(LeftArm.transform.localEulerAngles.x, LeftArm.transform.localEulerAngles.y, (float)newZRotation);
                    LeftArm.transform.localScale = new Vector3(a * translationScale, 1, 1);
                    break;
                case JointType.HandRight:
                    originJoint = body.Joints[JointType.ElbowRight];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X + a, sourceJoint.Position.X, originJoint.Position.Y, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.Y < originJoint.Position.Y)
                    {
                        newZRotation *= -1;
                    }
                    RightHand.transform.localEulerAngles = new Vector3(RightHand.transform.localEulerAngles.x, RightHand.transform.localEulerAngles.y, newZRotation - RightArm.transform.localEulerAngles.z);
                    RightHand.transform.localScale = new Vector3(a * translationScale /*/ RightArm.transform.localScale.x*/, 1, 1);
                    break;
                case JointType.ElbowRight:
                    originJoint = body.Joints[JointType.ShoulderRight];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X + a, sourceJoint.Position.X, originJoint.Position.Y, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.Y < originJoint.Position.Y)
                    {
                        newZRotation *= -1;
                    }
                    //a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Z, originJoint.Position.Z);
                    //b = a;
                    //c = CalculateLength(originJoint.Position.X - a, sourceJoint.Position.X, originJoint.Position.Z, sourceJoint.Position.Z);
                    //float newYRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    RightArm.transform.localEulerAngles = new Vector3(RightArm.transform.localEulerAngles.x, RightArm.transform.localEulerAngles.y, (float)newZRotation);
                    RightArm.transform.localScale = new Vector3(a * translationScale, 1, 1);
                    break;
                case JointType.KneeLeft:
                    originJoint = body.Joints[JointType.HipLeft];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X, sourceJoint.Position.X, originJoint.Position.Y - a, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.X < originJoint.Position.X)
                    {
                        newZRotation *= -1;
                    }
                    LeftLeg.transform.localEulerAngles = new Vector3(LeftLeg.transform.localEulerAngles.x, LeftLeg.transform.localEulerAngles.y, (float)newZRotation);
                    LeftLeg.transform.localScale = new Vector3(1, a * translationScale, 1);
                    break;
                case JointType.AnkleLeft:
                    originJoint = body.Joints[JointType.KneeLeft];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X, sourceJoint.Position.X, originJoint.Position.Y - a, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.X < originJoint.Position.X)
                    {
                        newZRotation *= -1;
                    }
                    LeftFoot.transform.localEulerAngles = new Vector3(LeftFoot.transform.localEulerAngles.x, LeftFoot.transform.localEulerAngles.y, (float)newZRotation - LeftLeg.transform.localEulerAngles.z);
                    LeftFoot.transform.localScale = new Vector3(1, a * translationScale, 1);
                    break;
                case JointType.KneeRight:
                    originJoint = body.Joints[JointType.HipRight];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X, sourceJoint.Position.X, originJoint.Position.Y - a, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.X < originJoint.Position.X)
                    {
                        newZRotation *= -1;
                    }
                    RightLeg.transform.localEulerAngles = new Vector3(RightLeg.transform.localEulerAngles.x, RightLeg.transform.localEulerAngles.y, (float)newZRotation);
                    RightLeg.transform.localScale = new Vector3(1, a * translationScale, 1);
                    break;
                case JointType.AnkleRight:
                    originJoint = body.Joints[JointType.KneeRight];
                    a = CalculateLength(sourceJoint.Position.X, originJoint.Position.X, sourceJoint.Position.Y, originJoint.Position.Y);
                    b = a;
                    c = CalculateLength(originJoint.Position.X, sourceJoint.Position.X, originJoint.Position.Y - a, sourceJoint.Position.Y);
                    newZRotation = Mathf.Rad2Deg * CalculateAngle(a, b, c);
                    if (sourceJoint.Position.X < originJoint.Position.X)
                    {
                        newZRotation *= -1;
                    }
                    RightFoot.transform.localEulerAngles = new Vector3(RightFoot.transform.localEulerAngles.x, RightFoot.transform.localEulerAngles.y, (float)newZRotation - RightLeg.transform.localEulerAngles.z);
                    RightFoot.transform.localScale = new Vector3(1, a * translationScale, 1);
                    break;
            }




            //var boneToEdit = bones[(int)supportedType];
            //boneToEdit.position = (GetVector3FromJoint(sourceJoint) - GetVector3FromJoint(body.Joints[JointType.SpineBase])) * 3f - new Vector3(0, 2);
        }
    }

    static float CalculateAngle(float a, float b, float c)
    {
        var result = (a * a + b * b - c * c) / (2 * a * b);
        return Mathf.Acos(result);
    }

    static float CalculateLength(float x1, float x2, float y1, float y2)
    {
        return Mathf.Sqrt(Math.Abs(x1 - x2) * Math.Abs(x1 - x2) + Math.Abs(y1 - y2) * Math.Abs(y1 - y2));
    }

    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X, joint.Position.Y, 0);
    }
}
