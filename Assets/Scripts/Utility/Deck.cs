using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Deck<T> 
{
    public List<T> cards = null;

    public RandomSeed r = null;

    private List<T> addedCards = null;

    public string name = "";

    public int Count
    {
        get
        {
            return cards.Count;
        }//get
    }//Count

    public Deck(string name, RandomSeed rand)
    {
        r = rand;
        this.name = name;
        addedCards = new List<T>();
        cards = new List<T>();
    }//Constructor

    public void Add(T card)
    {
        addedCards.Add(card);
        cards.Add(card);
    }//addNewCard

    public void Shuffle()
    {
        cards = null;
        cards = new List<T>();
        foreach(T card in addedCards)
        {
            cards.Add(card);
        }//foreach
    }//reShufflePile
    
    public T Draw()
    {
        if(cards.Count <= 0)
            Shuffle();
        
        //Pick a card, any card
        int cardNum = r.getIntInRange(0, cards.Count-1);
        
        //You pulled _card_
        T result = cards[cardNum];
        
        //Remove the card from the deck
        cards.RemoveAt(cardNum);
        
        return result;
    }//pullCardFrom

    
    public override string ToString ()
    {
        string str = "-- "+ name + " --\n";
        for(int i=0; i < Count; i++)
        {
            str += cards[i].ToString() + ", " ;
        }//for
        return str;
    }//ToString

    public bool isEmpty
    {
        get { return cards.Count <= 0; }
    }



}//Deck