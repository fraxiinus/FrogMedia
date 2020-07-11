using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class TabController : MonoBehaviour
{
    public ToggleGroup TabGroup;
    private Toggle lastActive;

    public GameObject TabPage1;
    public GameObject TabPage2;
    public GameObject TabPage3;
    public GameObject TabPage4;
    public GameObject TabPage5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var activeButton = TabGroup.ActiveToggles().First();

        if (activeButton != lastActive)
        {
            HideAllPages();
            
            string index = activeButton.name.Substring(activeButton.name.Length - 1);
            switch (index)
            {
                case "1":
                    TabPage1.SetActive(true);
                    break;
                case "2":
                    TabPage2.SetActive(true);
                    break;
                case "3":
                    TabPage3.SetActive(true);
                    break;
                case "4":
                    TabPage4.SetActive(true);
                    break;
                case "5":
                    TabPage5.SetActive(true);
                    break;
            }

            lastActive = activeButton;
        }
    }

    void HideAllPages()
    {
        TabPage1.SetActive(false);
        TabPage2.SetActive(false);
        TabPage3.SetActive(false);
        TabPage4.SetActive(false);
        TabPage5.SetActive(false);
    }
}
