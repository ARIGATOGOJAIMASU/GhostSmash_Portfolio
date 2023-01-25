using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wooseok
{
    public class CylinderCollider : MonoBehaviour
    {
        // Start is called before the first frame update

        [SerializeField]
        List<GameObject> objs;
    
        List<BoxCollider> boxcols;

        [SerializeField]
        int collider_amount;
        [SerializeField]
        float x;
        [SerializeField]
        float y;
        [SerializeField]
        float z;
    
        BoxCollider template_col;
        [SerializeField]
        GameObject template_obj;

        [SerializeField]
        Skill ParentSkill;

        void Start()
        {
            template_col = template_obj.GetComponent<BoxCollider>();
            template_col.center = new Vector3(0, 0, 0);
            template_obj.transform.localScale = new Vector3(x, y, z);
            template_obj.GetComponent<MeshRenderer>().enabled = false;
            template_obj.GetComponent<Skill_Colider>().mainskill = ParentSkill;
            //template_col.size = new Vector3(x, y, z);

            for (int i = 0; i < collider_amount; i++)
            {
                Quaternion rotation = Quaternion.Euler(new Vector3(0, (float)(i * 360 / collider_amount), 0) + this.transform.rotation.eulerAngles);
                objs.Add(Instantiate(template_obj, this.transform.position, rotation, this.transform));
            }
            template_obj.SetActive(false);

        }
    
        // Update is called once per frame
        void Update()
        {
        }
    
    }
}
