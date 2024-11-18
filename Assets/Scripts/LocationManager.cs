using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{
    public NotificationPanel notificationPanel;
    public LocationList locationList; // ScriptableObject listesi
    public TMP_InputField nameInput; // Ýsmi almak için InputField
    public Image photoPreview; // Fotoðraf önizlemesi
    private Sprite selectedPhoto; // Seçilen fotoðraf

    public void SelectPhoto(Sprite photo)
    {
        selectedPhoto = photo;
        photoPreview.sprite = photo;
    }

    public void AddLocation()
    {
        if (!string.IsNullOrEmpty(nameInput.text) && selectedPhoto != null)
        {
            LocationData newLocation = new LocationData
            {
                name = nameInput.text,
                photo = selectedPhoto
            };
            locationList.locations.Add(newLocation);

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
