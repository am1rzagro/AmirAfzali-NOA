using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPopupManager : MonoBehaviour
{
    public static UIPopupManager Instance;

    [System.Serializable]
    public class Messages
    {
        [System.Serializable]
        public struct Item
        {
            [TextArea] public string Description;

            public Sprite Icon;
        }

        [SerializeField] private Item LoseGameMessage;
        [SerializeField] private Item ToturialMessage;

        public void ShowLoseMessage() => Instance.Show(LoseGameMessage.Description);
        public void ShowToturialMessage() => Instance.Show(ToturialMessage.Description);

    }

    [SerializeField] private GameObject Root;

    [SerializeField] private Text txtHeader;
    [SerializeField] private Text txtDescription;

    [SerializeField] private Button BtnOK;

    public Messages messages;

    private void Awake()
    {
        Instance = this;
        BtnOK.onClick.AddListener(Hide);
    }

    public void Show(string Description)
    {
        Root.gameObject.SetActive(true);
        txtDescription.text = Description;
    }

    public void Hide()
    {
        Root.gameObject.SetActive(false);
    }
}
