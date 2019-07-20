using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimView : MonoBehaviour {
    //public Text elapsed;
    public Text[] populations;



    //public GameObject[] PopulationManagers;
    private PopulationManager[] PopulationManagerScripts;
    // Use this for initialization
    void Start () {
        GameObject[] PopulationManagers = GameObject.FindGameObjectsWithTag("PopulationManager");
        PopulationManagerScripts = new PopulationManager[PopulationManagers.Length];
        for(int i = 0; i < PopulationManagers.Length; i++)
        {
            PopulationManagerScripts[i] = PopulationManagers[i].GetComponent<PopulationManager>();
        }
        //StartCoroutine(updateDisplayData());
	}

    private IEnumerator updateDisplayData()
    {
        //elapsed.text = "elapsed : " + PopulationManager.elapsed;

        for(int i = 0; i < PopulationManagerScripts.Length; i++)
        {
            populations[i].text = " gen = " + PopulationManagerScripts[i].generation.ToString();
        }
        Debug.Log("?");
        yield return new WaitForSeconds(1f);
    }

    private void Update()
    {
        //elapsed.text = PopulationManager.elapsed.ToString();

        for (int i = 0; i < PopulationManagerScripts.Length; i++)
        {
            populations[i].text = +i+"_gen = " + PopulationManagerScripts[i].generation.ToString() + " t= " + PopulationManagerScripts[i].elapsed.ToString("F2");
        }
        //Debug.Log("?" + elapsed.text);
    }


}
