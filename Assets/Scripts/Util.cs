//Classe "Foure-tout" utilitaire -> peut être appelée partout
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    public static UnityEngine.Vector3 CoordonneesPointSelonVecteurDirecteur(UnityEngine.Vector3 vecteurDirecteur,UnityEngine.Vector3 point,float taux)
    {
        return new UnityEngine.Vector3(
            vecteurDirecteur.x * taux + point.x,
            vecteurDirecteur.y * taux + point.y,
            vecteurDirecteur.z * taux + point.z
            );
    }
}
