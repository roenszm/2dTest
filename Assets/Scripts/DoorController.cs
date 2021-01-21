using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    private Animator anim;
    private BoxCollider2D col;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<BoxCollider2D>();
        col.enabled = false;
    }

    //GameManager 调用
    public void OpenDoor()
    {
        anim.Play("DoorOpen");
        col.enabled = true;
    }
}
