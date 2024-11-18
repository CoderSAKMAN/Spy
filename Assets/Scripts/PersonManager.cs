using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PersonManager : MonoBehaviour
{
    public SaveManager saveManager;
    public NotificationPanel notificationPanel;
    public PersonList personList; // ScriptableObject listesi
    public TMP_InputField nameInput; // �smi almak i�in InputField
    public Image photoPreview; // Foto�raf �nizlemesi
    private Sprite selectedPhoto; // Se�ilen foto�raf

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
            notificationPanel.OpenPanel("L�tfen bir isim ve resim se�iniz.", .6f);
        }
    }
}
