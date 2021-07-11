using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{ 
    string tagName;
    // Start is called before the first frame update
    void Awake()
    {
        tagName = gameObject.tag;

        GameObject[] objs = GameObject.FindGameObjectsWithTag(tagName);

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }

}
