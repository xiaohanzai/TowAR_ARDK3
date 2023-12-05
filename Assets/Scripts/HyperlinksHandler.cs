using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class HyperlinksHandler : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the click was on a link
        TMP_Text textComponent = GetComponent<TMP_Text>();
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textComponent, eventData.position, null);

        if (linkIndex != -1)
        {
            // Handle the click event for the link
            string linkID = textComponent.textInfo.linkInfo[linkIndex].GetLinkID();
            Debug.Log($"Link clicked: {linkID}");

            // Open the link in a web browser or perform some other action
            Application.OpenURL(linkID);
        }
    }
}

