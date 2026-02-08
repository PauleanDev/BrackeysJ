using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class starImages
{
    public Image[] _levelButtonImages;
}

public class LevelButtons : MonoBehaviour
{
    [SerializeField] starImages[] _levelButtonImages;
    [SerializeField] Sprite _litStar;

    void Awake()
    {
        for (int i = 0; i < _levelButtonImages.Length; i++)
        {
            int _levelStars;
            _levelStars = PlayerPrefs.GetInt("Level" + i.ToString() + "BestStar");

            if (_levelStars > 0 && _levelStars <= 3)
            {
                for (int j = 1; j <= _levelStars; j++)
                {
                    Debug.Log(_levelButtonImages[i]._levelButtonImages[j - 1].name);
                    _levelButtonImages[i]._levelButtonImages[j-1].sprite = _litStar;
                }
            }
        }
    }

}
