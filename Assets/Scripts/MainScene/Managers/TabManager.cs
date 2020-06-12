using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    #region variables
    public RectTransform tabs;
    public RectTransform selectLine;
    public RectTransform settingPosT;
    public Button info;
    public Button game;
    public Button setting;

    private bool isSetting = false;

    private Vector3 screenSize;
    private float dragTime = 0;
    private float moveTime = 0;
    private int nowTab_ = 1;
    private bool isBigger0_2 = false;

    private Vector2 startMousePos = new Vector2();
    private Vector2 startTabPos = new Vector2();
    private float to = 0;
    private float from = 0;

    private float settingPos = 0;
    #endregion
    #region getter/setter
    private int nowTab
    {
        get
        {
            return nowTab_;
        }

        set
        {
            nowTab_ = value;
            to = screenSize.x * (nowTab - 0.5f);
            from = tabs.position.x;
            moveTime = 0;
        }
    }
    #endregion

    #region life cycle
    private void Awake()
    {
        screenSize = new Vector2(Screen.width, Screen.height);
        to = tabs.position.x;
        from = tabs.position.x;
        settingPos = settingPosT.position.x;
    }

    private void Update()
    {
        Slide();
        MoveTab();
        ChangeButtonColor();
        MoveLine();
    }
    #endregion
    #region change tabs
    public void ChangeTab(int i)
    {
        nowTab = i;
    }

    public void NextTab()
    {
        nowTab++;
        if (nowTab > 2) nowTab = 2;
    }

    public void PreTab()
    {
        nowTab--;
        if (nowTab < 0) nowTab = 0;
    }
    #endregion
    #region anims
    private void MoveLine()
    {
        selectLine.position = new Vector3(
            -(settingPos - screenSize.x * 0.5f) / screenSize.x * (tabs.position.x - screenSize.x * 0.5f) + screenSize.x * 0.5f,
            selectLine.position.y,
            0);
    }

    private void ChangeButtonColor()
    {
        Color color = new Color(0.6784314f, 0.1843137f, 0.1882353f, 0);
        Color black = new Color(0, 0, 0, 0);

        ColorBlock colorBlock_info = info.colors;
        ColorBlock colorBlock_game = game.colors;
        ColorBlock colorBlock_setting = setting.colors;

        if (nowTab == 2)
        {
            Color color_info = colorBlock_info.normalColor + color * Time.deltaTime * 3;
            if (color_info.a >= 0.6784314f) color_info = color;
            colorBlock_info.normalColor = colorBlock_info.pressedColor = colorBlock_info.highlightedColor = color_info + new Color(0, 0, 0, 1);


            Color color_game = colorBlock_game.normalColor - color * Time.deltaTime * 3;
            if (color_game.a <= 0) color_game = black;
            colorBlock_game.normalColor = colorBlock_game.pressedColor = colorBlock_game.highlightedColor = color_game + new Color(0, 0, 0, 1);

            Color color_setting = colorBlock_setting.normalColor - color * Time.deltaTime * 3;
            if (color_setting.a <= 0) color_setting = black;
            colorBlock_setting.normalColor = colorBlock_setting.pressedColor = colorBlock_setting.highlightedColor = color_setting + new Color(0, 0, 0, 1);
        }
        else if (nowTab == 1)
        {
            Color color_info = colorBlock_info.normalColor - color * Time.deltaTime * 3;
            if (color_info.a <= 0) color_info = black;
            colorBlock_info.normalColor = colorBlock_info.pressedColor = colorBlock_info.highlightedColor = color_info + new Color(0, 0, 0, 1);

            Color color_game = colorBlock_game.normalColor + color * Time.deltaTime * 3;
            if (color_game.a >= 0.6784314f) color_game = color;
            colorBlock_game.normalColor = colorBlock_game.pressedColor = colorBlock_game.highlightedColor = color_game + new Color(0, 0, 0, 1);

            Color color_setting = colorBlock_setting.normalColor - color * Time.deltaTime * 3;
            if (color_setting.a <= 0) color_setting = black;
            colorBlock_setting.normalColor = colorBlock_setting.pressedColor = colorBlock_setting.highlightedColor = color_setting + new Color(0, 0, 0, 1);
        }
        else if (nowTab == 0)
        {
            Color color_info = colorBlock_info.normalColor - color * Time.deltaTime * 3;
            if (color_info.a <= 0) color_info = black;
            colorBlock_info.normalColor = colorBlock_info.pressedColor = colorBlock_info.highlightedColor = color_info + new Color(0, 0, 0, 1);

            Color color_game = colorBlock_game.normalColor - color * Time.deltaTime * 3;
            if (color_game.a <= 0) color_game = black;
            colorBlock_game.normalColor = colorBlock_game.pressedColor = colorBlock_game.highlightedColor = color_game + new Color(0, 0, 0, 1);

            Color color_setting = colorBlock_setting.normalColor + color * Time.deltaTime * 3;
            if (color_setting.a >= 0.6784314f) color_setting = color;
            colorBlock_setting.normalColor = colorBlock_setting.pressedColor = colorBlock_setting.highlightedColor = color_setting + new Color(0, 0, 0, 1);
        }

        game.colors = colorBlock_game;
        info.colors = colorBlock_info;
        setting.colors = colorBlock_setting;
    }

    private void MoveTab()
    {
        if (dragTime == 0)
        {
            moveTime += Time.deltaTime * 5f;

            if (moveTime >= 1)
                moveTime = 1;

            tabs.position = new Vector3(from + (to - from) * Mathf.Pow(moveTime, 2), screenSize.y * 0.5f, 0);
        }
    }
    #endregion
    #region input
    private void Slide()
    {
        if (isSetting)
        {
            dragTime = 0;
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (dragTime == 0)
            {
                startMousePos = Input.mousePosition;
                startTabPos = tabs.position;
                isBigger0_2 = false;
            }
            dragTime += Time.deltaTime;

            if (dragTime >= 0.2f)
            {
                if (!isBigger0_2)
                {
                    isBigger0_2 = true;
                }
            }

            float distance = Input.mousePosition.x - startMousePos.x;
            float x = startTabPos.x + distance;
            if (x >= screenSize.x * 1.5f) x = screenSize.x * 1.5f;
            if (x <= screenSize.x * -0.5f) x = screenSize.x * -0.5f;
            tabs.position = new Vector3(x, startTabPos.y, 0);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            SlideUp();
            dragTime = 0;
        }
    }

    private void SlideUp()
    {
        if (dragTime <= 0.2f)
        {
            float distance = Input.mousePosition.x - startMousePos.x;
            float per = distance / screenSize.x;
            
            if (per >= 0.2f && nowTab != 2)
                nowTab++;
            else if (per <= -0.2f && nowTab != 0)
                nowTab--;
        }
        else
        {
            if (tabs.position.x <= 0)
                nowTab = 0;
            else if (tabs.position.x >= screenSize.x)
                nowTab = 2;
            else
                nowTab = 1;
        }
    }

    public void SetIsSetting(bool flag)
    {
        isSetting = flag;
    }
    #endregion
}
