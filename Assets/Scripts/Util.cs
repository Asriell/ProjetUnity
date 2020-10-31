//Classe "Foure-tout" utilitaire -> peut être appelée partout
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util
{
    //applique un layer à un objet et à l'ensemble de ses descendants.
    public static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if(obj == null)
        {
            return;
        }
        obj.layer = newLayer;
        foreach(Transform child in obj.transform)
        {
            if (child != null)
            {
                SetLayerRecursively(child.gameObject, newLayer);
            }
        }
    }
}
