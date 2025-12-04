using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public GameObject sayacPanel;
    public TextMeshProUGUI sayacText;
    public Button increaseTimeButton; // Süreyi artýrma butonu
    public GameObject warningtext;
    public TextMeshProUGUI personNameText;
    public Image personPhotoImage;
    public Button personPanel;
    public Button guessButton;
    public Button giveUpButton;
    public GameObject guessPanel;
    public Transform guessListContainer;
    public Button guessButtonPrefab;

    private List<PersonData> selectedPersons;
    private LocationData selectedLocation;
    private PersonData spy;
    private int currentPersonIndex = 0;
    private bool isRevealRoleOrLocation = false;

    private float remainingTime = 300f; // Baþlangýç süresi: 5 dakika
    private bool isTimerRunning = false; // Sayaç durumu

    void Start()
    {
        sayacPanel.SetActive(false);
        warningtext.SetActive(true);
        selectedPersons = GameData.SelectedPersons;
        var selectedLocations = GameData.SelectedLocations;

        spy = selectedPersons[Random.Range(0, selectedPersons.Count)];
        selectedLocation = selectedLocations[Random.Range(0, selectedLocations.Count)];

        ShowPerson(currentPersonIndex);

        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);

        // Süre artýrma butonuna iþlev ekle
        increaseTimeButton.onClick.AddListener(AddOneMinute);
    }

    void ShowPerson(int index)
    {
        PersonData person = selectedPersons[index];
        personNameText.text = person.name;
        personPhotoImage.sprite = person.photo;
        isRevealRoleOrLocation = false;

        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => OnNextPerson(person));
    }

    void OnNextPerson(PersonData person)
    {
        if (!isRevealRoleOrLocation)
        {
            warningtext.SetActive(false);
            if (person == spy)
            {
                personNameText.text = $"{person.name} - CASUS!";
            }
            else
            {
                personNameText.text = $"{person.name} - MEKAN: {selectedLocation.name}";
            }
            isRevealRoleOrLocation = true;
        }
        else
        {
            warningtext.SetActive(true);
            currentPersonIndex++;
            if (currentPersonIndex < selectedPersons.Count)
            {
                ShowPerson(currentPersonIndex);
            }
            else
            {
                EndGame();
            }
        }
    }

    void EndGame()
    {
        Sayac();

        personPanel.gameObject.SetActive(false);
        guessButton.gameObject.SetActive(true);
        giveUpButton.gameObject.SetActive(true);

        guessButton.onClick.AddListener(ShowGuessPanel);
        giveUpButton.onClick.AddListener(GiveUp);
    }

    void Sayac()
    {
        sayacPanel.SetActive(true);
        isTimerRunning = true;
    }

    void Update()
    {
        if (isTimerRunning && remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            sayacText.text = $"{minutes:D2}:{seconds:D2}";
        }
        else if (remainingTime <= 0 && isTimerRunning)
        {
            isTimerRunning = false;
            sayacText.text = "00:00";
            GiveUp();
        }
    }

    void AddOneMinute()
    {
        remainingTime += 60f; // Süreyi 1 dakika artýr
    }

    void ShowGuessPanel()
    {
        guessPanel.SetActive(true);
        warningtext.SetActive(false);

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
        if (guessedPerson == spy)
        {
            personPhotoImage.sprite = spy.photo;
            personNameText.text = "Tebrikler! CASUS'u buldun!";
        }
        else
        {
            personPhotoImage.sprite = spy.photo;
            personNameText.text = $"Yanlýþ tahmin! CASUS: {spy.name}";
        }

        guessPanel.SetActive(false);
        personPanel.gameObject.SetActive(true);
        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);
        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => SceneChanger(0));
    }

    void GiveUp()
    {
        personNameText.text = $"CASUS: {spy.name}";

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
