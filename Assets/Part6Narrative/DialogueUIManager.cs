using UnityEngine;
//We're gonna be using the text mesh pro library so we add it
using TMPro;
//
using UnityEngine.UI;

public class DialogueUIManager : MonoBehaviour
{
    //A reference to the text mesh pro component 
    [SerializeField]
    private TextMeshProUGUI textObject;
    //The prefab of the button that players press
    // to make decisions
    [SerializeField]
    private Transform buttonPrefab;
    //The area where buttons get spawned
    [SerializeField]
    private GameObject buttonArea;

    /// <summary>
    /// Call this function to display a message on the
    /// dialogue text box
    /// </summary>
    /// <param name="text">The text to be displayed</param>
    public void DisplayText(string text)
    {
        textObject.text = text;
    }

    /// <summary>
    /// Adds a choice to the text box with the given choice
    /// </summary>
    /// <param name="text"></param>
    /// <param name="choice"></param>
    public void AddChoice(string text, int choice)
    {
        //Create the button
        var newButton = Instantiate(buttonPrefab, buttonArea.transform);
        //Set it up so when clicked it calls the MakeChoice function
        newButton.GetComponent<Button>()
            .onClick.AddListener(()=>
            DialogueManager.instance.MakeChoice(choice));
        //Change the text of the button to match the choice being proposed
        newButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    //
    public void ClearChoices()
    {
        //This will get us an array with all the objects parents to the area
        // AND the area itself
        Button[] bttns = buttonArea.transform.
            GetComponentsInChildren<Button>();
        
        //Delete the objects under the button area from last to second
        //Since the 0th element is the parent
        for (int i = bttns.Length - 1; i >= 0; i--)
        {
            Destroy(bttns[i].gameObject);
        }
    }
}
