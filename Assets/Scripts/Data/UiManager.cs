using System;
using UnityEngine;
using System;

public class UiManager : MonoBehaviour
{

    public GameObject zombieUi;

    public static event EventHandler zombieTime;

    public static void DoSomethingStupid()
    {
        zombieTime?.Invoke(null, System.EventArgs.Empty);
    }


    public void Awake()
    {
        zombieUi.SetActive(false);

        zombieTime += ExecuteZombieTime;
    }

    private void ExecuteZombieTime(object sender, EventArgs e)
    {
        zombieUi.SetActive(true);
    }

}
