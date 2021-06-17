using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class Key : MonoBehaviour
{
    private Transform tr;
    public float maxYOffset;
    private bool canTake;
    public GameObject canTakeUI;

    void Start()
    {
        tr = GetComponent<Transform>();
        canTake = false;
    }

    // Update is called once per frame
    void Update()
    {
        float deltaY = Mathf.PingPong(Time.time * 0.01f, maxYOffset) - maxYOffset / 2;
        tr.position += Vector3.up * deltaY;

        if (canTake)
        {
            canTakeUI.SetActive(true);
        }
        else
        {
            canTakeUI.SetActive(false);
        }
/*
        //take the key
        if (canTake && Input.GetKeyDown(KeyCode.I))
        {
            UnityEngine.Debug.Log("take the key!");
            GameObject.FindObjectOfType<Player>().takeKey();
            Destroy(gameObject);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTake = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canTake = false;
        }
    }
}
