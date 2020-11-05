using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataTranslator
{

    private static string KILLS_SYMBOL = "[KILLS]";
    private static string DEATHS_SYMBOL = "[DEATHS]";

    public static string ValuesToData(int kills, int deaths)
    {
        return KILLS_SYMBOL + kills + "/" + DEATHS_SYMBOL + deaths;
    }
    public static int DataToKills(string data)
    {
        return Int32.Parse(DataToValue(data,KILLS_SYMBOL));
    }

    public static int DataToDeaths(string data)
    {
        return Int32.Parse(DataToValue(data, DEATHS_SYMBOL));
    }

    public static string DataToValue(string data, string symbol)
    {
        string[] pieces = data.Split('/');
        foreach (string piece in pieces)
        {
            if (piece.StartsWith(symbol))
            {
                return piece.Substring(symbol.Length);
            }
        }
        Debug.LogError(symbol + "not found in " + data);
        return "";
    } 
}
