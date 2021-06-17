using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour

{
    private Rigidbody2D rb;
    public float horSpeed;
    public float jumpSpeed;
    public Transform playerEnd;  //玩家牵绳的那一端
    [HideInInspector]
    public Transform nodeEnd;   //节点那一端
    private LineRenderer lr;
    public float nodeOverlapRadius;
    public LayerMask nodeLayer, keyLayer;
    public Vector3 transOffset;
    private SpriteRenderer sp;
    private bool withKey;
    public float maxRelativeVel;
    private bool isJumping, hasDoneSecondJump;
    public Vector3 keyDropOffset;
    public GameObject key, brokenKey;
    public bool transmiting;
    public Animator am;
    public float smoothFactor;


    public GameObject alink, blink;


    private bool linking;
    public void setLinking(bool f){linking = f; lr.positionCount = 0; }
    public bool isLinking(){return linking;}

    public bool isWithKey()
    {
        return withKey;
    }
    public void takeKey()
    {
        withKey = true;
    }
    public void dropKey()
    {
        withKey = false;
        //
        Vector3 newPlace = transform.position + keyDropOffset;
        Instantiate(key, newPlace, Quaternion.identity);
    }
    public void transKey()
    {
        withKey = false;
    }

    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lr = GetComponent<LineRenderer>();
        sp = GetComponent<SpriteRenderer>();
        withKey = false;
        isJumping = false;
        hasDoneSecondJump = false;

/*        Node n1 = GameObject.Find("Node (1)").GetComponent<Node>();
        Node n2 = GameObject.Find("Node").GetComponent<Node>();
        LinkTwoNodes(n1, n2);*/

    }

    // Update is called once per frame
    void Update()
    {
        if (transmiting) return;
        float hor = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(hor * horSpeed, rb.velocity.y);
        am.SetFloat("speed", Mathf.Abs(hor * horSpeed));

        //direction
        if (rb.velocity.x < -0.01f)
        {
            transform.localScale = new Vector3(-1, transform.localScale.y, transform.localScale.z);
        }
        else if (rb.velocity.x > 0.01f)
        {
            transform.localScale = new Vector3(1, transform.localScale.y, transform.localScale.z);
        }
        else
        {

        }

        if (Input.GetKeyDown(KeyCode.Space))

        {
            if (!withKey)
            {
                if (!isJumping)
                {
                    FindObjectOfType<AudioManager>().Play("jump1");
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                    isJumping = true;
                    am.SetBool("jumping", true);
                }
                else if (!hasDoneSecondJump)
                {
                    FindObjectOfType<AudioManager>().Play("jump2");
                    hasDoneSecondJump = true;
                    rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                }
           }
        }

        if (isLinking())
        {
            Vector2 p = Vector2.Lerp(lr.GetPosition(0), playerEnd.position, smoothFactor);
            lr.SetPosition(0, p);
        }


        //cancel the link between the player and the node
        if (Input.GetKeyDown(KeyCode.K))
        {
            setLinking(false);
            nodeEnd = null;
            lr.positionCount = 0;
        }


        //link
        if (Input.GetKeyDown(KeyCode.J))
        {
            Collider2D nodeCol =  Physics2D.OverlapCircle(transform.position, nodeOverlapRadius, nodeLayer);
            if (nodeCol != null)
            {
                if (!isLinking())
                {
                    LinkThePlayerWithTheNode(nodeCol);
                }
                else
                {
                    LinkTwoNodes(nodeCol.GetComponent<Node>(), nodeEnd.GetComponent<Node>());
                }
            }
        }

        //transform
        if (Input.GetKeyDown(KeyCode.L))
        {
            Collider2D nodeCol = Physics2D.OverlapCircle(transform.position, nodeOverlapRadius, nodeLayer);
            if (nodeCol != null) {
                if (nodeCol.GetComponent<Node>().blockIn)
                {
                    nodeCol.GetComponent<Node>().transBlock();
                }
                else if (!isWithKey())
                {
                    //execute trans player
                    nodeCol.GetComponent<Node>().transPlayer(this);
                    
                }
                else
                {
                    //execute trans key
                    nodeCol.GetComponent<Node>().transKey(this);

                }
            }
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (isWithKey())
            {
                dropKey();
            }
            else
            {
                Collider2D keyCol = Physics2D.OverlapCircle(transform.position, nodeOverlapRadius, keyLayer);
                if(keyCol != null)
                {
                    FindObjectOfType<AudioManager>().Play("collect");
                    takeKey();
                    Destroy(keyCol.gameObject);
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Floor") || collision.gameObject.CompareTag("Block"))
        {
            am.SetBool("jumping", false);
            isJumping = hasDoneSecondJump = false;
            if (withKey && collision.relativeVelocity.magnitude >= maxRelativeVel)
            {
                UnityEngine.Debug.Log("the key is broken! fail");
                withKey = false;

                Instantiate(brokenKey, transform.position + keyDropOffset, Quaternion.identity);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    public void LinkThePlayerWithTheNode(Collider2D nodeCol)
    {
        //link the player with the node
        setLinking(true);
        lr.positionCount = 2;
        nodeEnd = nodeCol.transform;
        lr.SetPosition(0, playerEnd.position);
        lr.SetPosition(1, nodeEnd.position);

        FindObjectOfType<AudioManager>().Play("attach");
    }

    public void LinkTwoNodes(Node node, Node _nodeEnd)
    {
        //link two nodes
        if (node == _nodeEnd)
        {
            FindObjectOfType<AudioManager>().Play("failAttach");
            return;
        }

        //check if the two have been linked already
        if (node.has(_nodeEnd))
        {
            if (!node.isLimited && _nodeEnd.isLimited)
            {
                node.delDes(_nodeEnd);
                _nodeEnd.delDes(node);
            }
            else {
                UnityEngine.Debug.Log("already linked! fail!");
                FindObjectOfType<AudioManager>().Play("failAttach");
                return;
            }
        }

        setLinking(false);

        GameObject o;
        if(_nodeEnd.isLimited) o = Instantiate(alink, Vector3.zero, Quaternion.identity);
        else o = Instantiate(blink, Vector3.zero, Quaternion.identity);
        o.GetComponent<LineRenderer>().SetPosition(0, node.transform.position);
        o.GetComponent<LineRenderer>().SetPosition(1, _nodeEnd.transform.position);

        node.addDes(_nodeEnd, o, _nodeEnd.isLimited);
        _nodeEnd.addDes(node, o, _nodeEnd.isLimited);
        FindObjectOfType<AudioManager>().Play("attach");
    }
}
