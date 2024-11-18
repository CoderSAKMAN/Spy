using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;

public class NotificationPanel : MonoBehaviour
{
    public GameObject notificationPanel;
    public TextMeshProUGUI notificationText;
    public Animator animator;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public void OpenPanel(string notification, float animTimer)
    {
        gameObject.SetActive(true);
        animator.SetTrigger("Open");
        notificationText.text = notification;
        StartCoroutine(ObjectClose(animTimer));
    }

    IEnumerator ObjectClose(float animTimer)
    {
        yield return new WaitForSeconds(animTimer * 2);

        animator.SetTrigger("Close");

        yield return new WaitForSeconds(animTimer / 2);

        gameObject.SetActive(false);
    }
}
