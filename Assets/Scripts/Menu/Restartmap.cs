using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Restartmap : MonoBehaviour {


    public void restartmapp() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
