using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the point of this class is to indicate a heat source
// this class may contain a power level
public class HeatSourceScript : MonoBehaviour
{

    public float heatPower = 10.0f;
    public float heatDissipationRate = 1.0f;
    public bool isActive = false;

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
