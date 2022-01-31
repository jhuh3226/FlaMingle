﻿using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples {
    public class HumanBodyTracker : MonoBehaviour {
        [SerializeField]
        [Tooltip ("The Skeleton prefab to be controlled.")]
        GameObject m_SkeletonPrefab;

        // added ---------------
        public GameObject cvFoot;
        public bool foundPerson = false; // bool to send to CanvasHolder

        // ---------------------

        [SerializeField]
        [Tooltip ("The ARHumanBodyManager which will produce body tracking events.")]
        ARHumanBodyManager m_HumanBodyManager;

        /// <summary>
        /// Get/Set the <c>ARHumanBodyManager</c>.
        /// </summary>
        public ARHumanBodyManager humanBodyManager {
            get { return m_HumanBodyManager; }
            set { m_HumanBodyManager = value; }
        }

        /// <summary>
        /// Get/Set the skeleton prefab.
        /// </summary>
        public GameObject skeletonPrefab {
            get { return m_SkeletonPrefab; }
            set { m_SkeletonPrefab = value; }
        }

        Dictionary<TrackableId, BoneController> m_SkeletonTracker = new Dictionary<TrackableId, BoneController> ();


        void OnEnable () {
            Debug.Assert (m_HumanBodyManager != null, "Human body manager is required.");
            m_HumanBodyManager.humanBodiesChanged += OnHumanBodiesChanged;
        }

        void OnDisable () {
            if (m_HumanBodyManager != null)
                m_HumanBodyManager.humanBodiesChanged -= OnHumanBodiesChanged;
        }

        void OnHumanBodiesChanged (ARHumanBodiesChangedEventArgs eventArgs) {
            BoneController boneController;

            foundPerson = true;

            foreach (var humanBody in eventArgs.added) {
                if (!m_SkeletonTracker.TryGetValue (humanBody.trackableId, out boneController)) {
                    Debug.Log ($"Adding a new skeleton [{humanBody.trackableId}].");

                    Debug.Log ("Also instantiate flamingo");
                    // Instantiate (flamingPrefab, humanBody.transform);

                    var newSkeletonGO = Instantiate (m_SkeletonPrefab, humanBody.transform);
                    boneController = newSkeletonGO.GetComponent<BoneController> ();
                    m_SkeletonTracker.Add (humanBody.trackableId, boneController);
                }

                boneController.InitializeSkeletonJoints ();
                boneController.ApplyBodyPose (humanBody);

                // added
                // foundPerson = true;
                // check either leftfoot or right foot is on the bottom
                var footCenterX = (boneController.leftFootX + boneController.rightFootX) / 2;
                if (boneController.rightFootY > boneController.leftFootY)
                    cvFoot.transform.position = new Vector3 (footCenterX, boneController.leftFootY, boneController.leftFootZ);
                else
                    cvFoot.transform.position = new Vector3 (footCenterX, boneController.rightFootY, boneController.rightFootZ);

                // Debug.Log (boneController.leftFootX);
            }

            foreach (var humanBody in eventArgs.updated) {
                if (m_SkeletonTracker.TryGetValue (humanBody.trackableId, out boneController)) {
                    boneController.ApplyBodyPose (humanBody);

                    // added
                    var footCenterX = (boneController.leftFootX + boneController.rightFootX) / 2;
                    if (boneController.rightFootY > boneController.leftFootY)
                        cvFoot.transform.position = new Vector3 (footCenterX, boneController.leftFootY, boneController.leftFootZ);
                    else
                        cvFoot.transform.position = new Vector3 (footCenterX, boneController.rightFootY, boneController.rightFootZ);
                }
            }

            foreach (var humanBody in eventArgs.removed) {
                Debug.Log ($"Removing a skeleton [{humanBody.trackableId}].");
                if (m_SkeletonTracker.TryGetValue (humanBody.trackableId, out boneController)) {
                    Destroy (boneController.gameObject);
                    m_SkeletonTracker.Remove (humanBody.trackableId);
                }
            }

        }
    }
}