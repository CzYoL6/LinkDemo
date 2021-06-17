using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public enum UIType
    {
        SUCCEEDUI, STOPUI,
    }
    public GameObject succeedUI;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void show(UIType type, bool f)
    {
        switch (type)
        {
            case UIType.SUCCEEDUI:
                {
                    succeedUI.SetActive(f);
                    break;
                }
            case UIType.STOPUI:
                {
                    break;
                }
                        
        }
    }
}
