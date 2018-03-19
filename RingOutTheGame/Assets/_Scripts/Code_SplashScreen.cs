using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Code_SplashScreen : MonoBehaviour {

    [SerializeField] private Image logo;

    // Use this for initialization
    void Start () {
        // Play the SplashScreen function
        StartCoroutine(PlayAnimations());
    }

    /// <summary>
    /// The SplashScreen animation
    /// </summary>
    IEnumerator PlayAnimations(){
        // Play the logo animation
        logo.GetComponent<Animation>().Play();

        // Wait for the logo animation clips length
        yield return new WaitForSeconds(logo.GetComponent<Animation>().clip.length);

        // Start the MainMenu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
