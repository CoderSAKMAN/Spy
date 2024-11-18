using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LocationManager : MonoBehaviour
{
    public NotificationPanel notificationPanel;
    public LocationList locationList; // ScriptableObject listesi
    public TMP_InputField nameInput; // �smi almak i�in InputField
    public Image photoPreview; // Foto�raf �nizlemesi
    private Sprite selectedPhoto; // Se�ilen foto�raf

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
            notificationPanel.OpenPanel("L�tfen bir isim ve resim se�iniz.", .6f);
        }
    }
}
