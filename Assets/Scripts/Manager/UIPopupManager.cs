using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
        [SerializeField] private Item WinMessage;

        public void ShowLoseMessage() => Instance.Show(LoseGameMessage.Description);
        public void ShowToturialMessage(UnityAction action) => Instance.Show(ToturialMessage.Description, action);
        public void ShowWinMessage(UnityAction action) => Instance.Show(WinMessage.Description, action);

    }

    [SerializeField] private GameObject Root;

    [SerializeField] private Text txtHeader;
    [SerializeField] private Text txtDescription;

    [SerializeField] private Button BtnOK;

    public Messages messages;

    private UnityAction ButtonAction;

    private void Awake()
    {
        Instance = this;
        BtnOK.onClick.AddListener(OnClick);
    }

    public void Show(string Description)
    {
        Root.gameObject.SetActive(true);
        txtDescription.text = Description;
    }
    public void Show(string Description ,UnityAction action )
    {
        Root.gameObject.SetActive(true);
        txtDescription.text = Description;
        ButtonAction = action;
    }

    public void Hide()
    {
        Root.gameObject.SetActive(false);
    }

    private void OnClick()
    {
        if (ButtonAction != null)
            ButtonAction();

        Hide();
    }
}
