using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerGroundDrop : MonoBehaviour
{
    [SerializeField] private float Delay_Time;
    [SerializeField] private GameObject HangLine;

    // Start is called before the first frame update
    public void FallStart()
    {
        StartCoroutine(nameof(Fall));
    }

    IEnumerator Fall()
    {
        yield return new WaitForSeconds(2);

        HangLine.SetActive(true);

        GetComponent<BoxCollider>().isTrigger = true;
        gameObject.AddComponent<Rigidbody>();

        while(transform.position.y > -50.0f)
        {
            GetComponent<Rigidbody>().velocity += new Vector3(0.0f, Time.deltaTime, 0.0f);
            yield return null;
        }
        
        Destroy(this.gameObject);
    }
}
