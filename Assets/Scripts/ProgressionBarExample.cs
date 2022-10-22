using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// För att använda UI-delar i Unity måste vi inkludera UnityEngine.UI
using UnityEngine.UI;

public class ProgressionBarExample : GameManager
{
    // Dessa variabler är referenser till RectTransforms (position och storlek för UI element). Just dessa två
    // motsvarar då bakgrunden för "progression" och den andra är punkten som förflyttar sig över bakgrunden.
    // SerializeField gör då variablen synlig i Unity Editor.
    [SerializeField] RectTransform backgroundBar;
    [SerializeField] RectTransform currentProgression;

    // Denna raden gör variablen synlig i editor (SerializeField) och Range säger vilka värden vi godkänner 
    // Variablen är mellan. I detta läget så kan progressionValue bara vara mellan 0 och 100.
    [SerializeField][Range(0, 100)] int progressionValue;

    void Update()
    {
        // Omvandla progressionen till 0-1 (att multiplicera med 0 innebär längst till vänster, 1 längst till höger)
        float progressAsDecimal = progressionValue / 100f;
        if (procent > 0)
        progressAsDecimal = procent % 1;

        float position = (backgroundBar.rect.width - currentProgression.rect.width ) * progressAsDecimal;

        // Denna metoden förflyttar currentProgression (den röda kuben) position är förflyttningen längs progression-baren.
        currentProgression.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, position, currentProgression.rect.width);
    }
}
