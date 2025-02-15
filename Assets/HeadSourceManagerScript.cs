using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is in charge of keeping track of all the heat objects inside the scene
public class HeatSourceManagerScript : MonoBehaviour
{

    public List<HeatSourceScript> heatSources;

    public static HeatSourceManagerScript Instance;

    // Start is called before the first frame update

    private void Awake()
    {
        Instance = this;
        heatSources = new(); 
    }

    void Start()
    {
        // search for all the game objects in the scene which already exist under the heat source script (or tag) 
        //  and add them to the array
        
    }

    // Update is called once per frame
    void Update()
    {
        // i don't think it has to do anything on the update
    }

    public void RegisterHeatSource(HeatSourceScript heatSource)
    {
        if (heatSources == null)
            heatSources = new List<HeatSourceScript>();

        heatSources.Add(heatSource);
    }

    public void UnRegisterHeatSource(HeatSourceScript targetHeatSource)
    {
        if (heatSources == null)
            return;

        if(heatSources.Contains(targetHeatSource))
			heatSources.Remove(targetHeatSource);
    }

    public bool TargetWithinDistance(Transform target)
    {
        bool targetWithinDistance = true;
        bool targetNotWithinDistance = false;

        if (target == null || heatSources == null || heatSources.Count == 0)
            return targetNotWithinDistance;

        RemoveNullsFromHeatSource();

        foreach(var hSource in heatSources)
        {

            // get the distance between the heat source and the target
            var distance = Vector3.Distance(hSource.transform.position, target.position);
            // get the power of this heat source
            var power = hSource.heatPower; // this is measured in meters or the same metric as distance
            if (power >= distance )
                return targetWithinDistance;
		}


        return targetNotWithinDistance;

    }

    private void RemoveNullsFromHeatSource()
    {
        if (heatSources == null || heatSources.Count == 0)
            return;

        heatSources.RemoveAll(source => source == null || source.gameObject == null);

    }
}
