using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class PointOfInterestWithEvent : MonoBehaviour
{
    public static event Action<PointOfInterestWithEvent> OnPointOfInterestEntered;

    [SerializeField]private string _poiname;

    public string Poiname { get => _poiname; }

    void OnDisable()
    {
        if (OnPointOfInterestEntered != null)
            OnPointOfInterestEntered(this);
    }
}
