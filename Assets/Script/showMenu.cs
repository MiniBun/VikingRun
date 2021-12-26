using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class showMenu : MonoBehaviour,IPointerClickHandler
{
    public GameObject gameMenu;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameMenu.activeSelf == false)
        {
            gameMenu.SetActive(true);
            Time.timeScale = 0;
        }
        else
        {
            gameMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
