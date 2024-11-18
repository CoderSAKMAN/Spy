using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewLocationList", menuName = "CASUS/Location List")]
public class LocationList : ScriptableObject
{
    public List<LocationData> locations = new List<LocationData>();
}
