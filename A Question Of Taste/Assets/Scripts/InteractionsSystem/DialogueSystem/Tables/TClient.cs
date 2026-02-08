using JetBrains.Annotations;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Scripting;

[Serializable]
public class TClient
{
    public ClientInfo[] clientInfo;
    public RuntimeAnimatorController[] skinAnimators;
    public Sprite[] skinIcons;
    public TClientGenreBodyComponent[] genreHairComponents;
    public DialogueLevelChances[] dialogueChances;
}

[Serializable]
public class ClientData
{
    [Header("Client Info")]
    public ClientInfo clientInfo;
    public TClientBodyComponent bodyComponents;
    public int dialogueLevel;

    [Header("Taste")]
    public TTaste clientTaste;

    [Header("State")]
    public bool waiting;
}

[Serializable]
public class ClientInfo
{
    public string clientName;
    public bool isMale;
    public AudioClip[] clientAudios;
}

[Serializable]
public class TClientGenreBodyComponent
{
    public TClientBodyComponentSprites[] bodyComponents;
}

[Serializable]
public class TClientBodyComponentSprites
{
    public Sprite[] sprites;
}

[Serializable]
public class TClientBodyComponent
{
    public ClientBodyComponent bodyComponent;
    public TClientBodyComponentSprites[] ComponentSprites;
}

[Serializable]
public class ClientAnimations
{
    public Animation idle;
    public Animation walking;
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
    public SFood food;
}

[Serializable]
public class TasteQuestions
{
    public string question;
    public TTaste clientTaste;
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

public enum ClientAnimationType
{
    Idle,
    Walk
}

public enum ClientBodyComponent
{
    Skin,
    Hair
}

