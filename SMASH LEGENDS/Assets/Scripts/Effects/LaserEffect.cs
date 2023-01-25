using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    [ExecuteAlways]
    public class LaserEffect : MonoBehaviour
    {

        public GameObject Flashback;

        public GameObject HitEffect;
        List<GameObject> hitEffects = new List<GameObject>();
        public float HitOffset = 0;
        public bool useLaserRotation = false;

        [SerializeField]
        GameObject StartPos;

        [SerializeField]
        GameObject EndPos;

        public float MaxLength;
        [SerializeField]
        private LineRenderer Laser;

        public float MainTextureLength = 1f;
        public float NoiseTextureLength = 1f;
        [SerializeField]
        private Vector4 Length = new Vector4(1, 1, 1, 1);

        [SerializeField]
        public float LaserSize = 0.3f;

        private bool LaserSaver = false;
        private bool UpdateSaver = false;

        private ParticleSystem[] Effects;

        


        private void OnValidate()
        {
            if(!Application.isPlaying)
            {
                Laser.alignment = LineAlignment.View;
                Laser = GetComponent<LineRenderer>();
                Laser.startWidth = LaserSize;
                Laser.endWidth = LaserSize;
            }
        }

        void Start()
        {
            //Get LineRender and ParticleSystem components from current prefab;
            Laser = GetComponent<LineRenderer>();
            Effects = GetComponentsInChildren<ParticleSystem>();
            hitEffects.Clear();
        }

        private void OnEnable()
        {
            if (Application.isPlaying)
            {

                if (StartPos != null)
                {
                    Laser.SetPosition(0, StartPos.transform.position);
                }
                else
                {
                    Laser.SetPosition(0, transform.position);
                }
                if (EndPos != null)
                {
                    Laser.SetPosition(1, EndPos.transform.position);
                }
                else
                {
                    Laser.SetPosition(1, Laser.GetPosition(0) + transform.forward * MaxLength);
                }
            }
        }

        void Update()
        {
            //Setting Laser StartPos && EndPos

            if (Application.isPlaying)
            {
                
                if (StartPos != null)
                {
                    Laser.SetPosition(0, StartPos.transform.position);
                }
                else
                {
                    Laser.SetPosition(0, transform.position);
                }
                if (EndPos != null)
                {
                    Laser.SetPosition(1, EndPos.transform.position);
                }
                else
                {
                    Laser.SetPosition(1, Laser.GetPosition(0) + transform.forward * MaxLength);
                }
                Flashback.transform.position = Laser.GetPosition(0);
                Flashback.transform.LookAt(Laser.GetPosition(1));


                //Setting Laser HitEffects On Playing
                if (Laser != null && UpdateSaver == false && Application.isPlaying)
                {
                    Laser.material.SetTextureScale("_MainTex", new Vector2(Length[0], Length[1]));
                    Laser.material.SetTextureScale("_Noise", new Vector2(Length[2], Length[3]));
                    RaycastHit[] hits; //DELETE THIS IF YOU WANT USE LASERS IN 2D
                                       //ADD THIS IF YOU WANNT TO USE LASERS IN 2D: RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward, MaxLength);       
                    hits = Physics.RaycastAll(Flashback.transform.position, Flashback.transform.TransformDirection(Flashback.transform.forward), Vector3.Distance(Laser.GetPosition(0), Laser.GetPosition(1)));
                    //Debug.Log("hits.Length: " + hits.Length);
                    //Debug.Log("hitEffects.Count: " + hitEffects.Count);
                    if (hits.Length > 0)//CHANGE THIS IF YOU WANT TO USE LASERRS IN 2D: if (hit.collider != null)
                    {
                        //End laser position if collides with object
                        //Laser.SetPosition(1, hit.point);
                        for (int i = 0; i < hits.Length; i++)
                        {
                            RaycastHit hit = hits[i];
                            Debug.Log("Thishit's Position: " + hit.transform.position);
                            if (i >= hitEffects.Count)
                            {
                                hitEffects.Add(Instantiate(HitEffect, hit.point + hit.normal * HitOffset, transform.rotation, transform));
                            }
                            else
                            {
                                hitEffects[i].transform.position = hit.point + hit.normal * HitOffset;
                                if (useLaserRotation)
                                {
                                    hitEffects[i].transform.rotation = transform.rotation;
                                }
                                else
                                {
                                    HitEffect.transform.LookAt(hit.point + hit.normal);
                                }
                            }
                            //Debug.Log("hits.Length: " + hits.Length);
                            //Debug.Log("hitEffects.Count: " + hitEffects.Count);
                            hitEffects[i].SetActive(true);

                            //HitEffect.transform.position = hit.point + hit.normal * HitOffset;
                            //if (useLaserRotation)
                            //    HitEffect.transform.rotation = transform.rotation;
                            //else
                            //    HitEffect.transform.LookAt(hit.point + hit.normal);
                            Length[0] = MainTextureLength * (Vector3.Distance(transform.position, hit.point));
                            Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, hit.point));

                        }
                        if (hits.Length < hitEffects.Count)
                        {
                            for (int i = hits.Length; i < hitEffects.Count; i++)
                            {
                                hitEffects[i].SetActive(false);
                            }
                        }
                        HitEffect.transform.position = Laser.GetPosition(1);
                        HitEffect.SetActive(true);
                        foreach (var AllPs in Effects)
                        {
                            if (!AllPs.isPlaying) AllPs.Play();
                        }
                        //Texture tiling

                        //Texture speed balancer {DISABLED AFTER UPDATE}
                        //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, hit.point));
                        //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, hit.point));
                        //Destroy(hit.transform.gameObject); // destroy the object hit
                        //hit.collider.SendMessage("SomeMethod"); // example
                        //if (hit.collider.tag == "Enemy")
                        //{
                        //    hit.collider.GetComponent<HittedObject>().TakeDamage(damageOverTime * Time.deltaTime);
                        //}
                    }
                    else
                    {
                        Debug.Log("아무도 안맞고 isPlaying true일 때");
                        foreach (var hitEffect in hitEffects)
                        {
                            hitEffect.SetActive(false);
                        }
                        //End laser position if doesn't collide with object
                        var EndPos = Laser.GetPosition(1);
                        Laser.SetPosition(1, EndPos);
                        HitEffect.transform.position = EndPos;
                        HitEffect.SetActive(true);
                        //foreach (var AllPs in Hit)
                        //{
                        //    if (AllPs.isPlaying) AllPs.Play();
                        //}
                        //Texture tiling
                        Length[0] = MainTextureLength * (Vector3.Distance(transform.position, EndPos));
                        Length[2] = NoiseTextureLength * (Vector3.Distance(transform.position, EndPos));
                        //LaserSpeed[0] = (LaserStartSpeed[0] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
                        //LaserSpeed[2] = (LaserStartSpeed[2] * 4) / (Vector3.Distance(transform.position, EndPos)); {DISABLED AFTER UPDATE}
                    }
                    //Insurance against the appearance of a laser in the center of coordinates!
                    if (Laser.enabled == false && LaserSaver == false)
                    {
                        LaserSaver = true;
                        Laser.enabled = true;
                    }
                }

            }
        }

        public void DisablePrepare()
        {
            if (Laser != null)
            {
                Laser.enabled = false;
            }
            UpdateSaver = true;
            //Effects can = null in multiply shooting
            if (Effects != null)
            {
                foreach (var AllPs in Effects)
                {
                    if (AllPs.isPlaying) AllPs.Stop();
                }
            }
        }
    }
}