using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrail : MonoBehaviour
{
    public PlayerSize playerSize;
    public float endWidthMultiplier = 0.1f;

    private TrailRenderer trailRenderer;

    void Start()
    {
        trailRenderer = GetComponent<TrailRenderer>();
        if (playerSize == null)
        {
            playerSize = GetComponentInParent<PlayerSize>();
        }
        if (playerSize == null)
        {
            Debug.LogError("playerSize not found. Please assign it in the inspector or ensure it's on the same GameObject.");
        }
    }

    void Update()
    {
        if (playerSize != null && trailRenderer != null)
        {
            UpdateTrailWidth();
        }
    }

    void UpdateTrailWidth()
    {
        float currentScale = playerSize.CurrentSize;
        trailRenderer.startWidth = currentScale;
        trailRenderer.endWidth = currentScale * endWidthMultiplier;
    }
}
