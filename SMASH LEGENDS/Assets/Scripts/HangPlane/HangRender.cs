using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Junpyo
{
    public class HangRender : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField] MeshRenderer[] HangLine;

        // Update is called once per frame
        void Update()
        {
            //HangCheak Rendder
            if (Input.GetKeyDown(KeyCode.P))
            {
                for (int i = 0; i < HangLine.Length; ++i)
                {
                    HangLine[i].enabled = !HangLine[i].enabled;
                }
            }
        }
    }
}
