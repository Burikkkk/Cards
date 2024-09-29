using Beardy;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public int cardsAmount = 21;
    [SerializeField]
    private int pairsAmount = 9;
    [SerializeField]
    private GameObject card;
    [SerializeField]
    private Sprite questionSprite;
    [SerializeField]
    private Sprite backSprite;
    [SerializeField]
    private Sprite[] pairedSprites;
    [SerializeField]
    private Sprite[] unpairedSprites;
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private TMP_Text winnerText;
    [SerializeField]
    private TMP_Text player1Text;
    [SerializeField]
    private TMP_Text player2Text;
    [SerializeField]
    private Image player1Image;
    [SerializeField]
    private Image player2Image;
    [SerializeField]
    private Color shadedColor;
    [SerializeField]
    private GameObject lastButton;

    private static Player player1;
    private static Player player2;
    public static Player currentPlayer;

    private List<Sprite> selectedSprites;
    private int unpairedAmount;

    private void Start()
    {
        player1 = new Player();
        player1.SetScoreText(player1Text);
        currentPlayer = player1;
        player2 = new Player();
        player2.SetScoreText(player2Text);

        selectedSprites = new List<Sprite>();

        unpairedAmount = cardsAmount - pairsAmount * 2;
        Card.SetBack(backSprite);

        SelectPairedCards();
        AddQuestionCards();

        SetCards(); 
        Invoke("DisableGrid", 0.2f);
    }

    private void Update()
    {
        if(cardsAmount == unpairedAmount)
        {
            EndGame();
        }
    }
    //private void 

    public void ChangePlayer()
    {
        if(currentPlayer == player1)
        {
            currentPlayer = player2;
            player1Image.color = shadedColor;
            player2Image.color = Color.white;
            return;
        }
        currentPlayer = player1;
        player2Image.color = shadedColor;
        player1Image.color = Color.white;
    }

    private void DisableGrid()
    {
        canvas.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = false;
    }

    private void EnableGrid()
    {
        canvas.GetComponent<UnityEngine.UI.GridLayoutGroup>().enabled = true;
    }

    private void SetCards()
    {
        var indices = GetIndices(cardsAmount);
        for (int i = 0; i < cardsAmount; i++)
        {
            GameObject newCard = Instantiate(card, canvas.transform);
            Card cardComponent = newCard.GetComponent<Card>();

            int randomIndex = Random.Range(0, indices.Count);
            int spriteIndex = indices[randomIndex], id = spriteIndex / 2;

            Sprite selectedSprite = selectedSprites[spriteIndex];

            if (selectedSprite == questionSprite)
                id = -1;

            cardComponent.SetCard(id, selectedSprite, this);
            indices.RemoveAt(randomIndex);

        }
    }

    private void AddQuestionCards()
    {
        for (int i = 0; i < unpairedAmount; i++)
        {
            selectedSprites.Add(questionSprite);
        }
    }

    private void SelectPairedCards() //список выбранных парных карт
    {
        var indices = GetIndices(pairedSprites.Length / 2);
        SetRandomPairs(indices);
    }

    private List<int> GetIndices(int len) //список с индексами
    {
        List<int> indices = new List<int>();

        for (int i = 0; i < len; i++)
        {
            indices.Add(i);
        }
        return indices;
    }

    private void SetRandomPairs(List<int> indices)//список с парными картами
    {
        for (int i = 0; i < pairsAmount; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            int spriteIndex1 = indices[randomIndex] * 2, spriteIndex2 = indices[randomIndex] * 2 + 1;

            selectedSprites.Add(pairedSprites[spriteIndex1]);
            selectedSprites.Add(pairedSprites[spriteIndex2]);

            indices.RemoveAt(randomIndex);
        }
    }

    public void EndGame()
    {
        Invoke("EnableGrid", 1.7f);
        Invoke("SetUnpairedCards", 1.7f);
        cardsAmount = -1;
        Invoke("SetWinnerText", 1.7f);
        Card.SetCheckMatch(false);
        Invoke("EnableLastButton", 4.7f);
    }

    public void SetWinnerText()
    {
        winnerText.enabled = true;
        if(player1.Score > player2.Score)
        {
            winnerText.text += "Игрок 1!";
        }
        else
        {
            winnerText.text += "Игрок 2!";
        }
    }

    public void EnableLastButton()
    {
        lastButton.SetActive(true);
    }

    public void SetUnpairedCards()
    {
        var cards = GetRemainingCards();
        var indices = GetIndices(unpairedSprites.Length);
        for (int i = 0; i < cards.Count; i++)
        {
            int randomIndex = Random.Range(0, indices.Count);
            int spriteIndex = indices[randomIndex];

            cards[i].SetFront(unpairedSprites[spriteIndex]);

            indices.RemoveAt(randomIndex);
        }
    }

    public List<Card> GetRemainingCards()
    {
        List<Card> cards = new List<Card>();
        foreach (Transform child in canvas.transform)
        {
            cards.Add(child.gameObject.GetComponent<Card>());
        }
        return cards;
    }
}
