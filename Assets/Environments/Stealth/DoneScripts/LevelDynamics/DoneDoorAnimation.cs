using UnityEngine;
using System.Collections;
using System.Linq;

public class DoneDoorAnimation : MonoBehaviour
{
    public bool requireKey;                         // Whether or not a key is required.
    public AudioClip doorSwishClip;                 // Clip to play when the doors open or close.
    public AudioClip accessDeniedClip;				// Clip to play when the player doesn't have the key for the door.
    public GameObject[] RequiredKeys;                  //the key gameobjects required to open the door
	
	
	private Animator anim;							// Reference to the animator component.
	private DoneHashIDs hash;						// Reference to the HashIDs script.
	private GameObject player;						// Reference to the player GameObject.
	private DonePlayerInventory playerInventory;	// Reference to the PlayerInventory script.
	private int count;								// The number of colliders present that should open the doors.
	
	
	void Awake ()
	{
		// Setting up the references.
		anim = GetComponent<Animator>();
		hash = GameObject.FindGameObjectWithTag(DoneTags.gameController).GetComponent<DoneHashIDs>();
		player = GameObject.FindGameObjectWithTag(DoneTags.player);
		playerInventory = player.GetComponent<DonePlayerInventory>();
	}
	
	
	void OnTriggerEnter (Collider other)
	{
		// If the triggering gameobject is the player...
		if(other.transform.root.gameObject == player)
		{
			// ... if this door requires a key...
			if(requireKey || (RequiredKeys != null && RequiredKeys.Length > 0))
			{
				// ... if the player has the key...
				if(requireKey && playerInventory.hasKey)
					// ... increase the count of triggering objects.
					count++;
                else if(playerInventory.KeyCards != null && playerInventory.KeyCards.Length > 0 && RequiredKeys != null && RequiredKeys.ToList().Where(x => playerInventory.KeyCards.Contains(x)).Any())
                {
                    //the player has the required key card
                    count++;
                }
				else
				{
					// If the player doesn't have the key play the access denied audio clip.
					GetComponent<AudioSource>().clip = accessDeniedClip;
					GetComponent<AudioSource>().Play();
				}
			}
			else
				// If the door doesn't require a key, increase the count of triggering objects.
				count++;
		}
		// If the triggering gameobject is an enemy...
		else if(other.gameObject.tag == DoneTags.enemy)
		{
			// ... if the triggering collider is a capsule collider...
			if(other is CapsuleCollider)
				// ... increase the count of triggering objects.
				count++;
		}
	}
	
	
	void OnTriggerExit (Collider other)
	{
		// If the leaving gameobject is the player or an enemy and the collider is a capsule collider...
		if(other.transform.root.gameObject == player || (other.gameObject.tag == DoneTags.enemy && other is CapsuleCollider))
			// decrease the count of triggering objects.
			count = Mathf.Max(0, count-1);
	}
	
	
	void Update ()
	{
		// Set the open parameter.
		anim.SetBool(hash.openBool,count > 0);
		
		// If the door is opening or closing...
		if(anim.IsInTransition(0) && !GetComponent<AudioSource>().isPlaying)
		{
			// ... play the door swish audio clip.
			GetComponent<AudioSource>().clip = doorSwishClip;
			GetComponent<AudioSource>().Play();
		}
	}
}
