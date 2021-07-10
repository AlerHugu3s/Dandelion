using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    public static SpriteController _instance;
    private GameObject[] petals;
    private GameObject stem;
    private float[] pWeights;
    private GrivityControl grivityControl;
    private float[] states;

    // Start is called before the first frame update
    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        InitPetal();
        grivityControl = GetComponent<GrivityControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void InitPetal()
    {
        GameObject petal = gameObject.transform.Find("Petal").gameObject;
        petals = new GameObject[petal.transform.childCount];
        states = new float[petal.transform.childCount];
        for (int i = 0; i < petal.transform.childCount; i++)
        {
            petals[i] = petal.transform.GetChild(i).gameObject;
            if (petals[i] != null)
            {
                string picPath = "Sprites/" + i + "_0";
                var sr = petals[i].GetComponent<SpriteRenderer>();
                sr.sprite = GetNewSprite(picPath,sr.sprite.textureRect);
                //sr.material = 

            }
            else
            {
                Debug.Log("Im null");
            }
        }
        stem = gameObject.transform.Find("Stem").gameObject;
        if (stem != null)
        {
            string picPath = "Sprites/" + "4";
            var sr = stem.GetComponent<SpriteRenderer>();
            sr.sprite = GetNewSprite(picPath, sr.sprite.textureRect);
        }
    }

    void GetPetalWeight()
    {
        pWeights = grivityControl?.GetWeights();
    }

    public void RefreshPetalState()
    {
        GetPetalWeight();
        for (int i = 0; i < petals.Length; i++)
        {
            if (pWeights[i] > 0)
            {
                if (pWeights[i] <= 0.5f && pWeights[i] != states[i])
                {
                    // 一半
                    string picPath = "Sprites/" + i + "_1";
                    var sr = petals[i].GetComponent<SpriteRenderer>();
                    var sprite = GetNewSprite(picPath, sr.sprite.textureRect);
                    sr.sprite = sprite;
                    states[i] = pWeights[i];
                }
            }
            else 
            {
                // 空了
                if (pWeights[i] != states[i])
                {
                    string picPath = "Sprites/" + i + "_2";
                    var sr = petals[i].GetComponent<SpriteRenderer>();
                    var sprite = GetNewSprite(picPath, sr.sprite.textureRect);
                    sr.sprite = sprite;
                    states[i] = pWeights[i];
                }                
            }
        }
    }


    private Sprite GetNewSprite(string picPath,Rect rect)
    {
        Texture2D texture = Resources.Load(picPath) as Texture2D;
        return Sprite.Create(texture,rect, Vector2.zero);

    }
}
