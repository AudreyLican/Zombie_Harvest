using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animate : MonoBehaviour
{
    // Start is called before the first frame update
    Animator PlayerAnimator;
    void Awake()
    {
        PlayerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerAnimator.SetFloat("walk", Input.GetAxis("Vertical"));
    }
}
