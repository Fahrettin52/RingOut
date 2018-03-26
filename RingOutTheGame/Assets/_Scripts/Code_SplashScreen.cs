using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Code_SplashScreen : MonoBehaviour {

    [SerializeField] private Image logo; // The image that contains the animation

    // Use this for initialization
    void Start() {
        // Call the SplashScreen methode
        StartCoroutine(PlayAnimations());
    }

    /// <summary>
    /// The SplashScreen animation
    /// </summary>
    IEnumerator PlayAnimations() {
        // Store the Animation component in a temporarily variable for better performance
        Animation animation = logo.GetComponent<Animation>();
        // Play the Logo animation
        animation.Play();
        // Wait for the Logo animation clips length
        yield return new WaitForSeconds(animation.clip.length);
        // Start the MainMenu
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
