using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class Modal : MonoBehaviour
{
    TMP_Text title;
    TMP_Text description;
    Button cancelButton;
    Button continueButton;

    void Awake()
    {
        title = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        description = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        cancelButton = transform.GetChild(2).GetChild(0).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetChild(1).GetComponent<Button>();
        cancelButton.onClick.AddListener(() => { gameObject.SetActive(false); });
    }

    public void ActivateModal(string title, string description, UnityAction action)
    {
        gameObject.SetActive(true);
        Debug.Assert(title != null);
        Debug.Assert(this.title != null);
        this.title.text = title;
        this.description.text = description;
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(action);

        LayoutRebuilder.ForceRebuildLayoutImmediate(transform.GetComponent<RectTransform>());
    }

}
