using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NORCAT.Chains {

    [CustomEditor(typeof(ChainGenerator))]
    public class Editor_NVR_Chain : Editor {


        private ChainGenerator chainObject;
        private GUIStyle errorStyle;
        private GUIStyle warningStyle;
        private bool useSlack;
        private Vector3[] points;




        // ---- ON INSPECTOR GUI ----
        // -- Draws the custom GUI.
        // -- This is pretty documentation light, but you shouldn't need to modify it. 
        public override void OnInspectorGUI() {
            chainObject = (ChainGenerator)target;
            Color oldColor = GUI.color;

            // Create styles
            if (errorStyle == null) {
                errorStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                errorStyle.normal.textColor = Color.red;
            }
            if (warningStyle == null) {
                warningStyle = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
                warningStyle.wordWrap = true;
                warningStyle.normal.textColor = new Color(0.85f, 0.4117f, 0f);
            }

            // Precalculate physics setting warnings
            bool hasPhysicsWarning = false;
            string physicsWarning = "";
            float timestepNeeded = float.Parse(((16.0f / chainObject.numberOfLinks) * 0.02f).ToString("F4"));
            if (chainObject.numberOfLinks > 64) {
                hasPhysicsWarning = true;
                physicsWarning = "Having more than 64 links in a chain is very difficult to simulate. Do at your own risk.";
            }else if(Time.fixedDeltaTime > timestepNeeded && chainObject.numberOfLinks > 10) {
                hasPhysicsWarning = true;
                physicsWarning = "Currently your physics timestep is too slow to simulate this number of links.\nTry a timestep of at least " + timestepNeeded +".\nYou can change your fixed timestep from Edit->Project Settings->Time";
            }


            // Show Chain Generation Options.
            EditorGUILayout.LabelField("Chain Generation", EditorStyles.boldLabel);
            SerializedProperty prefab = serializedObject.FindProperty("chainLinkPrefab");
            EditorGUILayout.PropertyField(prefab, new GUIContent("Chain Link Prefab"));
            if (hasPhysicsWarning) GUI.color = warningStyle.normal.textColor;
            SerializedProperty numLinks = serializedObject.FindProperty("numberOfLinks");
            EditorGUILayout.PropertyField(numLinks, new GUIContent("Number of Links in Chain"));
            if (hasPhysicsWarning) GUI.color = oldColor;
            SerializedProperty spaceBetweenLinks = serializedObject.FindProperty("spaceBetweenLinks");
            EditorGUILayout.PropertyField(spaceBetweenLinks, new GUIContent("Space Between Links"));
            SerializedProperty chainDrag = serializedObject.FindProperty("chainDrag");
            EditorGUILayout.PropertyField(chainDrag, new GUIContent("Chain Drag"));
            SerializedProperty linkRestrictiveness = serializedObject.FindProperty("linkRestrictiveness");
            EditorGUILayout.PropertyField(linkRestrictiveness, new GUIContent("Link Restrictiveness"));
            EditorGUILayout.Space();

            // Show Chain Anchor settings
            EditorGUILayout.LabelField("Chain Anchor", EditorStyles.boldLabel);
            SerializedProperty startAttached = serializedObject.FindProperty("startAttachedToEnd");
            EditorGUILayout.PropertyField(startAttached, new GUIContent("Attach end of Chain?"));
            if (chainObject.startAttachedToEnd && chainObject.startAttachedToEnd != chainObject.lastKnownValue) {
                // Move the end point to our desired end point.
                float chainDistance = (chainObject.numberOfLinks - 1) * chainObject.spaceBetweenLinks;
                Vector3 worldPoint = chainObject.transform.position + (chainObject.transform.forward * chainDistance);
                chainObject.endAttachPoint = chainObject.transform.InverseTransformPoint(worldPoint);

                // Clean up the exact decimal placing of the local position
                chainObject.endAttachPoint = CleanPosition(chainObject.endAttachPoint);
            }
            chainObject.lastKnownValue = chainObject.startAttachedToEnd;
            if (chainObject.startAttachedToEnd) {
                EditorGUI.indentLevel += 1;
                SerializedProperty endAttachPoint = serializedObject.FindProperty("endAttachPoint");
                EditorGUILayout.PropertyField(endAttachPoint, new GUIContent("End Attach Point"));
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.Space();

            // Show Sound settings
            EditorGUILayout.LabelField("Sound Effects", EditorStyles.boldLabel);
            SerializedProperty useSounds = serializedObject.FindProperty("useSoundEffects");
            EditorGUILayout.PropertyField(useSounds, new GUIContent("Use Sound Effects?"));
            if (chainObject.useSoundEffects) {
                EditorGUI.indentLevel += 1;
                SerializedProperty randomChainSounds = serializedObject.FindProperty("randomChainSounds");
                EditorGUILayout.PropertyField(randomChainSounds, new GUIContent("Random Chain Clinks"), true, null);
                //SerializedProperty mixer = serializedObject.FindProperty("mixer");
                //EditorGUILayout.PropertyField(mixer, new GUIContent("Mixer Group"));
                SerializedProperty audioLinksPercent = serializedObject.FindProperty("audioLinksPercent");
                EditorGUILayout.PropertyField(audioLinksPercent, new GUIContent("% of Links with Audio"));
                EditorGUI.indentLevel -= 1;
            }
            EditorGUILayout.Space();

            // Detect errors
            bool hasErrors = false;
            if (chainObject.chainLinkPrefab == null) {
                hasErrors = true;
                EditorGUILayout.LabelField("Set the chain link prefab to be a model of the individual chain links.", errorStyle);
            } else if (chainObject.numberOfLinks <= 0) {
                hasErrors = true;
                EditorGUILayout.LabelField("Ensure the number of links to generate is greater than 1.", errorStyle);
            } else if (chainObject.spaceBetweenLinks <= 0) {
                hasErrors = true;
                EditorGUILayout.LabelField("You need a positive, non-zero distance between links.", errorStyle);
            }

            // Show any physics warnings
            if (hasPhysicsWarning) {
                EditorGUILayout.Space();
                GUILayout.Label(physicsWarning, errorStyle);
                EditorGUILayout.Space();
            }

            // Show dirty prompt
            if (chainObject.dirty && chainObject.hasGenerated) {
                GUILayout.Label("To apply changes, generate a new chain with the button below.", warningStyle);
            }

            // If we have none, show the button.
            EditorGUI.BeginDisabledGroup(hasErrors);
            if (GUILayout.Button("Generate Chain")) {
                GenerateChainLinks();
            }
            EditorGUI.EndDisabledGroup();

            if (serializedObject.ApplyModifiedProperties()) {
                chainObject.dirty = true;
            }
        }


        // ---- GENERATE CHAIN LINKS ----
        // -- Creates the chain links and links them together.
        public void GenerateChainLinks() {
            // Before anything else, we turf all children.
            if (chainObject.transform.childCount > 0) {
                bool shouldErase = EditorUtility.DisplayDialog("Re-Generate Chain?",
                            "This will permanantly delete any changes you have made to the current chain.",
                            "JUST DO IT™",
                            "Cancel"
                        );
                if (!shouldErase) {
                    return;
                }
            }

            // Cleanup any previous
            while (chainObject.transform.childCount > 0) {
                DestroyImmediate(chainObject.transform.GetChild(0).gameObject);
            }

            // Add the Audio Source as needed
            if (chainObject.useSoundEffects) {
                GameObject chainSFX_GO = new GameObject("Chain SFX");
                chainSFX_GO.transform.parent = chainObject.transform;
                chainSFX_GO.transform.position = chainObject.transform.position;
                AudioSource chainSFX = chainSFX_GO.AddComponent<AudioSource>();
                chainSFX.outputAudioMixerGroup = chainObject.mixer;
                chainSFX.playOnAwake = false;
                chainSFX.loop = false;
                chainSFX.volume = 1.0f;
                chainSFX.spatialBlend = 1f;
                chainSFX.minDistance = 1f;
                chainSFX.maxDistance = 5f;
                chainSFX.spatialize = false;
                chainSFX.spatializePostEffects = false;
                chainObject.chainAudioSource = chainSFX;
            }

            // Okay so to start, we make our first link.
            GameObject first = Instantiate(chainObject.chainLinkPrefab, chainObject.transform);
            first.name = "First Link";
            first.transform.position = chainObject.transform.position;
            first.transform.localRotation = Quaternion.identity;
            first.transform.localScale = Vector3.one;

            // If we are using a fixed end, we modify our position and rotation
            if (chainObject.startAttachedToEnd) {
                Vector3 position1 = points[0];
                Vector3 position2 = points[1];
                Vector3 forward = (position2 - position1).normalized;
                first.transform.rotation = Quaternion.LookRotation(forward, first.transform.up);
            }

            // Add a Rigidbody and constrain it.
            Rigidbody rb = first.GetComponent<Rigidbody>();
            if (rb == null) {
                rb = first.AddComponent<Rigidbody>();
                rb.mass = 0.05f;
            }
            rb.isKinematic = true;
            Rigidbody previousRB = rb;

            // Calculate our limits
            float lowTwistLimit = Mathf.Lerp(-5f, -60f, chainObject.linkRestrictiveness);
            float highTwistLimit = Mathf.Lerp(5f, 60f, chainObject.linkRestrictiveness);
            float swingLimit = Mathf.Lerp(5f, 160f, chainObject.linkRestrictiveness);
            float swing2Limit = Mathf.Lerp(5f, 160f, chainObject.linkRestrictiveness);

            // Next, for each link in the chain minus the first and last, add a constrained link.
            Vector3 lastPoint = chainObject.transform.position;
            int targetIndex = 1;
            for (int i = 1; i < chainObject.numberOfLinks; i++) {
                GameObject linkGO = Instantiate(chainObject.chainLinkPrefab, chainObject.transform);
                Vector3 offset = new Vector3(0, 0, chainObject.spaceBetweenLinks * i);
                linkGO.transform.localPosition = first.transform.localPosition + offset;
                Vector3 curRot = first.transform.localRotation.eulerAngles;
                Quaternion newRotation = Quaternion.Euler(curRot.x, curRot.y, (i % 2 == 0) ? curRot.z : curRot.z + 90);
                linkGO.transform.localRotation = newRotation;
                linkGO.transform.localScale = Vector3.one;

                // If we should use slack, we adjust our position and rotation.
                if (chainObject.startAttachedToEnd && useSlack) {
                    Vector3 oldUp = linkGO.transform.up;

                    // First, look at our next position on the chain
                    Vector3 position = points[targetIndex];
                    Vector3 forward = (position - lastPoint).normalized;
                    if (Vector3.Distance(lastPoint, position) > chainObject.spaceBetweenLinks) {
                        // We can't make it. We gotta just move towards it.
                        position = lastPoint + (forward * chainObject.spaceBetweenLinks);
                    } else {
                        // Our distance is greater than or equal to the distance we need to move.
                        // Find the index of the next most ideal point.
                        targetIndex = GetClosestPointOnLine(lastPoint + (forward * chainObject.spaceBetweenLinks), points, targetIndex);

                        if (targetIndex == 0 && i == chainObject.numberOfLinks - 1) {
                            position = points[points.Length-1];
                            forward = (position - lastPoint).normalized;
                            position = lastPoint + (forward * chainObject.spaceBetweenLinks);
                        } else {
                            // If it's the same as our last point, increase it by 1
                            while (Vector3.Distance(points[targetIndex], lastPoint) < chainObject.spaceBetweenLinks * 0.4f) {
                                targetIndex++;
                            }

                            // Also correct for the wrong direction
                            Vector3 worldPoint = chainObject.transform.TransformPoint(chainObject.endAttachPoint);
                            Vector3 pureDir = (worldPoint - chainObject.transform.position).normalized;
                            float dot = Vector3.Dot(pureDir, (points[targetIndex] - lastPoint).normalized);
                            while (dot <= 0 && targetIndex + 1 < points.Length) {
                                targetIndex++;
                                dot = Vector3.Dot(pureDir, (points[targetIndex] - lastPoint).normalized);
                            }

                            // Now move in the direction of it.
                            position = points[targetIndex];
                            forward = (position - lastPoint).normalized;
                            position = lastPoint + (forward * chainObject.spaceBetweenLinks);
                        }

                    }
                    lastPoint = position;

                    linkGO.transform.rotation = Quaternion.LookRotation(forward, oldUp);
                    linkGO.transform.position = position;
                }

                // Add the RigidBody
                Rigidbody rbChild = linkGO.GetComponent<Rigidbody>();
                if (rbChild == null) {
                    rbChild = linkGO.AddComponent<Rigidbody>();
                    rbChild.mass = 0.05f;
                    rbChild.angularDrag = 5f;
                    rbChild.drag = chainObject.chainDrag;
                }
                rbChild.isKinematic = false;

                // Add the sound
                if (chainObject.useSoundEffects) {
                    float percentAudio = chainObject.audioLinksPercent * 0.01f;
                    int numToSkip = (int)Mathf.Round(1.0f / percentAudio);
                    if (i % numToSkip == 0) {
                        ChainLink cLink = linkGO.AddComponent<ChainLink>();
                        cLink.audioSource = chainObject.chainAudioSource;
                        cLink.randomAudio = chainObject.randomChainSounds;
                    }
                }

                // Add the Character Joint
                if (i < chainObject.numberOfLinks - 1) {
                    CharacterJoint cj = linkGO.GetComponent<CharacterJoint>();
                    if (cj == null) {
                        cj = linkGO.AddComponent<CharacterJoint>();
                    }
                    cj.anchor = new Vector3(cj.anchor.x, cj.anchor.y, chainObject.spaceBetweenLinks * -0.5f);
                    cj.connectedBody = previousRB;
                    SoftJointLimit l1 = new SoftJointLimit(); l1.limit = lowTwistLimit; cj.lowTwistLimit = l1;
                    SoftJointLimit l2 = new SoftJointLimit(); l2.limit = highTwistLimit; cj.highTwistLimit = l2;
                    SoftJointLimit l3 = new SoftJointLimit(); l3.limit = swingLimit; cj.swing1Limit = l3;
                    SoftJointLimit l4 = new SoftJointLimit(); l4.limit = swing2Limit; cj.swing2Limit = l4;
                } else {
                    // This is the last link! Instead of adding joint, we add a second to the last link.
                    CharacterJoint cj = previousRB.gameObject.AddComponent<CharacterJoint>();
                    cj.anchor = new Vector3(cj.anchor.x, cj.anchor.y, chainObject.spaceBetweenLinks * 0.5f);
                    cj.connectedBody = rbChild;
                    SoftJointLimit l1 = new SoftJointLimit();l1.limit = -177f; cj.lowTwistLimit = l1;
                    SoftJointLimit l2 = new SoftJointLimit();l2.limit = 177f; cj.highTwistLimit = l2;
                    SoftJointLimit l3 = new SoftJointLimit();l3.limit = 177f; cj.swing1Limit = l3;
                    SoftJointLimit l4 = new SoftJointLimit();l4.limit = 177f; cj.swing2Limit = l4;
                    rbChild.isKinematic = (chainObject.startAttachedToEnd);
                }

                // Set the previousRB for the next link
                previousRB = rbChild;

                linkGO.name = "Link " + i;
            }

            chainObject.dirty = false;
            chainObject.hasGenerated = true;

            GUIUtility.ExitGUI();
        }


        // ---- ON SCENE GUI ----
        // -- Handles drawing the end position handle, as well as drawing a set of lines representing what the chain will
        // -- look like upon generation.
        private void OnSceneGUI() {
            if(chainObject != null && chainObject.startAttachedToEnd) {
                Vector3 oldPosition = chainObject.endAttachPoint;

                // Show the handle for the end point
                EditorGUI.BeginChangeCheck();
                Vector3 worldPoint = chainObject.transform.TransformPoint(chainObject.endAttachPoint);
                worldPoint = Handles.PositionHandle(worldPoint, chainObject.gameObject.transform.rotation);
                if (EditorGUI.EndChangeCheck()) {
                    Undo.RecordObject(target, "Changed End Point");
                    chainObject.endAttachPoint = chainObject.transform.InverseTransformPoint(worldPoint);
                }

                // Now show a series of lines detailing the path of the chain.
                float chainDistance = (chainObject.numberOfLinks - 1) * chainObject.spaceBetweenLinks;
                float pointDistance = Vector3.Distance(chainObject.gameObject.transform.position, worldPoint);
                Vector3 forward = (worldPoint - chainObject.transform.position).normalized;

                if (pointDistance >= chainDistance) {
                    // Clamp.
                    worldPoint = chainObject.transform.position + (forward * chainDistance);
                    Handles.DrawLine(chainObject.transform.position, worldPoint);
                    useSlack = false;
                } else {
                    // We can't satisfy this distance without adding slack to the chain.
                    // Attempt 2, a blind while loop that keeps changing the bottom position of the middle link.
                    float currentDistance = pointDistance;
                    float gravityDip = chainObject.spaceBetweenLinks;
                    Vector3[] pointsArcStart = new Vector3[0];
                    Vector3[] pointsArcEnd = new Vector3[0];
                    int sentinal = 1000;
                    while (currentDistance < chainDistance && sentinal > 0) {
                        // Start with the bezier to the center.
                        Vector3 p1 = chainObject.transform.position;
                        Vector3 p2 = (p1 + (forward*(pointDistance*0.5f))) + (Physics.gravity.normalized * gravityDip);
                        Vector3 t1 = p1 + (Physics.gravity.normalized * (gravityDip));
                        Vector3 t2 = p2;//p2 + (forward * -1f * (gravityDip));
                        pointsArcStart = Handles.MakeBezierPoints(p1, p2, t1, t2, Mathf.FloorToInt(chainObject.numberOfLinks/2.0f));
                        currentDistance = 0;
                        for (int i=0; i< pointsArcStart.Length; i++) {
                            if (i < pointsArcStart.Length - 1) {
                                currentDistance += Vector3.Distance(pointsArcStart[i], pointsArcStart[i + 1]);
                            }
                        }

                        // Then add the bezier to the end.
                        p1 = p2;
                        p2 = worldPoint;
                        t1 = p1;//p1 + (forward * (gravityDip));
                        t2 = p2 + (Physics.gravity.normalized * (gravityDip));
                        pointsArcEnd = Handles.MakeBezierPoints(p1, p2, t1, t2, Mathf.CeilToInt(chainObject.numberOfLinks/2.0f));
                        for (int i = 0; i < pointsArcEnd.Length; i++) {
                            if (i < pointsArcEnd.Length - 1) {
                                currentDistance += Vector3.Distance(pointsArcEnd[i], pointsArcEnd[i + 1]);
                            }
                        }

                        // Increase Gravity Dip
                        gravityDip += (chainObject.spaceBetweenLinks * 0.5f);

                        // Decrease Sentinal
                        sentinal--;
                    }

                    // Now draw it.
                    points = new Vector3[pointsArcStart.Length + pointsArcEnd.Length];
                    if (sentinal > 0) {
                        for (int i = 0; i < pointsArcStart.Length; i++) {
                            if (i < pointsArcStart.Length - 1) {
                                Handles.DrawLine(pointsArcStart[i], pointsArcStart[i+1]);
                            }
                            points[i] = pointsArcStart[i];
                        }
                        for (int i = 0; i < pointsArcEnd.Length; i++) {
                            if (i < pointsArcEnd.Length - 1) {
                                Handles.DrawLine(pointsArcEnd[i], pointsArcEnd[i + 1]);
                            }
                            points[i + pointsArcStart.Length] = pointsArcEnd[i];
                        }
                    } else {
                        Debug.LogError("Impossible constraint on chain.");
                    }

                    useSlack = true;
                }

                // Set the dirty flag if anything has changed.
                if (Vector3.Distance(oldPosition, chainObject.endAttachPoint) > 0.0001f) {
                    chainObject.dirty = true;
                }

                // Clean up the exact decimal placing of the local position
                chainObject.endAttachPoint = CleanPosition(chainObject.endAttachPoint);
            } else {
                useSlack = false;

                // Still show the white line, just using our predicted distance and forward
                if (chainObject != null) {
                    float chainDistance = (chainObject.numberOfLinks - 1) * chainObject.spaceBetweenLinks;
                    Vector3 worldPoint = chainObject.transform.position + (chainObject.transform.forward * chainDistance);
                    Handles.DrawLine(chainObject.transform.position, worldPoint);
                }
            }
        }


        private Vector3 CleanPosition(Vector3 pos) {
            Vector3 cleanedPosition = new Vector3();
            cleanedPosition.x = float.Parse(pos.x.ToString("F4"));
            cleanedPosition.y = float.Parse(pos.y.ToString("F4"));
            cleanedPosition.z = float.Parse(pos.z.ToString("F4"));
            return cleanedPosition;
        }


        private int GetClosestPointOnLine(Vector3 point, Vector3[] line, int minIndex) {
            float distance = float.MaxValue;
            int index = 0;
            for (int i= minIndex+1; i<line.Length; i++) {
                float d2 = Vector3.Distance(point, line[i]);
                if(d2 < distance) {
                    distance = d2;
                    index = i;
                }
            }

            return index;
        }


        private int layerMaskToLayer(LayerMask layerMask) {
            int layerNumber = 0;
            int layer = layerMask.value;
            while (layer > 0) {
                layer = layer >> 1;
                layerNumber++;
            }
            return layerNumber - 1;
        }
    }
}