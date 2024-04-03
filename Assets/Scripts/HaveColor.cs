using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HaveColor : MonoBehaviour
{
    // private bool[] ColorStat = new bool[] { false, false, false };
    // 비트맵 방식으로 교체
    private sbyte ColorStat = 0b000;
    public ThisColor curColor { get; private set; }

    public enum ThisColor { BLACK = 0, RED = 4, GREEN = 2, BLUE = 1, YELLOW = 6, MAGENTA = 5, CYAN = 3, WHITE = 7}

    private void Start()
    {
        SetColor(curColor);
    }

    /// <summary>현제 보유중인 색 속성을 지정</summary>
    /// <param name="haveColor">haveColor는 HaveColor.ThisColor의 색중 하나이며 haveColor의 색 속성으로 지정해줍니다.</param>
    public void SetColor(ThisColor haveColor)
    {
        switch(haveColor)
        {
            case ThisColor.BLACK:
                this.ColorStat = 0b000;
                curColor = ThisColor.BLACK;
                break;
            case ThisColor.RED:
                this.ColorStat = 0b100;
                curColor = ThisColor.RED;
                break;
            case ThisColor.GREEN:
                this.ColorStat = 0b010;
                curColor = ThisColor.GREEN;
                break;
            case ThisColor.BLUE:
                this.ColorStat = 0b001;
                curColor = ThisColor.BLUE;
                break;
            case ThisColor.YELLOW:
                this.ColorStat = 0b110;
                curColor = ThisColor.YELLOW;
                break;
            case ThisColor.MAGENTA:
                this.ColorStat = 0b101;
                curColor = ThisColor.MAGENTA;
                break;
            case ThisColor.CYAN:
                this.ColorStat = 0b011;
                curColor = ThisColor.CYAN;
                break;
            case ThisColor.WHITE:
                this.ColorStat = 0b111;
                curColor = ThisColor.WHITE;
                break;
        }
    }
    /// <summary>지정한 색 속성을 벗깁니다.</summary>
    /// <param name="peelingColor">peelingColor는 벗길 색입니다. Weapons.Colors값을 받으며 이에 대응하는 색을 벗깁니다.</param>
    public void PeelColor(Weapons.Colors peelingColor)
    {
        switch(peelingColor)
        {
            case Weapons.Colors.RED:
                this.ColorStat &= ~(1 << 2); // RED
                break;
            case Weapons.Colors.GREEN:
                this.ColorStat &= ~(1 << 1); // GREEN
                break;
            case Weapons.Colors.BLUE:
                this.ColorStat &= ~(1 << 0); // BLUE
                break;
        }
        UpdateColor();
    }
    /// <summary>
    /// 현제 속성이 가진 색 속성에 대응하는 Color값을 반환합니다.
    /// Material의 색이나 Color를 변화시키는데 도움이 됩니다.
    /// </summary>
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
    /// <summary>
    /// 현제 ColorStat로 curColor를 갱신합니다.
    /// </summary>
    private void UpdateColor()
    {
        /*
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
        */
        curColor = (ThisColor)ColorStat;
    }

    /*
/// <summary>지정된 RGB값으로 색 속성을 지정합니다.</summary>
/// <param name="red">red는 빨강(Red)속성이며 이를 지니는지 true/false로 지정합니다.</param>
/// <param name="green">green는 초록(Green)속성이며 이를 지니는지 true/false로 지정합니다.</param>
/// <param name="blue">blue는 파랑(Blue)속성이며 이를 지니는지 true/false로 지정합니다.</param>
public void ChangeColor(bool red, bool green, bool blue)
{
    this.ColorStat[0] = red;
    this.ColorStat[1] = green;
    this.ColorStat[2] = blue;
    UpdateColor();
}
*/
    
}
