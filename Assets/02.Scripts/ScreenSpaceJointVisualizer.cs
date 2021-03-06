using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples {
    public class ScreenSpaceJointVisualizer : MonoBehaviour {
        // added
        public GameObject UpRightSphere, RightSphere, RightFootSphere, cvFlamingoCheck;
        public GameObject UpLeftSphere, LeftSphere, LeftFootSphere; // 11, 12, 13

        float tiltRightA, tiltRightB, tiltRightC = 0;
        float tiltLeftA, tiltLeftB, tiltLeftC = 0;

        int poseCounter = 0;
        public bool flamingoPose, posing = false; // bool passed to CanvasHolder

        // 2D joint skeleton
        enum JointIndices {
            Invalid = -1,
            Head = 0, // parent: Neck1 [1]
            Neck1 = 1, // parent: Root [16]
            RightShoulder1 = 2, // parent: Neck1 [1]
            RightForearm = 3, // parent: RightShoulder1 [2]
            RightHand = 4, // parent: RightForearm [3]
            LeftShoulder1 = 5, // parent: Neck1 [1]
            LeftForearm = 6, // parent: LeftShoulder1 [5]
            LeftHand = 7, // parent: LeftForearm [6]
            RightUpLeg = 8, // parent: Root [16]
            RightLeg = 9, // parent: RightUpLeg [8]
            RightFoot = 10, // parent: RightLeg [9]
            LeftUpLeg = 11, // parent: Root [16]
            LeftLeg = 12, // parent: LeftUpLeg [11]
            LeftFoot = 13, // parent: LeftLeg [12]
            RightEye = 14, // parent: Head [0]
            LeftEye = 15, // parent: Head [0]
            Root = 16, // parent: <none> [-1]
        }

        [SerializeField]
        [Tooltip ("The AR camera being used in the scene.")]
        Camera m_ARCamera;

        /// <summary>
        /// Get or set the <c>Camera</c>.
        /// </summary>
        public Camera arCamera {
            get { return m_ARCamera; }
            set { m_ARCamera = value; }
        }

        [SerializeField]
        [Tooltip ("The ARHumanBodyManager which will produce human body anchors.")]
        ARHumanBodyManager m_HumanBodyManager;

        /// <summary>
        /// Get or set the <c>ARHumanBodyManager</c>.
        /// </summary>
        public ARHumanBodyManager humanBodyManager {
            get { return m_HumanBodyManager; }
            set { m_HumanBodyManager = value; }
        }

        [SerializeField]
        [Tooltip ("A prefab that contains a LineRenderer component that will be used for rendering lines, representing the skeleton joints.")]
        GameObject m_LineRendererPrefab;

        /// <summary>
        /// Get or set the Line Renderer prefab.
        /// </summary>
        public GameObject lineRendererPrefab {
            get { return m_LineRendererPrefab; }
            set { m_LineRendererPrefab = value; }
        }

        Dictionary<int, GameObject> m_LineRenderers;
        static HashSet<int> s_JointSet = new HashSet<int> ();

        void Awake () {
            m_LineRenderers = new Dictionary<int, GameObject> ();
        }

        void UpdateRenderer (NativeArray<XRHumanBodyPose2DJoint> joints, int index) {
            GameObject lineRendererGO;
            if (!m_LineRenderers.TryGetValue (index, out lineRendererGO)) {
                lineRendererGO = Instantiate (m_LineRendererPrefab, transform);
                m_LineRenderers.Add (index, lineRendererGO);
            }

            var lineRenderer = lineRendererGO.GetComponent<LineRenderer> ();

            // Traverse hierarchy to determine the longest line set that needs to be drawn.
            var positions = new NativeArray<Vector2> (joints.Length, Allocator.Temp);
            try {
                var boneIndex = index;
                int jointCount = 0;
                while (boneIndex >= 0) {
                    var joint = joints[boneIndex];
                    if (joint.tracked) {
                        // positions[jointCount++] = joint.position;

                        //added
                        // Debug.Log ("upRightLeg" + joints[8].position + "rightLeg: " + joints[9].position);
                        UpRightSphere.transform.position = joints[8].position;
                        RightSphere.transform.position = joints[9].position;
                        RightFootSphere.transform.position = joints[10].position;
                        tiltRightA = (joints[8].position.y - joints[9].position.y) / (joints[8].position.x - joints[9].position.x);
                        tiltRightB = (joints[9].position.y - joints[10].position.y) / (joints[9].position.x - joints[10].position.x);
                        tiltRightC = (joints[8].position.y - joints[10].position.y) / (joints[8].position.x - joints[10].position.x);

                        UpLeftSphere.transform.position = joints[11].position;
                        LeftSphere.transform.position = joints[12].position;
                        LeftFootSphere.transform.position = joints[13].position;
                        tiltLeftA = (joints[11].position.y - joints[12].position.y) / (joints[11].position.x - joints[12].position.x);
                        tiltLeftB = (joints[12].position.y - joints[13].position.y) / (joints[12].position.x - joints[13].position.x);
                        tiltLeftC = (joints[11].position.y - joints[13].position.y) / (joints[11].position.x - joints[13].position.x);

                        // Debug.Log ("tiltLeftA: " + tiltLeftA * 100 + ", tiltLeftB: " + tiltLeftB * 100 + ", tiltLeftC: " + tiltLeftC * 100);

                        if (((tiltRightA * 100 > 15 && tiltRightA * 100 < 45) && (tiltRightB * 100 > -25 && tiltRightB * 100 < 0)) || ((tiltLeftA * 100 < -15 && tiltLeftA * 100 > -45) && (tiltLeftB * 100 < 25 && tiltLeftB * 100 > 0))) {
                            poseCounter++;
                            // Debug.Log (poseCounter);
                            if (poseCounter > 300) {
                                posing = true;
                                if (poseCounter > 2000) {
                                    Debug.Log ("flamingo posture");
                                    flamingoPose = true;
                                }
                            }
                            // cvFlamingoCheck.SetActive (true);
                        } else {
                            flamingoPose = false;
                            // cvFlamingoCheck.SetActive (false);
                            // Debug.Log ("tiltRightA: " + tiltRightA * 100 + ", tiltRightB: " + tiltRightB * 100 + ", tiltRightC: " + tiltRightC * 100);
                        }

                        if (!s_JointSet.Add (boneIndex))
                            break;
                    } else
                        break;

                    boneIndex = joint.parentIndex;
                }

                // Render the joints as lines on the camera's near clip plane.
                lineRenderer.positionCount = jointCount;
                lineRenderer.startWidth = 0.001f;
                lineRenderer.endWidth = 0.001f;
                for (int i = 0; i < jointCount; ++i) {
                    var position = positions[i];
                    var worldPosition = m_ARCamera.ViewportToWorldPoint (
                        new Vector3 (position.x, position.y, m_ARCamera.nearClipPlane));
                    lineRenderer.SetPosition (i, worldPosition);
                }
                lineRendererGO.SetActive (true);
            } finally {
                positions.Dispose ();
            }
        }

        void Update () {
            Debug.Assert (m_HumanBodyManager != null, "Human body manager cannot be null");
            var joints = m_HumanBodyManager.GetHumanBodyPose2DJoints (Allocator.Temp);
            if (!joints.IsCreated) {
                HideJointLines ();
                return;
            }

            using (joints) {
                s_JointSet.Clear ();
                for (int i = joints.Length - 1; i >= 0; --i) {
                    if (joints[i].parentIndex != -1)
                        UpdateRenderer (joints, i);
                }
            }
        }

        void HideJointLines () {
            foreach (var lineRenderer in m_LineRenderers) {
                lineRenderer.Value.SetActive (false);
            }
        }
    }
}