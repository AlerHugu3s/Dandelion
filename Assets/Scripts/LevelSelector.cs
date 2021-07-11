using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public GameObject levelHolder;
    public GameObject levelIcon;
    public GameObject thisCanvas;

    public Vector2 iconSpacing;

    private Rect panelDimensions;
    private Rect iconDimensions;
    private int amountPerPage;
    private int currentLevelCount;

    private int sceneCount;
    private string[] scenes;

// Start is called before the first frame update
    void Start()
    {
        sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;
        scenes = new string[sceneCount];
        for (int i = 1; i < sceneCount; i++)
        {
            scenes[i] = System.IO.Path.GetFileNameWithoutExtension(UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex(i));
        }

        panelDimensions = levelHolder.GetComponent<RectTransform>().rect;
        iconDimensions = levelIcon.GetComponent<RectTransform>().rect;
        int maxInRow = Mathf.FloorToInt((panelDimensions.width) / (iconDimensions.width + iconSpacing.x));
        int maxInCol = Mathf.FloorToInt((panelDimensions.height) / (iconDimensions.height + iconSpacing.y));
        
        amountPerPage = maxInRow * maxInCol;
        int totalPages = Mathf.CeilToInt((float)(sceneCount - 1) / amountPerPage);
        LoadPanel(totalPages);
    }

    void LoadPanel(int numberOfPanels)
    {
        GameObject panelClone = Instantiate(levelHolder) as GameObject;
        PageSwiper swiper = transform.Find("Panels").gameObject.AddComponent<PageSwiper>();
        swiper.totalPages = numberOfPanels;

        for (int i = 1; i <= numberOfPanels; i++)
        {
            GameObject panel = Instantiate(panelClone) as GameObject;
            panel.transform.SetParent(thisCanvas.transform, false);
            panel.transform.SetParent(transform.Find("Panels"));
            panel.name = "Page-" + i;
            panel.GetComponent<RectTransform>().localPosition = new Vector2(panelDimensions.width * (i - 1), 0);
            SetUpGrid(panel);
            int numberOfIcons = i == numberOfPanels ? sceneCount - 1 - currentLevelCount: amountPerPage;
            LoadIcon(numberOfIcons, panel);
        }
        Destroy(panelClone);
    }

    void SetUpGrid(GameObject panel)
    {
        GridLayoutGroup grids = panel.AddComponent<GridLayoutGroup>();
        grids.cellSize = iconDimensions.size;
        grids.childAlignment = TextAnchor.MiddleCenter;
        grids.spacing = iconSpacing;
    }

    void LoadIcon(int numOfIcon, GameObject parentObj)
    {
        for (int i = 1; i <= numOfIcon; i++)
        {
            currentLevelCount++;
            GameObject icon = Instantiate(levelIcon) as GameObject;
            icon.transform.SetParent(thisCanvas.transform, false);
            icon.transform.SetParent(parentObj.transform);
            icon.name = "Level " + i;
            icon.GetComponentInChildren<Text>().text = "Level " + scenes[currentLevelCount];
            icon.AddComponent<OnLevelChosen>().SetSceneIndex(currentLevelCount);
        }
    }
}
