using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public static SpriteController _instance;
    private GameObject[] petals;
    private GameObject stem;
    private float[] pWeights;

    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        InitPetal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitPetal()
    {
        GameObject petal = gameObject.transform.Find("Petal").gameObject;
        for (int i = 0; i < petal.transform.childCount; i++)
        {
            //petals[i] = petal.transform.GetChild(i).gameObject;
            if (petals[i] != null)
            {
                string picPath = "Sprites/" + i + "_0";
                var sr = petals[i].GetComponent<SpriteRenderer>();
                sr.sprite = GetNewSprite(picPath);
                //sr.material = 

            }
            else
            {
                Debug.Log("Im null");
            }
        }
        //stem = gameObject.transform.Find("Stem").gameObject;
        if (stem != null)
        {
            string picPath = "Sprites/" + "4";
            stem.GetComponent<SpriteRenderer>().sprite = GetNewSprite(picPath);
        }
    }

    void GetPetalWeight()
    {
        var gc = GetComponent<GrivityControl>();
        pWeights = gc.GetWeights();
    }

    public void RefreshPetalState()
    {
        GetPetalWeight();
        for (int i = 0; i < petals.Length; i++)
        {
            if (pWeights[i] > 0)
            {
                if (pWeights[i] <= 0.5f)
                {
                    // 一半
                    string picPath = "Sprites/" + i + "_1";
                    petals[i].GetComponent<SpriteRenderer>().sprite = GetNewSprite(picPath);
                }
            }
            else
            {
                // 空了
                string picPath = "Sprites/" + i + "_2";
                petals[i].GetComponent<SpriteRenderer>().sprite = GetNewSprite(picPath);
            }
        }
    }


    private Sprite GetNewSprite(string picPath)
    {
        Texture2D texture = Resources.Load(picPath) as Texture2D;
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);

    }
}
