using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OnLevelChosen : MonoBehaviour,IPointerClickHandler
{
    private int sceneIndex;
    public void SetSceneIndex(int sceneIndex)
    {
        this.sceneIndex = sceneIndex;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.pointerClick == this.gameObject)
        {
            if (!AudioController._instance.PlayAudioClip("BtnClick"))
            {
                AudioController._instance.RegisterAudioClip("BtnClick", "Audio/SoundFx/buttonClick");
                AudioController._instance.PlayAudioClip("BtnClick");
            }
            SceneManager.LoadScene(sceneIndex);
            MainMenuController._instance.HideAll();
            if (!AudioController._instance.ChangeBGM("InGameMusic"))
            {
                AudioController._instance.RegisterAudioClip("InGameMusic", "Audio/Music/2”Œœ∑÷–“Ù¿÷");
                AudioController._instance.ChangeBGM("InGameMusic");
            }
        }
    }
}
