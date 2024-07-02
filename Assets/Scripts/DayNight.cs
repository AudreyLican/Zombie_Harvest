using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class DayNight : MonoBehaviour
{
    Light sun;
    public float speed = 10f;
    
    void Start()
    {
        sun = GetComponent<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        sun.transform.Rotate(Vector3.right * speed * Time.deltaTime);
    }
}
