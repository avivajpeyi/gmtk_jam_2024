﻿using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float verticalSpeed = 15.0f;
    [SerializeField] private float horizontalSpeed = 10.0f;
    
    private IHealthSystem _healthSystem;
    private IHorizontalInputProvider _horizontalInputProvider;
    private Rigidbody _rigidbody;
    private EventManager _eventManager;
    
    private void Awake()
    {
        _eventManager = FindObjectOfType<EventManager>();
        
        _rigidbody = GetComponent<Rigidbody>();

        _healthSystem = GetComponent<IHealthSystem>();

        _horizontalInputProvider = new HorizontalInputProvider();
    }

    private void Start()
    {
        _rigidbody.velocity = new Vector3(0, 0, -verticalSpeed);

        if (_healthSystem != null)
        {
            Debug.Log("Health System Found!");
            _healthSystem.OnHealthEmpty.AddListener(Die);
        }
    }

    private void FixedUpdate()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        var horizontalInput = _horizontalInputProvider.GetHorizontalInput();
        
        var movement = new Vector3(horizontalInput, 0,0);
        
        // ensure that the vertical speed is constant
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y, -verticalSpeed);
        
        _rigidbody.AddForce(movement*horizontalSpeed);
    }
    
    private void Die()
    {
        //Invoke Game Over
        Debug.Log("Player Died!");
        _eventManager.GetEvent<GameOverEvent>().Invoke();
    }
}