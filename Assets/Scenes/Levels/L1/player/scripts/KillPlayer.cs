using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
		public int respawn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

		void OnTriggerEnter2D(Collider2D other) {
			// Reload the scene when you touch a spike
			// TODO: make this throw you back to a checkpoint if it exists
			if (other.CompareTag("Player")) {
				SceneManager.LoadScene(respawn);
			}
		}
}
