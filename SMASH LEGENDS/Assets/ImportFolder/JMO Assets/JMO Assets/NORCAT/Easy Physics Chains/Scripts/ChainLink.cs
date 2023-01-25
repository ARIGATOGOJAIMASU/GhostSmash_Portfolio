// ~~----------~~ NORCAT STUDIO ~~----------~~
// -- CLASS     NORCAT VR (Extensions) - CHAIN LINK
// -- AUTHOR    Mike Daoust
// -- CREATED   June 30th, 2020
// -- MOOD      Sicky
// --
// - Each Chain link is responsible for generating sounds on collisions.
// - together, they sound like a realistic chain.
// --

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainLink : MonoBehaviour {


    public AudioSource audioSource;         // The audio source to play from. Auto generated.
    public AudioClip[] randomAudio;         // Our list of random clips to pick from.
    public static float lastSoundTime;      // The time the last link sound was played.
    private bool audioEnabled = true;       // If true, we can play sound effects.
    private Rigidbody rb;                   // Our rigidbody, saved to more easily access our relative velocity.




    // ---- AWAKE ----
    // -- Gets our Rigidbody
    // ----
    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }



    // ---- ENABLE/DISABLE AUDIO ----
    // -- Enables or disables to generation of audio from this link.
    // ----
    public void EnableAudio() {
        audioEnabled = true;
    }
    public void DisableAudio() {
        audioEnabled = false;
    }


    // ---- ON COLLISION ENTER ----
    // -- Plays a random sound based on the speed, impact force, and time of the last sound played.
    // ----
    private void OnCollisionEnter(Collision collision) {
        float force = collision.relativeVelocity.magnitude;
        if (audioEnabled && (force > 1.5f || Time.time > lastSoundTime + 0.25f)) {
            float normVol = Mathf.Max(rb.angularVelocity.magnitude, rb.velocity.magnitude);
            normVol = Mathf.Lerp(0, 1, Mathf.Clamp01(normVol / 4f));
            if (force > 1.5f) normVol = 1.0f;
            if (normVol > 0.2f) {
                audioSource.transform.position = transform.position;
                AudioClip randomAudioClip = randomAudio[Random.Range(0, randomAudio.Length)];
                audioSource.PlayOneShot(randomAudioClip, normVol);
                lastSoundTime = Time.time;
            }
        }
    }
}
