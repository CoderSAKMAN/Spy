using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseScript : MonoBehaviour
{
    public GameObject Object;
    public Animator animator;
    public float animTimer;

    public void OpenObject()
    {
        if(animator != null)
        {
            ObjectTrue();
            animator.SetTrigger("Open");
        }
        else
        {
            ObjectTrue();
        }
    }
    public void CloseObject()
    {
        if (animator != null)
        {
            animator.SetTrigger("Close");
            Invoke("ObjectFalse", animTimer);
        }
        else
        {
            ObjectFalse();
        }
    }

    void ObjectTrue()
    {
        Object.SetActive(true);
    }

    void ObjectFalse()
    {
        Object.SetActive(false);
    }
}
