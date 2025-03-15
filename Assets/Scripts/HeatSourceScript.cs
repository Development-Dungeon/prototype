using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeatSourceScript : MonoBehaviour
{
    // draw the sphere around the heat source range ? 

    public float heatPower = 10.0f;
    public float heatDissipationRate = 1.0f;
    public bool isActive = false;
    public bool drawGizmos = false;
    
    void OnDrawGizmos()
    {
        if (!drawGizmos)
            return;
        
        Gizmos.color = Color.red;

        var radius = heatPower / heatDissipationRate;
        
        Gizmos.DrawWireSphere(transform.position, radius); 
    }

    private void Start()
    {
        if(HeatSourceManagerScript.Instance != null)
			HeatSourceManagerScript.Instance.RegisterHeatSource(this);
    }


    private void OnDestroy()
    {
        if(HeatSourceManagerScript.Instance != null)
			HeatSourceManagerScript.Instance.UnRegisterHeatSource(this);
    }

}
