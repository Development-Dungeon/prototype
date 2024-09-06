using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerDepth : MonoBehaviour
{

    public float _currentDepth;
    public float _maxDepth;
    public float damageFromPressure;
    public float damageTimerLength = 5;
    public int WaterDepth = 553;
    public TMP_Text depthText;

    private Utilities.CountdownTimer DamageTimer;
    private GameObject player;
    private Health PlayerHealth;

    private void Awake()
    {
        _currentDepth = _maxDepth;
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

    private void Start() 
    { 

    }
    public void SetMaxHealth(float maxHealth) => _maxDepth = maxHealth;
    public void SetCurrentHealth(float currentHealth) => _currentDepth = currentHealth;
    public bool IsDead() => _currentDepth <= 0;

    private void Update()
    {
        DamageTimer.Tick(Time.deltaTime);
    }
        

    public void FixedUpdate()
    {
	    _currentDepth = Mathf.RoundToInt(WaterDepth - player.transform.position.y);
        if (_currentDepth < 0) _currentDepth = 0;

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

}
