using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class Node : MonoBehaviour
{
    public class Link
    {
        public GameObject theLink;
        public bool isLimited_link;
        public Link(GameObject _gameObject, bool _isLimited_link)
        {
            theLink = _gameObject;
            isLimited_link = _isLimited_link;
        }
    }
    private List<Node> des;
    private Dictionary<Node, Link> links;
    public bool isEnd, isLimited;
    public Vector3 keyTransOffset;
    public GameObject key;
    public bool blockIn = false;
    public GameObject block;

    // Start is called before the first frame update
    public void addDes(Node t, GameObject l, bool limit){des.Add(t); links[t] = new Link(l, limit) ; }
    public void delDes(Node t){des.Remove(t); Destroy(links[t].theLink); links.Remove(t); }
    public bool has(Node t){return des.Contains(t);}
    public int getCount(){return des.Count;}
    public Node theDes(){return des[0];}
    public void transPlayer(Player player)
    {
        //FindObjectOfType<PostprocessManager>().trans();
        if (getCount() == 0)
        {
            UnityEngine.Debug.Log("no any link! fail!");

        }
        else if (getCount() >= 2)
        {
            UnityEngine.Debug.Log("ambiguous! fail!");
        }
        else
        {
            /*if (player.isLinking() && player.nodeEnd.gameObject == gameObject)
            {
                UnityEngine.Debug.Log("cannot trans! fail");
            }*/
            /*else
            {*/
            player.transmiting = true;
            FindObjectOfType<PostprocessManager>().trans(() =>
            {
                player.transform.position = new Vector3(theDes().transform.position.x, theDes().transform.position.y, 0) + player.transOffset;
                FindObjectOfType<AudioManager>().Play("trans");
                if (links[theDes()].isLimited_link)
                {
                    Node theDesNode = theDes().GetComponent<Node>();
                    delDes(theDes().GetComponent<Node>());
                    theDesNode.GetComponent<Node>().delDes(this);
                }
                player.transmiting = false;
            });
            
            /*}*/
        }
    }

    public void transKey(Player player)
    {
        if (getCount() == 0)
        {
            UnityEngine.Debug.Log("no any link! fail!");

        }
        else if (getCount() >= 2)
        {
            UnityEngine.Debug.Log("ambiguous! fail!");
        }
        else
        {
            player.transmiting = true;
            FindObjectOfType<PostprocessManager>().trans(() =>
            {
                player.transKey();
                Vector3 newPlace = new Vector3(theDes().transform.position.x, theDes().transform.position.y, 0) + keyTransOffset;
                Instantiate(key, newPlace, Quaternion.identity);
                FindObjectOfType<AudioManager>().Play("trans");
                if (links[theDes()].isLimited_link)
                {
                    Node theDesNode = theDes().GetComponent<Node>();
                    delDes(theDes().GetComponent<Node>());
                    theDesNode.GetComponent<Node>().delDes(this);
                }
            });
            player.transmiting = false;
        }
    }

    public void transBlock()
    {
        if (getCount() == 0)
        {
            UnityEngine.Debug.Log("no any link! fail!");

        }
        else if (getCount() >= 2)
        {
            UnityEngine.Debug.Log("ambiguous! fail!");
        }
        else
        {
            FindObjectOfType<PostprocessManager>().trans(() =>
            {
                block.transform.position = new Vector3(theDes().transform.position.x, theDes().transform.position.y, 0) + keyTransOffset;
                FindObjectOfType<AudioManager>().Play("trans");
                if (links[theDes()].isLimited_link)
                {
                    Node theDesNode = theDes().GetComponent<Node>();
                    delDes(theDes().GetComponent<Node>());
                    theDesNode.GetComponent<Node>().delDes(this);
                }
            });
        }
    }

    void Awake()
    {
        des = new List<Node>();
        links = new Dictionary<Node, Link>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            UnityEngine.Debug.Log("block is in");
            blockIn = true;
            block = collision.gameObject;
        }


        if (!isEnd)
        {
            return;
        }
        if (collision.CompareTag("Player"))
        {
            if (collision.GetComponent<Player>().isWithKey())
            {
                UnityEngine.Debug.Log("succeed!");
                FindObjectOfType<AudioManager>().Play("succeed");
                /*FindObjectOfType<UIManager>().show(UIManager.UIType.SUCCEEDUI, true);*/
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Block"))
        {
            blockIn = false;
            block = null;
        }
    }
}
