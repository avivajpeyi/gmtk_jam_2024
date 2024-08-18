using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Vector3 = UnityEngine.Vector3;


// import tweening
using DG.Tweening;
using Quaternion = UnityEngine.Quaternion;


public enum SizeName
{
    XSmall,
    Small,
    Regular,
    Large
}


public class PlayerSize : Singleton<PlayerSize>
{
    public float baseGrowthRate = 0.5f;
    public float growthExponent = 1.5f;
    public float shrinkAmount = 1.75f;
    public float minSize = 0.5f;
    public float maxSize = 5f;
    public GameObject shrinkEffect;
    public bool debug = false;

    private HealthSystem _playerHealth;
    
    private Vector3 originalScale;
    public float CurrentSize => transform.localScale.x;
    
    
    public float[] SizeValues = new float[] {1.5f, 2.5f, 3.5f, 4.5f};

    public TMP_Text _txtUi;

    Sequence flashSequence;
    
    
    
    
    
    
    private bool invulnerable = false;
    private float invulnerabilityTime = 0.25f;
    private Renderer _renderer;
    
    void ResetInvulnerability()
    {
        invulnerable = false;
    }
    
    void StartInvulnerability()
    {
        invulnerable = true;
        Invoke(nameof(ResetInvulnerability), invulnerabilityTime);
    }
    
    
    
    private SizeName previousSizeName;
    public SizeName CurrentSizeName
    {
        get
        {
            SizeName _sizeName;
            if (CurrentSize < SizeValues[0])
            {
                _sizeName= SizeName.XSmall;
            }
            else if (CurrentSize < SizeValues[1])
            {
                _sizeName= SizeName.Small;
            }
            else if (CurrentSize < SizeValues[2])
            {
                _sizeName= SizeName.Regular;
            }
            else
            {
                _sizeName= SizeName.Large;
            }
            
            return _sizeName;
        }
    }

    

    // Reduce size on health loss
    public void OnHealthReduced(float val)
    {
        if (val < 0 && CurrentSizeName != SizeName.XSmall)
            Shrink();

        _txtUi.text = CurrentSizeName.ToString();

    }

    // Growth rate property
    private float GrowthRate
    {
        get
        {
            float currentSize = transform.localScale.x;
            return baseGrowthRate * Mathf.Pow(currentSize, growthExponent - 1);
        }
    }

    void Start()
    {
        originalScale = transform.localScale;
        _playerHealth = GetComponent<HealthSystem>();
        _playerHealth.OnHealthChanged.AddListener(OnHealthReduced);
        _txtUi.text = CurrentSizeName.ToString();
        previousSizeName = CurrentSizeName;
        _renderer = GetComponentInChildren<Renderer>();
        // SetupFlashSequence(invulnerabilityTime);
        
    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player collided with " + other.name);
        
        // if player collides with "Obstacle" tag, shrink the player
        if (other.CompareTag("Obstacle") && !invulnerable)
        {
            StartInvulnerability();
            Shrink();
        }
    }

    void Update()
    {   
        Grow();

        // Debug shrink
        if (debug && Input.GetKeyDown(KeyCode.Space))
        {
            Shrink();
            
        }

        CheckSizeNameChange();
        
        _txtUi.text = CurrentSizeName.ToString() + " " + CurrentSize.ToString("00.0");
        
    }

    
    void Grow()
    {
        float currentSize = transform.localScale.x;
        float newScale = currentSize + (GrowthRate * Time.deltaTime);
        newScale = Mathf.Clamp(newScale, minSize, maxSize);
        transform.localScale = Vector3.one * newScale;

    }
    
    void CheckSizeNameChange()
    {
        SizeName currentSizeName = CurrentSizeName;
        if (currentSizeName != previousSizeName)
        {
            // OnSizeNameChanged?.Invoke(currentSizeName);
            previousSizeName = currentSizeName;
            _txtUi.text = CurrentSizeName.ToString();
            
        }

    }

    
    
    public void Shrink()
    {

        // Reduce size to the next level
        // SizeName nextSize;
        // switch (CurrentSizeName)
        // {
        //     case SizeName.Large:
        //         nextSize = SizeName.Regular;
        //         break;
        //     case SizeName.Regular:
        //         nextSize = SizeName.Small;
        //         break;
        //     case SizeName.Small:
        //         nextSize = SizeName.XSmall;
        //         break;
        //     default:
        //         nextSize = SizeName.Small;
        //         break;
        // }

        //
        // // Set the player's scale to the next lower size
        // float newSize = SizeValues[(int)nextSize-1];
        // transform.localScale = Vector3.one * newSize;
        //
        
        
        
        
        float newScale = transform.localScale.x - shrinkAmount;

        if (newScale <= minSize)
        {
            _playerHealth.ModifyHealth(-1);
            return;
        }
        else
        {
            // flashSequence.Play();
            // StartFlashing();
            
            
            transform.localScale = Vector3.one * newScale;
        
            CheckSizeNameChange();
            // Trigger particle effect
            Instantiate(shrinkEffect, transform.position, Quaternion.identity);
        }
        
        


    }


    private void OnDrawGizmos()
    {
        // Draw a circle aroudn the player's MinSize
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minSize);
        
        
    }
    
    


    
    // void SetupFlashSequence(float flashDuration, int flashCount=5)
    // {
    //
    //     flashSequence = DOTween.Sequence();
    //     float singleFlashDuration = flashDuration / (flashCount * 2);
    //
    //     for (int i = 0; i < 5; i++)
    //     {
    //         flashSequence.Append(_renderer.material.DOColor(Color.clear, "_Color", 
    //             singleFlashDuration / 2));
    //         flashSequence.Append(_renderer.material.DOColor(Color.white, "_Color", singleFlashDuration / 2));
    //     }
    //
    //
    // }
    
    
    public void StartFlashing()
    {
        StartCoroutine(FlashRoutine(invulnerabilityTime, 5));
    }

    private IEnumerator FlashRoutine(float flashDuration, int flashCount)
    {
        // Calculate the duration of each individual flash
        float singleFlashDuration = flashDuration / (flashCount * 2);

        for (int i = 0; i < flashCount; i++)
        {
            _renderer.enabled = false;
            yield return new WaitForSeconds(singleFlashDuration);
            _renderer.enabled = true;
            yield return new WaitForSeconds(singleFlashDuration);
        }
    }

}
