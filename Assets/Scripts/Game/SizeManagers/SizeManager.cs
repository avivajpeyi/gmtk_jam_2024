using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SizeManager : MonoBehaviour
{
    SizeTarget sizeTarget;
    PlayerSize playerSize;
    IScoreSystem _scoreSystem;
    
    
    
    
    //
    // public UnityEvent OnTargetReached = new UnityEvent();

    
    // Start is called before the first frame update
    void Start()
    {
        playerSize = FindObjectOfType<PlayerSize>();
        sizeTarget = FindObjectOfType<SizeTarget>();
        _scoreSystem = InterfaceUtilities.FindObjectOfType<IScoreSystem>();

        Invoke("UpdateSize", 0.1f);
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (SizesMatch)
        {
            UpdateSize();
            _scoreSystem.ModifyScore(10);
            // OnTargetReached.Invoke();
               
        }
        
    }


    void UpdateSize()
    {
        sizeTarget.ChangeTargetSize();
        sizeTarget.SetText(playerTooBig); 
    }

    bool SizesMatch
    {
        get
        {
            return (int) playerSize.CurrentSizeName == (int) sizeTarget.targetSize;
        }   
    }
    
    
    bool playerTooBig
    {
        get
        {
            return (int) playerSize.CurrentSizeName > (int) sizeTarget.targetSize;
        }
    }
    
    
    
}
