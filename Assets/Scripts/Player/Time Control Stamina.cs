using UnityEngine;
using UnityEngine.UI;

public class TimeControlStamina : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] TimeFlowManager timeFlowManager;
    public Slider staminaSlider;

    [Header("Spec")]
    [SerializeField] float staminaCapacity;
    [SerializeField] float staminaShrinkAmount;
    [SerializeField] float staminaRegainAmount;

    public float curStamina { get; private set; }
    private bool overHeat;
    private float curColorNum;
    private Image fillColor;

    private void Start()
    {
        curStamina = staminaCapacity;
        overHeat = false;
        curColorNum = 0;
    }

    private void FixedUpdate()
    {
        SliderCalculate();
        if (staminaSlider != null)
        {
            staminaSlider.value = curStamina / staminaCapacity;
            SliderColor();
        }
    }

    private void SliderCalculate()
    {
        if (staminaSlider != null)
        {
            if (timeFlowManager.Slow && !overHeat)
            {
                curStamina -= staminaShrinkAmount * Time.unscaledDeltaTime;
                if (curStamina <= 0)
                {
                    curStamina = 0;
                    overHeat = true;
                }
            }
            else
            {
                if (curStamina < staminaCapacity)
                    curStamina += staminaRegainAmount * Time.fixedDeltaTime;
                if (overHeat && curStamina > staminaCapacity)
                {
                    curStamina = staminaCapacity;
                    overHeat = false;
                }
            }
        }
    }
    private void SliderColor()
    {
        fillColor = staminaSlider.fillRect.gameObject.GetComponent<Image>();
        curColorNum = curStamina / staminaCapacity;
        if (overHeat)
        {
            fillColor.color = new Color(255, curColorNum, curColorNum);
        }
        else if (timeFlowManager.Slow && CanSlow())
        {
            if(curColorNum > 0.6)
                fillColor.color = Color.white;
            else if(curColorNum < 0.6 && curColorNum > 0.2)
                fillColor.color = new Color(255, 128, 0);
            else if(curColorNum < 0.2)
                fillColor.color = new Color(255, 0, 0);
        }
        else
            fillColor.color = Color.white;
    }

    public bool CanSlow()
    {
        if (overHeat)
            return false;
        return true;
    }
}
