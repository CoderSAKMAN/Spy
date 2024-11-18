using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonManager : MonoBehaviour
{
    public SaveManager saveManager;
    public NotificationPanel notificationPanel;
    public PersonList personList; // ScriptableObject listesi
    public TMP_InputField nameInput; // Ýsmi almak için InputField
    public Image photoPreview; // Fotoðraf önizlemesi
    private Sprite selectedPhoto; // Seçilen fotoðraf

    public void SelectPhoto(Sprite photo)
    {
        selectedPhoto = photo;
        photoPreview.sprite = photo;
    }

    public void AddPerson()
    {
        if (!string.IsNullOrEmpty(nameInput.text) && selectedPhoto != null)
        {
            PersonData newPerson = new PersonData
            {
                name = nameInput.text,
                photo = selectedPhoto
            };
            personList.persons.Add(newPerson);
            saveManager.SaveData();

            nameInput.text = "";
            photoPreview.sprite = null;
            selectedPhoto = null;
        }
        else
        {
            notificationPanel.OpenPanel("Lütfen bir isim ve resim seçiniz.", .6f);
        }
    }
}
