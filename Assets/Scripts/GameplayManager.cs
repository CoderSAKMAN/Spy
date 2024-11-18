using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameplayManager : MonoBehaviour
{
    public GameObject warningtext;
    public TextMeshProUGUI personNameText;    // Kiþi ismini gösterecek Text alaný
    public Image personPhotoImage;            // Kiþi fotoðrafýný gösterecek Image alaný
    public Button personPanel;                 // Bir sonraki kiþiye geçiþ butonu
    public Button guessButton;                // TAHMÝN ET butonu
    public Button giveUpButton;               // PES ET butonu
    public GameObject guessPanel;             // Tahmin paneli, CASUS'u tahmin etmek için
    public Transform guessListContainer;      // Tahmin panelindeki liste
    public Button guessButtonPrefab;          // Tahmin için kiþi butonlarý

    private List<PersonData> selectedPersons; // Seçili kiþiler listesi
    private LocationData selectedLocation;    // Seçili mekan
    private PersonData spy;                   // CASUS olarak seçilen kiþi
    private int currentPersonIndex = 0;       // Þu anki kiþi sýrasý
    private bool isRevealRoleOrLocation = false; // Kiþinin rolünün veya mekânýn gösterilip gösterilmediðini kontrol eder

    void Start()
    {
        warningtext.SetActive(true);
        selectedPersons = GameData.SelectedPersons;
        var selectedLocations = GameData.SelectedLocations;

        // CASUS'u ve MEKAN'ý rastgele seç
        spy = selectedPersons[Random.Range(0, selectedPersons.Count)];
        selectedLocation = selectedLocations[Random.Range(0, selectedLocations.Count)];

        // Ýlk kiþiyi göster
        ShowPerson(currentPersonIndex);

        // Tahmin ve pes et butonlarýný baþta gizle
        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);
    }

    void ShowPerson(int index)
    {
        // Gösterilecek kiþiyi al
        PersonData person = selectedPersons[index];
        personNameText.text = person.name;
        personPhotoImage.sprite = person.photo;
        isRevealRoleOrLocation = false; // Baþlangýçta rol veya mekan gösterilmemiþ durumda

        // Bir sonraki kiþiye geçmek için butona týklanýldýðýnda yapýlacaklar
        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => OnNextPerson(person));
    }

    void OnNextPerson(PersonData person)
    {
        if (!isRevealRoleOrLocation)
        {
            warningtext.SetActive(false);
            // Eðer ilk týklama ise, rol veya mekaný göster
            if (person == spy)
            {
                personNameText.text = $"{person.name} - CASUS!";
            }
            else
            {
                personNameText.text = $"{person.name} - MEKAN: {selectedLocation.name}";
            }
            isRevealRoleOrLocation = true; // Ýlk týklamada rol veya mekan gösterildi
        }
        else
        {
            warningtext.SetActive(true);
            // Eðer ikinci týklama ise, sýradaki kiþiye geç
            currentPersonIndex++;
            if (currentPersonIndex < selectedPersons.Count)
            {
                ShowPerson(currentPersonIndex);
            }
            else
            {
                EndGame(); // Tüm kiþiler gösterildiyse oyunu bitir
            }
        }
    }

    void EndGame()
    {
        // TAHMÝN ET ve PES ET butonlarýný göster
        personPanel.gameObject.SetActive(false);
        guessButton.gameObject.SetActive(true);
        giveUpButton.gameObject.SetActive(true);

        // Tahmin et ve pes et butonlarýna iþlev ekle
        guessButton.onClick.AddListener(ShowGuessPanel);
        giveUpButton.onClick.AddListener(GiveUp);
    }

    void ShowGuessPanel()
    {
        // Tahmin panelini aktif hale getir
        guessPanel.SetActive(true);
        warningtext.SetActive(false);

        // Tahmin panelindeki kiþileri listele
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
        // Tahmin edilen kiþi CASUS mu kontrol et
        if (guessedPerson == spy)
        {
            personNameText.text = "Tebrikler! CASUS'u buldun!";
        }
        else
        {
            personNameText.text = $"Yanlýþ tahmin! CASUS: {spy.name}";
        }

        // Tahmin panelini ve butonlarý gizle
        guessPanel.SetActive(false);
        personPanel.gameObject.SetActive(true);
        guessButton.gameObject.SetActive(false);
        giveUpButton.gameObject.SetActive(false);
        personPanel.onClick.RemoveAllListeners();
        personPanel.onClick.AddListener(() => SceneChanger(0));

    }

    void GiveUp()
    {
        // PES ET butonuna basýldýðýnda CASUS'u göster
        personNameText.text = $"CASUS: {spy.name}";

        // Butonlarý gizle
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
