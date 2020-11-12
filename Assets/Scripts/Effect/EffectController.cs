using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour
{
    //Animation Event
    public void Finish()
    {
        gameObject.SetActive(false);
    }
}
