using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManager : MonoBehaviour
{
    // Start is called before the first frame update
    public enum Alignment{
        Left,
        Center,
        Right
    }
    [SerializeField]
    GameObject cardPrefab;
    public bool interactable;
    public Alignment alignment;
    public float maxWidth;
    float baseWidth;
    public float cardInterval;
    public float minInterval;
    protected List<Card> cards;
    Card selectedCard;
    [SerializeField]
    SmoothObject selectIcon;
    SmoothObject motion;
    virtual protected void Awake()
    {
        motion = GetComponent<SmoothObject>();
    }
    virtual protected void Start()
    {
        if (selectIcon != null) selectIcon.gameObject.SetActive(false);
        cards = new List<Card>();
        selectedCard = null;
        baseWidth = maxWidth;
        if (minInterval < 0.01f) minInterval = 1.2f;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        UserInput();
        UpdateSelectIcon();
    }
    virtual protected void UserInput()
    {
        if (!interactable) return;
        int selectDelta = 0;
        if (Input.GetKeyDown(KeyCode.LeftArrow)) selectDelta--;
        if (Input.GetKeyDown(KeyCode.RightArrow)) selectDelta++;
        
        int select = GetSelectedIndex();
        if (selectDelta != 0 && inBoundary(select + selectDelta))
        {
            select += selectDelta;
            ReSelect(cards[select]);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            cards[select].Hit();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            AddCard(Random.Range(0, cards.Count));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            DelCard(Random.Range(0, cards.Count));
        }
    }
    void UpdateSelectIcon()
    {
        if (!interactable) return;
        if (cards.Count == 0)
        {
            selectIcon.gameObject.SetActive(false);
        }
        else
        {
            selectIcon.gameObject.SetActive(true);
            selectIcon.targetPos = selectedCard.transform.localPosition;  
        }
    }
    int GetSelectedIndex()
    {
        if (selectedCard == null) return -1;
        return cards.IndexOf(selectedCard);
    }
    bool inBoundary(int pos)
    {
        return (pos >= 0 && pos < cards.Count);
    }
    protected Card AddCard(int pos)
    {
        Card newCard = Instantiate(cardPrefab, this.transform).GetComponent<Card>();
        return AddCard(pos, newCard);
    }
    protected Card AddCard(int pos, Card newCard)
    {
        cards.Insert(pos, newCard);
        UpdatePos();
        if (cards.Count == 1) ReSelect(cards[0]);
        return newCard;
    }
    protected void DelCard(int pos)
    {
        if (cards[pos] == selectedCard) {
            selectedCard = null;
            if (pos > 0) ReSelect(cards[pos - 1]);
            else if (pos + 1 < cards.Count) ReSelect(cards[pos + 1]);
        }
        cards[pos].SelfDestroy();
        cards.RemoveAt(pos);
        UpdatePos();
    }
    void ReSelect(Card newCard)
    {
        if (!interactable) return;
        if (selectedCard != null) selectedCard.Deselect();
        selectedCard = newCard;
        if (selectedCard != null) selectedCard.Select();
    }
    void UpdatePos()
    {
        float pos, interval;
        
        if ((cards.Count - 1) * cardInterval > maxWidth) interval = maxWidth / (cards.Count - 1);
        else interval = cardInterval;
        //Debug.Log("interval = " + interval);
        if (interval < minInterval)
        {
            motion.targetSize = new Vector2(interval / minInterval, interval / minInterval);
            interval = minInterval;
          //  Debug.Log("motion.targetSize = " + motion.targetSize);
        }
        else motion.targetSize = new Vector2(1, 1);

        if (alignment == Alignment.Center)
        {
            if (cards.Count % 2 == 0) pos = - (cards.Count / 2 - 0.5f) * interval;
            else pos = - (cards.Count / 2) * interval;
        }
        else if (alignment == Alignment.Right) pos = - cards.Count * interval;
        else pos = 0;

        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].targetPos = new Vector2(pos, 0);
            pos += interval;
        }

    }
}
