using UnityEngine;
using System.Collections;
using TMPro;
using System;

public class PlayerDepth : MonoBehaviour
{

    public float _currentDepth;
    public float _maxDepth;
    public float damageFromPressure;
    public float damageTimerLength = 5;
    public int WaterDepth = 553;
    public TMP_Text depthText;
    public event Action<PlayerDepth> PlayerDepthUpdateEvent;

    private Utilities.CountdownTimer DamageTimer;
    private GameObject player;
    private Health PlayerHealth;

    private void Awake()
    {
        SetCurrentDepth(_maxDepth);
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerHealth = player.GetComponent<Health>();
        DamageTimer = new Utilities.CountdownTimer(damageTimerLength);
        DamageTimer.OnTimerStop += PlayerTakeDamage;
    }

    private void PlayerTakeDamage()
    {
        PlayerHealth.TakeDamage(damageFromPressure);
        DamageTimer.Reset(damageTimerLength);
        DamageTimer.Start();
    }

    public void SetMaxDepth(float newMaxDepth)
    {
        // if the new max is different then the old. fire an event
        if(newMaxDepth != _maxDepth) 
		{ 
			_maxDepth = newMaxDepth;

            if (PlayerDepthUpdateEvent != null)
            {  
                PlayerDepthUpdateEvent.Invoke(this);
		    }

		}

    }
    public void SetCurrentDepth(float currentDepth)
    {
        if(_currentDepth != currentDepth)
        {  
			_currentDepth = currentDepth;
            if (PlayerDepthUpdateEvent != null)
            {  
                PlayerDepthUpdateEvent.Invoke(this);
		    }
		}
    }

    private void Update()
    {
        DamageTimer.Tick(Time.deltaTime);
    }
        
    public void FixedUpdate()
    {
	    var calculatedDepth = Mathf.RoundToInt(WaterDepth - player.transform.position.y);

        if(calculatedDepth < 0)
        {
            SetCurrentDepth(0);
		} 
		else
        {
            SetCurrentDepth(calculatedDepth);
		}
        
        UpdateDepthUI();
        TriggerDamageEvent();

    }

    private void TriggerDamageEvent()
    {
        if (_currentDepth > _maxDepth)
        {
            if (!DamageTimer.IsRunning)
            {  
                DamageTimer.Reset(damageTimerLength);
                DamageTimer.Start();
		    }
	    }
        else if(_currentDepth <= _maxDepth)
        {
            if (DamageTimer.IsRunning)
                DamageTimer.Pause();
		}
    }

    private void UpdateDepthUI()
    {
        if(_currentDepth > 0)
        {
			depthText.text = _currentDepth + "/" + _maxDepth + " meters";

		}
        else
        {  
			depthText.text = "";
		}


    }

    public void IncreaseMax(int depthBonus)
    {
        SetMaxDepth(_maxDepth + depthBonus);
    }
}
