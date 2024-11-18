using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public NotificationPanel notificationPanel;

    [Header("Person Settings")]
    public Transform personListContainer;         // Kiþi Scroll View'in Content bölümü
    public Button personButtonPrefab;             // Kiþi buton prefab'ý

    [Header("Location Settings")]
    public Transform locationListContainer;       // Mekan Scroll View'in Content bölümü
    public Button locationButtonPrefab;           // Mekan buton prefab'ý

    [Header("Global Lists")]
    public PersonList personList; // ScriptableObject listesi
    public LocationList locationList; // ScriptableObject listesi

    [Header("Selected Lists")]
    public List<PersonData> selectedPersons = new List<PersonData>();
    public List<LocationData> selectedLocations = new List<LocationData>();

    private void Start()
    {
        SetLists();  // Baþlangýçta verileri yükle ve listeleri ayarla
    }

    public void SetLists()
    {
        if (personList.persons.Count == 0 && locationList.locations.Count == 0) 
        {
            gameObject.SetActive(false);
            notificationPanel.OpenPanel("Lütfen bir kiþi veya mekan ekleyin.", .6f);
        }
        else
        {
            ClearList(personListContainer);
            PopulatePersonList(); // Kiþi listesini doldur

            ClearList(locationListContainer);
            PopulateLocationList(); // Mekan listesini doldur
        }
    }



    private void PopulatePersonList()
    {
        foreach (var person in personList.persons)
        {
            Button newPersonButton = Instantiate(personButtonPrefab, personListContainer);
            newPersonButton.GetComponentInChildren<TextMeshProUGUI>().text = person.name;
            Image childImage = newPersonButton.transform.GetChild(0).GetComponent<Image>();
            childImage.sprite = person.photo;
            newPersonButton.onClick.AddListener(() => OnPersonSelected(person, newPersonButton));
        }
    }

    private void PopulateLocationList()
    {
        foreach (var location in locationList.locations)
        {
            Button newLocationButton = Instantiate(locationButtonPrefab, locationListContainer);
            newLocationButton.GetComponentInChildren<TextMeshProUGUI>().text = location.name;
            Image childImage = newLocationButton.transform.GetChild(0).GetComponent<Image>();
            childImage.sprite = location.photo;
            newLocationButton.onClick.AddListener(() => OnLocationSelected(location, newLocationButton));
        }
    }

    private void ClearList(Transform container)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnPersonSelected(PersonData person, Button button)
    {
        if (selectedPersons.Contains(person))
        {
            selectedPersons.Remove(person);
            button.image.color = Color.white;
        }
        else
        {
            selectedPersons.Add(person);
            button.image.color = Color.green;
        }
    }

    private void OnLocationSelected(LocationData location, Button button)
    {
        if (selectedLocations.Contains(location))
        {
            selectedLocations.Remove(location);
            button.image.color = Color.white;
        }
        else
        {
            selectedLocations.Add(location);
            button.image.color = Color.blue;
        }
    }

    public void OnConfirmSelections()
    {
        if (selectedPersons.Count == 0 || selectedLocations.Count == 0)
        {
            notificationPanel.OpenPanel("Lütfen en az bir kiþi ve mekan seçin!", .6f);
            return;
        }

        GameData.SelectedPersons = new List<PersonData>(selectedPersons);
        GameData.SelectedLocations = new List<LocationData>(selectedLocations);

        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
