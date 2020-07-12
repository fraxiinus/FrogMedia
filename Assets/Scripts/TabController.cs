using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class TabController : MonoBehaviour
{
    public ToggleGroup TabGroup;
    public GameObject TabPanel;
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
        // If user didnt use a shortcut, maybe they pressed a button
        HandleTabShortcuts();
        var activeButton = TabGroup.ActiveToggles().First();

        if (activeButton != lastActive)
        {
            HideAllPages();
            
            string index = activeButton.name.Substring(activeButton.name.Length - 1);
            switch (index)
            {
                case "1":
                    ShowPage(TabPage1);
                    break;
                case "2":
                    ShowPage(TabPage2);
                    break;
                case "3":
                    ShowPage(TabPage3);
                    break;
                case "4":
                    ShowPage(TabPage4);
                    break;
                case "5":
                    ShowPage(TabPage5);
                    break;
            }

            lastActive = activeButton;
        }
    }

    bool HandleTabShortcuts()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            TabPanel.FindChildObject("TabButton1").GetComponent<Toggle>().isOn = true;
            return true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            TabPanel.FindChildObject("TabButton2").GetComponent<Toggle>().isOn = true;
            return true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha3))
        {
            TabPanel.FindChildObject("TabButton3").GetComponent<Toggle>().isOn = true;
            return true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha4))
        {
            TabPanel.FindChildObject("TabButton4").GetComponent<Toggle>().isOn = true;
            return true;
        }
        if (Input.GetKeyUp(KeyCode.Alpha5))
        {
            TabPanel.FindChildObject("TabButton5").GetComponent<Toggle>().isOn = true;
            return true;
        }
        return false;
    }

    public void GoToHomePage()
    {
        TabPanel.FindChildObject("TabButton1").GetComponent<Toggle>().isOn = true;
    }

    void HideAllPages()
    {
        TabPage1.transform.localScale = new Vector3(0, 0, 0);
        TabPage2.transform.localScale = new Vector3(0, 0, 0);
        TabPage3.transform.localScale = new Vector3(0, 0, 0);
        TabPage4.transform.localScale = new Vector3(0, 0, 0);
        TabPage5.transform.localScale = new Vector3(0, 0, 0);
    }

    static void ShowPage(GameObject obj)
    {
        obj.transform.localScale = new Vector3(1, 1, 1);
    }
}
