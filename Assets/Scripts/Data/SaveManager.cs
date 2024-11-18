using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private const string LocationFileName = "locations.json";
    private const string PersonFileName = "persons.json";

    public LocationList locationList; // Atanacak LocationList ScriptableObject
    public PersonList personList;     // Atanacak PersonList ScriptableObject

    public void SaveData()
    {
        SaveLocations();
        SavePersons();
    }

    public void LoadData()
    {
        LoadLocations();
        LoadPersons();
    }

    private void SaveLocations()
    {
        if (locationList != null)
        {
            string json = JsonUtility.ToJson(locationList, true);
            File.WriteAllText(GetFilePath(LocationFileName), json);
            Debug.Log("Locations saved.");
        }
        else
        {
            Debug.LogWarning("LocationList is null. Cannot save locations.");
        }
    }

    private void SavePersons()
    {
        if (personList != null)
        {
            string json = JsonUtility.ToJson(personList, true);
            File.WriteAllText(GetFilePath(PersonFileName), json);
            Debug.Log("Persons saved.");
        }
        else
        {
            Debug.LogWarning("PersonList is null. Cannot save persons.");
        }
    }

    private void LoadLocations()
    {
        string path = GetFilePath(LocationFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, locationList);
            Debug.Log("Locations loaded.");
        }
        else
        {
            Debug.LogWarning($"Locations file not found at {path}");
        }
    }

    private void LoadPersons()
    {
        string path = GetFilePath(PersonFileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            JsonUtility.FromJsonOverwrite(json, personList);
            Debug.Log("Persons loaded.");
        }
        else
        {
            Debug.LogWarning($"Persons file not found at {path}");
        }
    }

    private string GetFilePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
