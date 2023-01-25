// ~~----------~~ NORCAT STUDIO ~~----------~~
// -- CLASS     NORCAT VR (Extensions) - CHAIN
// -- AUTHOR    Mike Daoust
// -- CREATED   June 30th, 2020
// -- MOOD      Sicky
// --
// - This chain component is the starting point for making a physically driven chain
// - That can be picked up and interacted with by the player.
// - It still has a lot of instabilities in it. Sadly. But it's more or less OK.
// - Set it up the way you want, then push the Generate Chain button to continue.
// --

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


namespace NORCAT.Chains {
    public class ChainGenerator : MonoBehaviour {


        /// <summary>The prefab to use for the individual chain links</summary>
        [Tooltip("The prefab to use for the individual chain links")]
        public GameObject chainLinkPrefab;

        /// <summary>The number of links in the chain to generate.</summary>
        [Tooltip("The number of links in the chain to generate.")]
        public int numberOfLinks = 16;

        /// <summary>The space between each link (in Unity world space units)</summary>
        [Tooltip("The space between each link (in Unity world space units)")]
        public float spaceBetweenLinks = 0.1f;

        /// <summary>The amount of drag to be applied to each chain link.</summary>
        [Tooltip("The amount of drag to be applied to each chain link.")]
        [Range(0f, 5f)] public float chainDrag = 1.0f;

        /// <summary>How freely each link can rotate. 0 for a minimal amount, 1 for a nearly free rotation.</summary>
        [Tooltip("How freely each link can rotate. 0 for a minimal amount, 1 for a nearly free rotation.")]
        [Range(0f, 1f)] public float linkRestrictiveness = 0.5f;

        /// <summary>If true, this chain will be fixed at the end as well as the start.</summary>
        [Tooltip("If true, this chain will be fixed at the end as well as the start.")]
        public bool startAttachedToEnd = false;

        /// <summary>The end point to attach to. You can change this visually in the scene.</summary>
        [Tooltip("The end point to attach to. You can change this visually in the scene.")]
        public Vector3 endAttachPoint;

        /// <summary>If true, every link begins frozen and set to kinematic.</summary>
        [Tooltip("If true, every link begins frozen and set to kinematic.")]
        public bool startPhysicsFrozen;

        /// <summary>If true, we generate sound effects for collisions on this chain.</summary>
        [Tooltip("If true, we generate sound effects for collisions on this chain.")]
        public bool useSoundEffects;

        /// <summary>The array of random chain sounds that can be played on collisions.</summary>
        [Tooltip("The array of random chain sounds that can be played on collisions.")]
        public AudioClip[] randomChainSounds = new AudioClip[5];

        /// <summary>The mixing group (if any) these sounds should be played from.</summary>
        [Tooltip("The mixing group (if any) these sounds should be played from.")]
        public AudioMixerGroup mixer;

        /// <summary>The percentage of chain links that can generate sounds. Use less for better performance.</summary>
        [Tooltip("The percentage of chain links that can generate sounds. Use less for better performance.")]
        [Range(0f, 100f)] public float audioLinksPercent;


        public AudioSource chainAudioSource;        // The audio source, dynamically generated.
        public bool lastKnownValue;                 // Used to reset the end position.
        public bool dirty = false;                  // True if we have made changes since last generation.
        public bool hasGenerated = false;           // True if we have generated at least once.

        private bool physicsDisabled;               // If true, we are frozen in relative space.
        private List<ChainLink> links = new List<ChainLink>();  // List of all links with the component added.
        private Transform originalParent;           // Our original parent.
        private Transform trackerT;                 // This exists to ensure that the chain does not contribute to the vehicle's physics stack.




        // ---- START ----
        // -- Sets up this chain and all its links. Handles attaching on start, sound effects,
        // -- and parenting issues relating to the grabbable chain.
        // ----
        private void Start() {
            // On start, we re-parent to root, and add a tracker where we used to be.
            originalParent = transform.parent;
            GameObject tracker = new GameObject("Chain Tracker");
            tracker.transform.position = transform.position;
            tracker.transform.rotation = transform.rotation;
            tracker.transform.parent = originalParent;
            transform.parent = null;
            trackerT = tracker.transform;

            // If we have sound effects, set them up now.
            if (useSoundEffects && chainAudioSource != null) {
                // Chain Clinks
                foreach (ChainLink link in GetComponentsInChildren<ChainLink>()) {
                    link.audioSource = chainAudioSource;
                    link.randomAudio = randomChainSounds;
                    links.Add(link);
                }
            }
        }


        // ---- UPDATE ----
        // -- Matches our position with the tracker's position.
        // -- Allowing the chain to move with the parent without changing it's calculated physics.
        // ----
        private void Update() {
            transform.position = trackerT.position;
            transform.rotation = trackerT.rotation;
        }


        // ---- DISABLE PHYSICS ----
        // -- Disables physics for this chain, limiting the chain to kinematic movement in local space.
        // ----
        public void DisablePhysics() {
            if (!physicsDisabled) {
                physicsDisabled = true;
                Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
                if (useSoundEffects) {
                    foreach (ChainLink link in links) {
                        link.DisableAudio();
                    }
                }
            }
        }


        // ---- ENABLE PHYSICS ----
        // -- If we were disabled, re-enables all physics.
        // ----
        public void ResumePhysics() {
            if (physicsDisabled) {
                physicsDisabled = false;
                Rigidbody[] rbs = GetComponentsInChildren<Rigidbody>();
                if (useSoundEffects) {
                    foreach (ChainLink link in links) {
                        link.EnableAudio();
                    }
                }
            }
        }
    }
}