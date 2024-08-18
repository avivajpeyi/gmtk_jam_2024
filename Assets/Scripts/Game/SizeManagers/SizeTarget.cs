using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;

public class SizeTarget : Singleton<SizeTarget>
{
    public float[] horizontalScales = new float[] { 4, 6, 10 };
    public float changeInterval = 2f; // Time in seconds between size changes

    [SerializeField] private RectTransform rectTransform;
    private int currentScaleIndex = 0;
    private float originalHeight;

    public bool debug = false;

    public TMP_Text _txtUi;


    private Sequence textAnim;
    public SizeName targetSize = SizeName.Small;
    
    
    string leftArrow = "<size=150%>←</size>";
    string rightArrow = "<size=150%>→</size>";
    
    
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        originalHeight = rectTransform.sizeDelta.y;
    }



    public void ChangeTargetSize()
    {
        // change the size of the target different from the current size
        
        SizeName newTargetSize;
        do
        {
            newTargetSize = (SizeName)UnityEngine.Random.Range(1, 4); // 1 to 3 (Small to Large)
        } while (newTargetSize == targetSize);
        
        
        switch (newTargetSize)
        {
            case SizeName.Small:
                currentScaleIndex = 1;
                break;
            case SizeName.Regular:
                currentScaleIndex = 2;
                break;
            case SizeName.Large:
                currentScaleIndex = 3;
                break;
        }
        
        targetSize = newTargetSize;
        float newWidth = originalHeight * horizontalScales[currentScaleIndex-1];
        
        Vector2 newRectSize = new Vector2(newWidth, originalHeight);
        // DoTween to animate rectTransform.sizeDelta
        rectTransform.DOSizeDelta(newRectSize, 0.5f);
        _txtUi.text = targetSize.ToString();
    }


    void Update()
    {
        if (debug)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ChangeTargetSize();
            }
        }
    }

    

    public void SetText(bool playerTooBig)
    {
        if (playerTooBig)
        {
            _txtUi.text = rightArrow + "Shrink" + leftArrow;
            // DoTween to make _txtUi look like its beeing squashed from the sides
            textAnim = DOTween.Sequence()
                .Append(_txtUi.transform.DOScaleX(0.85f, 0.3f))
                .Append(_txtUi.transform.DOScaleX(1.0f, 0.3f));
            // loop the animation
            textAnim.SetLoops(-1, LoopType.Yoyo);
            textAnim.Play();
        }
        else
        {
            _txtUi.text = leftArrow + "Grow" + rightArrow;
            textAnim = DOTween.Sequence()
                .Append(_txtUi.transform.DOScaleX(1.15f, 0.3f))
                .Append(_txtUi.transform.DOScaleX(1.0f, 0.3f));
            textAnim.SetLoops(-1, LoopType.Yoyo);
            textAnim.Play();
        }
    }
    
}