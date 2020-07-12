using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeterHandler : MonoBehaviour
{
    // max value is set by script handling score
    public float MaxValue;
    
    [SerializeField]
    public float currentValue;
    [SerializeField]
    private float maxSize;
    [SerializeField]
    private float currentSize;

    public GameObject MeterForeground; // set in unity inspector
    public GameObject MeterBackground; // set in unity inspector

    private RectTransform foregroundRect;

    // Start is called before the first frame update
    void Start()
    {
        currentSize = MeterForeground.GetComponent<RectTransform>().sizeDelta.x;
        maxSize = MeterBackground.GetComponent<RectTransform>().sizeDelta.x;
        foregroundRect = MeterForeground.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (foregroundRect.sizeDelta.x != currentSize)
        {
            var oldSize = foregroundRect.sizeDelta;
            foregroundRect.sizeDelta = new Vector2(currentSize, oldSize.y);
        }
    }

    public float SetValueTo(float value)
    {
        if (value < 0 || value > MaxValue) return currentValue;

        currentValue = value;
        var ratio = currentValue / MaxValue;
        currentSize = maxSize * ratio;

        return currentValue;
    }

    public float IncreaseValueBy(float delta)
    {
        if (currentValue == MaxValue) return currentValue;
        currentValue += delta;
        var ratio = currentValue / MaxValue;
        currentSize = maxSize * ratio;
        return currentValue;
    }

    public float DecreaseValueBy(float delta)
    {
        if (currentValue <= 0) return currentValue;
        currentValue -= delta;
        var ratio = currentValue / MaxValue;
        currentSize = maxSize * ratio;
        return currentValue;
    }
}
