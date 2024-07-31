using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnvironmentData
{
    public List<string> pickedUpItems;
    public List<string> entitiesRemoved;
    public List<string> entitiesAdded;

    public EnvironmentData(List<string> _pickedUpItems, List<string> _entitiesRemoved, List<string> _entitiesAdded)
    {
        pickedUpItems = _pickedUpItems;
        entitiesRemoved = _entitiesRemoved;
        entitiesAdded = _entitiesAdded;
    }
}

