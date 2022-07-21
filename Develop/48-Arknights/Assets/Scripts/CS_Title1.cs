using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CS_Title1 : MonoBehaviour {
    public void OnButtonStart () {
        SceneManager.LoadScene ("StageSelection");
    }
}
