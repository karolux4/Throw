//======= Copyright (c) Valve Corporation, All rights reserved. ===============
using UnityEngine;
using System.Collections;

namespace Valve.VR.Extras
{
    [RequireComponent(typeof(SteamVR_TrackedObject))]
    public class SteamVR_TestThrow : MonoBehaviour
    {
        public GameObject prefab;
        public Rigidbody attachPoint;
        private bool allowed_to_throw;
        private int delta = 0;
        private float time = 0;
        private float velocity = 0;
        private Vector3 max_velocity;
        private Vector3 max_angular_velocity;

        public SteamVR_Action_Boolean spawn = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("InteractUI");

        SteamVR_Behaviour_Pose trackedObj;
        FixedJoint joint;

        private void Awake()
        {
            trackedObj = GetComponent<SteamVR_Behaviour_Pose>();
        }

        private void FixedUpdate()
        {
            if (joint == null)
            {
                GameObject go = GameObject.Instantiate(prefab);
                go.transform.position = attachPoint.transform.position;

                joint = go.AddComponent<FixedJoint>();
                joint.connectedBody = attachPoint;
            }
            else if (joint != null && allowed_to_throw && time>0.4f)
            {
                GameObject go = joint.gameObject;
                Rigidbody rigidbody = go.GetComponent<Rigidbody>();
                Object.DestroyImmediate(joint);
                joint = null;

                // We should probably apply the offset between trackedObj.transform.position
                // and device.transform.pos to insert into the physics sim at the correct
                // location, however, we would then want to predict ahead the visual representation
                // by the same amount we are predicting our render poses.

                Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
                if (origin != null)
                {
                    rigidbody.GetComponent<OnCollision>().Velocity = max_velocity;
                    rigidbody.GetComponent<OnCollision>().Angular_Velocity = max_angular_velocity;
                    rigidbody.velocity = max_velocity;
                    rigidbody.angularVelocity = max_angular_velocity;
                }
                else
                {
                    rigidbody.GetComponent<OnCollision>().Velocity = max_velocity;
                    rigidbody.GetComponent<OnCollision>().Angular_Velocity = max_angular_velocity;
                    rigidbody.velocity = max_velocity;
                    rigidbody.angularVelocity = max_angular_velocity;
                }

                rigidbody.maxAngularVelocity = rigidbody.angularVelocity.magnitude;
                allowed_to_throw = false;
                delta = 0;
                velocity = 0;
                max_velocity = new Vector3(0,0,0);
                max_angular_velocity = new Vector3(0, 0, 0);
                time = 0;
                throw new System.Exception("Throw");
            }
            else
            {
                if (time > 0.4f)
                {
                    Check_Throw(ref velocity, ref delta,ref max_velocity, ref max_angular_velocity);
                }
                else
                {
                    time += Time.deltaTime;
                }
            }
        }
        private void Check_Throw(ref float prev_velocity, ref int delta, ref Vector3 max_velocity,ref Vector3 max_angular_velocity)
        {
            Transform origin = trackedObj.origin ? trackedObj.origin : trackedObj.transform.parent;
            if (prev_velocity > 0.75f && prev_velocity > origin.TransformVector(trackedObj.GetVelocity()).magnitude)
            {
                if (delta > 0)
                {
                    allowed_to_throw = true;
                }
                else
                {
                    delta++;
                }
            }
            else
            {
                prev_velocity = origin.TransformVector(trackedObj.GetVelocity()).magnitude;
                max_velocity = origin.TransformVector(trackedObj.GetVelocity());
                max_angular_velocity = origin.TransformVector(trackedObj.GetAngularVelocity());
                allowed_to_throw = false;
            }
        }
    }
}