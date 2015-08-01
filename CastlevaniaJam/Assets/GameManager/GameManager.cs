using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour 
{
    public static GameManager Instance;    
    public GameObject Player;

	void Start () 
    {
        Instance = this;
    }
	
	
	void Update () 
    {
	
	}
}
