using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogBox : MonoBehaviour
{
    // Start is called before the first frame update
    static GameObject DialogBoxPrefab;
    TMP_Text content;
    public float playDelay = 0.1f;
    [TextArea]
    public string defaultText;
    bool fastForwarded;
    void Awake()
    {
        content = GetComponentInChildren<TMP_Text>();
        content.text = defaultText;
    }
    void Start()
    {
        
    }

    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.T))
        // {
        //     ShowMessage(defaultText);
        // }
         fastForwarded |= FastForwardCheck();
    }
    public void Disappear()
    {
        Destroy(transform.parent.gameObject);
    }
    public IEnumerator ShowMessage(string msg, bool tapToContinue = true, bool screenGamePlay = true, bool animated = true)
    {
        if (screenGamePlay)
        {
            Utils.SetAllKeys(false);
        }

        fastForwarded = false;
        if (!animated) {
            content.text = msg;
        }
        else {
            int length = msg.Length;
            for (int i = 1; i <= length; i++)
            {
                if (fastForwarded) i = length;
                content.text = msg.Substring(0, i);
                yield return new WaitForSeconds(playDelay);
            }
        }
        if (tapToContinue)
        {
            while (true)
            {
                if (Input.GetKeyDown(KeyCode.Space)) break;
                yield return null;
            }
        }
        if (screenGamePlay)
        {
            Utils.SetAllKeys(true);
        }
    }
    bool FastForwardCheck()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return)) return true;
        return false;
    }
    static public DialogBox CreateDialogBox(Vector2 pos, Vector2 tailAnchor = default(Vector2))
    {
        if (DialogBoxPrefab == null) DialogBoxPrefab = Resources.Load<GameObject>("Prefabs/UI/DialogBox");
        DialogBox newDialogBox = Instantiate(DialogBoxPrefab).GetComponentInChildren<DialogBox>();
        RectTransform rect = newDialogBox.GetComponent<RectTransform>();
        rect.anchoredPosition = pos;
        rect.pivot = tailAnchor;
        return newDialogBox;
    }
}
