using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// the point of this class is to indicate a heat source
// this class may contain a power level
public class HeatSourceScript : MonoBehaviour
{

    public int heatPower = 10;

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
