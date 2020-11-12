using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{

    private Animator anim;
    private Rigidbody2D rb;
    private PlayerController controller;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("speedX", Mathf.Abs(rb.velocity.x));
        anim.SetFloat("speedY", rb.velocity.y);
        anim.SetBool("ground", controller.isGround);
        //anim.SetBool("jump", controller.isJump);  //暂时通过speedY来检测是否处于滞空（包括跳跃）状态
    }
}
