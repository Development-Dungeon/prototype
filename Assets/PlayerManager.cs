using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour

    
{
    // Start is called before the first frame update
    public GameObject HealthBar;
    public GameObject OxygenTank;
    public float TimeRemaining = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        TimeRemaining -= Time.deltaTime;

        if (TimeRemaining <= 0)
        {
            var healthbarscript = HealthBar.GetComponent<HealthBar>();
            healthbarscript.UpdateHealth(-30);
            TimeRemaining = 10;
        }
       
    }
}
