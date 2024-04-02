using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveColor : MonoBehaviour
{
    protected bool[] ColorStat = new bool[] { false, false, false };
    public ThisColor curColor { get; private set; }

    public enum ThisColor { BLACK, RED, GREEN, BLUE, YELLOW, MAGENTA, CYAN, WHITE}

    private void Start()
    {
        SetColor(curColor);
    }

    public void SetColor(ThisColor haveColor)
    {
        switch(haveColor)
        {
            case ThisColor.BLACK:
                this.ColorStat[0] = false;
                this.ColorStat[1] = false;
                this.ColorStat[2] = false;
                curColor = ThisColor.BLACK;
                break;
            case ThisColor.RED:
                this.ColorStat[0] = true;
                this.ColorStat[1] = false;
                this.ColorStat[2] = false;
                curColor = ThisColor.RED;
                break;
            case ThisColor.GREEN:
                this.ColorStat[0] = false;
                this.ColorStat[1] = true;
                this.ColorStat[2] = false;
                curColor = ThisColor.GREEN;
                break;
            case ThisColor.BLUE:
                this.ColorStat[0] = false;
                this.ColorStat[1] = false;
                this.ColorStat[2] = true;
                curColor = ThisColor.BLUE;
                break;
            case ThisColor.YELLOW:
                this.ColorStat[0] = true;
                this.ColorStat[1] = true;
                this.ColorStat[2] = false;
                curColor = ThisColor.YELLOW;
                break;
            case ThisColor.MAGENTA:
                this.ColorStat[0] = true;
                this.ColorStat[1] = false;
                this.ColorStat[2] = true;
                curColor = ThisColor.MAGENTA;
                break;
            case ThisColor.CYAN:
                this.ColorStat[0] = false;
                this.ColorStat[1] = true;
                this.ColorStat[2] = true;
                curColor = ThisColor.CYAN;
                break;
            case ThisColor.WHITE:
                this.ColorStat[0] = true;
                this.ColorStat[1] = true;
                this.ColorStat[2] = true;
                curColor = ThisColor.WHITE;
                break;
        }
    }
    public void PeelColor(Weapons.Colors peelingColor)
    {
        switch(peelingColor)
        {
            case Weapons.Colors.RED:
                ChangeColor(false, this.ColorStat[1], this.ColorStat[2]);
                break;
            case Weapons.Colors.GREEN:
                ChangeColor(this.ColorStat[0], false, this.ColorStat[2]);
                break;
            case Weapons.Colors.BLUE:
                ChangeColor(this.ColorStat[0], this.ColorStat[1], false);
                break;
        }
    }
    public void ChangeColor(bool red, bool green, bool blue)
    {
        this.ColorStat[0] = red;
        this.ColorStat[1] = green;
        this.ColorStat[2] = blue;
        UpdateColor();
    }
    public Color MaterialColor()
    {
        switch (curColor)
        {
            case ThisColor.BLACK:
                return Color.black;
            case ThisColor.RED:
                return Color.red;
            case ThisColor.GREEN:
                return Color.green;
            case ThisColor.BLUE:
                return Color.blue;
            case ThisColor.YELLOW:
                return Color.yellow;
            case ThisColor.MAGENTA:
                return Color.magenta;
            case ThisColor.CYAN:
                return Color.cyan;
            case ThisColor.WHITE:
                return Color.white;
        }
        return Color.black;
    }
    private void UpdateColor()
    {
        switch(this.ColorStat[0])
        {
            case true:
                switch (this.ColorStat[1])
                {
                    case true:
                        switch (this.ColorStat[2])
                        {
                            case true:
                                curColor = ThisColor.WHITE;
                                break;
                            case false:
                                curColor = ThisColor.YELLOW;
                                break;
                        }
                        break;
                    case false:
                        switch (this.ColorStat[2])
                        {
                            case true:
                                curColor = ThisColor.MAGENTA;
                                break;
                            case false:
                                curColor = ThisColor.RED;
                                break;
                        }
                        break;
                }
                break;
            case false:
                switch (this.ColorStat[1])
                {
                    case true:
                        switch (this.ColorStat[2])
                        {
                            case true:
                                curColor = ThisColor.CYAN;
                                break;
                            case false:
                                curColor = ThisColor.GREEN;
                                break;
                        }
                        break;
                    case false:
                        switch (this.ColorStat[2])
                        {
                            case true:
                                curColor = ThisColor.BLUE;
                                break;
                            case false:
                                curColor = ThisColor.BLACK;
                                break;
                        }
                        break;
                }
                break;
        }
    }
}
