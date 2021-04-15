using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Speedometer : MonoBehaviour
{

    [SerializeField] public Rigidbody target;
    [SerializeField] public float maxSpeed = 0.0f;

    [Header("UI")]
    [SerializeField] private Text speedLabel;

    private float speed = 0.0f;
    // Update is called once per frame
    void Update()
    {
        
        speed = target.velocity.magnitude * 3.6f;
        if (speedLabel != null)
            speedLabel.text = ((int)speed) + "km/h";
    }
}
