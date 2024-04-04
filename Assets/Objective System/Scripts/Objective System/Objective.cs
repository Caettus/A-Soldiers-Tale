using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Objective : MonoBehaviour
{
    [SerializeField] AudioSource objSFX;
    public GameObject theTrigger;
    public GameObject theObjective;
    public GameObject theText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(missionObj());
    }


    private IEnumerator missionObj()
    {
        objSFX.Play();
        theObjective.SetActive(true);
        theObjective.GetComponent<Animation>().Play("ObjectiveDisplayAnim");
        theText.GetComponent<Text>().text = "Objective: Clear the enemy's trench!";
        yield return new WaitForSeconds(5.3f);
        theText.GetComponent<Text>().text = "";
        theTrigger.SetActive(false);
        theObjective.SetActive(false);
    }
}