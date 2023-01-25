using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddOnInfoManager : MonoBehaviour
{
    [SerializeField]
    private GameObject AddOnInfoUI;
    public void OnClickExitBtn()
    {
        AddOnInfoUI.SetActive(false);
    }
}
