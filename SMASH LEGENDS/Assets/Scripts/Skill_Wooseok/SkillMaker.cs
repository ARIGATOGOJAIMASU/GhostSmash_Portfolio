using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillMaker : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject skill;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(skill);
        }
    }
}
