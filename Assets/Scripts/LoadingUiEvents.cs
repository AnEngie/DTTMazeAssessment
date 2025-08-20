using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingUiEvents : MonoBehaviour
{
    public void OnLoaded()
    {
        gameObject.SetActive(false);
    }
}
