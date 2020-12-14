using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ranking Data: clase que nos va a ayudar a serializar los datos relacionados con el ranking de puntuacion
/// </summary>
public class RankingData
{
    public int sizeRankingData;
    public string[] nameRankingData;
    public int[] scoreRankingData;
    public int[] dateRankingData;

    public RankingData(string[] name, int[] score, int[] date, int sizeRanking)
    {
        sizeRankingData = sizeRanking;

        nameRankingData = new string[sizeRanking];
        scoreRankingData = new int[sizeRanking];
        dateRankingData = new int[sizeRanking];

        for(int i = 0; i < sizeRanking; i++)
        {
            nameRankingData[i] = name[i];
            scoreRankingData[i] = score[i];
            dateRankingData[i] = date[i];
        }
    }
}
