using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PopulationManager : MonoBehaviour {
    public GameObject birdPref;
    public GameObject startingPos;
    public int populationSize = 50;
    List<GameObject> population = new List<GameObject>();
    //public static float elapsed = 0;
    public float elapsed = 0;
    public float trialTime = 3;
    public int generation = 1;
    [Range(0, 100)]
    public float mutationPerc = 5;
    [Range(100, 1)]
    public float weightOfCrash = 100f;

    private void Start()
    {
        for(int i = 0; i < populationSize; i++)
        {
            GameObject newBird = Instantiate(birdPref, startingPos.transform.position, this.transform.rotation);
            newBird.GetComponent<Brain>().Init();
            population.Add(newBird);
        }
    }

    GameObject Breed(GameObject parent1, GameObject parent2)
    {
        GameObject newBird = Instantiate(birdPref, startingPos.transform.position, this.transform.rotation);
        Brain newBrain = newBird.GetComponent<Brain>();

        newBrain.Init();

        if (Random.Range(0,100) <= mutationPerc)
        {
            newBrain.dna.Mutate();
        }
        else
        {
            newBrain.dna.Combine(parent1.GetComponent<Brain>().dna, parent2.GetComponent<Brain>().dna);
        }
        return newBird;
    }

    void BreedNewPopulation()
    {
        List<GameObject> sortedList = population.OrderBy(o => (o.GetComponent<Brain>().distanceTraveled - o.GetComponent<Brain>().crash*weightOfCrash*0.1)).ToList();

        population.Clear();

        for (int i = (int)(3*sortedList.Count/4.0f) - 1; i <sortedList.Count - 1; i++)
        {
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i+1], sortedList[i]));
            population.Add(Breed(sortedList[i], sortedList[i + 1]));
            population.Add(Breed(sortedList[i + 1], sortedList[i]));
        }
        for (int i = 0; i<sortedList.Count; i++)
        {
            Destroy(sortedList[i]);
        }
        generation++;
    }

    private void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed >= trialTime)
        {
            BreedNewPopulation();
            elapsed = 0;
        }
    }
}
