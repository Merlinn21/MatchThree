using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : Subject
{ 
    [SerializeField]private string poiName;

    private void OnDisable()
    {
        Notify(poiName);
    }
}
