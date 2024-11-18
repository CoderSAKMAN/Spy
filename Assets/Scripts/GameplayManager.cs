using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public GameObject warningtext;
    public TextMeshProUGUI personNameText;    // Ki�i ismini g�sterecek Text alan�
    public Image personPhotoImage;            // Ki�i foto�raf�n� g�sterecek Image alan�
    public Button personPanel;                 // Bir sonraki ki�iye ge�i� butonu
    public Button guessButton;                // TAHM�N ET butonu
    public Button giveUpButton;               // PES ET butonu
    public GameObject guessPanel;             // Tahmin paneli, CASUS'u tahmin etmek i�in
    public Transform guessListContainer;      // Tahmin panelindeki liste
    public Button guessButtonPrefab;          // Tahmin i�in ki�i butonlar�

    private List<PersonData> selectedPersons; // Se�ili ki�iler listesi
    private LocationData selectedLocation;    // Se�ili mekan
    private PersonData spy;                   // CASUS olarak se�ilen ki�i
    private int currentPersonIndex = 0;       // �u anki ki�i s�ras�
    private bool isRevealRoleOrLocation = false; // Ki�inin rol�n�n veya mek�n�n g�sterilip g�sterilmedi�ini kontrol eder

    void Start()
    {
        warningtext.SetActive(true);
        selectedPersons = GameData.SelectedPersons;
        var selectedLocations = GameData.SelectedLocations;

        // CASUS'u ve MEKAN'� rastgele se�
        spy = selectedPersons[Random.Range(0, selectedPersons.Count)];
        selectedLocation = selectedLocations[Random.Range(0, selectedLocations.Count)];

        // �lk ki�iyi g�ster
        ShowPerson(currentPersonIndex);

        // Tahmin ve pes et butonlar�n� ba�ta gizle
        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);
    }

    void ShowPerson(int index)
    {
        // G�sterilecek ki�iyi al
        PersonData person = selectedPersons[index];
        personNameText.text = person.name;
        personPhotoImage.sprite = person.photo;
        isRevealRoleOrLocation = false; // Ba�lang��ta rol veya mekan g�sterilmemi� durumda

        // Bir sonraki ki�iye ge�mek i�in butona t�klan�ld���nda yap�lacaklar
        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => OnNextPerson(person));
    }

    void OnNextPerson(PersonData person)
    {
        if (!isRevealRoleOrLocation)
        {
            warningtext.SetActive(false);
            // E�er ilk t�klama ise, rol veya mekan� g�ster
            if (person == spy)
            {
                personNameText.text = $"{person.name} - CASUS!";
            }
            else
            {
                personNameText.text = $"{person.name} - MEKAN: {selectedLocation.name}";
            }
            isRevealRoleOrLocation = true; // �lk t�klamada rol veya mekan g�sterildi
        }
        else
        {
            warningtext.SetActive(true);
            // E�er ikinci t�klama ise, s�radaki ki�iye ge�
            currentPersonIndex++;
            if (currentPersonIndex < selectedPersons.Count)
            {
                ShowPerson(currentPersonIndex);
            }
            else
            {
                EndGame(); // T�m ki�iler g�sterildiyse oyunu bitir
            }
        }
    }

    void EndGame()
    {
        // TAHM�N ET ve PES ET butonlar�n� g�ster
        personPanel.gameObject.SetActive(false);
        guessButton.gameObject.SetActive(true);
        giveUpButton.gameObject.SetActive(true);

        // Tahmin et ve pes et butonlar�na i�lev ekle
        guessButton.onClick.AddListener(ShowGuessPanel);
        giveUpButton.onClick.AddListener(GiveUp);
    }

    void ShowGuessPanel()
    {
        // Tahmin panelini aktif hale getir
        guessPanel.SetActive(true);
        warningtext.SetActive(false);

        // Tahmin panelindeki ki�ileri listele
        foreach (var person in selectedPersons)
        {
            Button guessPersonButton = Instantiate(guessButtonPrefab, guessListContainer);
            guessPersonButton.GetComponentInChildren<TextMeshProUGUI>().text = person.name;
            Image childImage = guessPersonButton.transform.GetChild(0).GetComponent<Image>();
            childImage.sprite = person.photo;
            guessPersonButton.onClick.AddListener(() => OnGuess(person));
        }
    }

    void OnGuess(PersonData guessedPerson)
    {
        // Tahmin edilen ki�i CASUS mu kontrol et
        if (guessedPerson == spy)
        {
            personNameText.text = "Tebrikler! CASUS'u buldun!";
        }
        else
        {
            personNameText.text = $"Yanl�� tahmin! CASUS: {spy.name}";
        }

        // Tahmin panelini ve butonlar� gizle
        guessPanel.SetActive(false);
        personPanel.gameObject.SetActive(true);
        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);
        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => SceneChanger(0));

    }

    void GiveUp()
    {
        // PES ET butonuna bas�ld���nda CASUS'u g�ster
        personNameText.text = $"CASUS: {spy.name}";

        // Butonlar� gizle
        guessPanel.SetActive(false);
        personPanel.gameObject.SetActive(true);
        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);
        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => SceneChanger(0));
    }

    void SceneChanger(int i)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(i);
    }
}
