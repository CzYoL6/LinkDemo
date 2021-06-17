using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerTr;
    public float smoothFactor;
    // Start is called before the first frame update
    void Start()
    {
        playerTr = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 newPlace = new Vector3(playerTr.position.x, playerTr.position.y, -10);
        Vector3 vel = new Vector3(0,0,0);
        transform.position = Vector3.SmoothDamp(transform.position, newPlace, ref vel, smoothFactor);
    }
}
