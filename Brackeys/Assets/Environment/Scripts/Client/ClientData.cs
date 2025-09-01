using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;

[Serializable]
public class ClientData
{
    public GenreInfo[] genreInfo;
    public DialogueLevelChances[] dialogueChances;
    public int tasteCode;
    // 0 Sweet
    // 1 Spicy
    // 2 Salty
}

[Serializable]
public class GenreInfo
{
    public string[] clientName;
    public ClientAnimations[] clientAnimations;
    public AudioClip[] voice;
}

[Serializable]
public class ClientAnimations
{
    public Animation idle;
    public Animation walking;
    public Animation happy;
}

[Serializable]
public class Answers
{
    public bool tasteQ;
    public bool ingredienQ;

    public FoodAnswares[] foodAnswares;
    public TasteAnswares tasteAnswers;
}

[Serializable]
public class Questions
{
    public string question;
    public bool subMenu;
    public bool tasteQ;
    public bool ingredientQ;

    public TasteQuestions[] tasteQuestions;
    public IngredientQuestions[] ingredientQuestions;
}

[Serializable]
public class IngredientQuestions
{
    public Food food;
}

[Serializable]
public class TasteQuestions
{
    public string question;
    public Tastes taste;
}

[Serializable]
public class FoodAnswares
{
    public string[] positiveAnswares;
}

[Serializable]
public class TasteAnswares
{
    public string[] answer;
    public string[] wrongTasteAnswers;
}

[Serializable]
public class DialogueLevelChances
{
    public int positiveChance;
    public int negativeChance;
}

[Serializable]
public class Tastes
{
    public int sweetness;
    public int spicy;
    public int salty;
}
