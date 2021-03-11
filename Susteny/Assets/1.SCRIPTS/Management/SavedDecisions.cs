using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavedDecisions : MonoBehaviour
{
    /// BOOLEANS - DEFINING PLAYER CHOICES (MIGHT BE SAVED TO BINARY IN LATER DEVELOPMENT) ///

    public bool annaDead;

    /// DODATKOWE FUNKCJE (SŁUŻĄCE NP. DO USTAWIANIA WARTOŚCI BOOLI PRZEZ ZEWNĘTRZNE SYSTEMY) ///

    public void setAnnaDead(bool b)
    {
        annaDead = b;
    }
}
