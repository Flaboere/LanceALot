using UnityEngine;
using System.Collections;

public class PrincessChamberScript : MonoBehaviour {


	public GameObject exceptedHorsePart;
	public GameObject expectedKnightPart;
	public GameObject lance;

	bool knightIn;
	bool horseIn;
	bool lanceIn;

	bool fired;
	void OnTriggerEnter2D(Collider2D other) {

		Debug.Log(other.gameObject);
		if(other.gameObject == exceptedHorsePart)
			horseIn = true;
		if(other.gameObject == expectedKnightPart)
			knightIn = true;
		if(other.gameObject == lance)
			lanceIn = true;

		if(!fired)
		{
			fired = true;
			StartCoroutine(EnterChamber());
		}
	}

	IEnumerator EnterChamber()
	{
		yield return new WaitForSeconds(2);

		if(horseIn && !knightIn)
			OnHorseIn();
		else if(!horseIn && knightIn)
			OnKnightIn();
		else if(horseIn && knightIn)
			OnThreesome();

		if(lanceIn)
			OnLanceIn();

		SendMessage ("Player", "End");
	}

	void OnHorseIn()
	{
		SoundController.Instance.OnHorseIn();
	}

	void OnKnightIn()
	{
		SoundController.Instance.OnKnightIn();
	}

	void OnThreesome()
	{
		SoundController.Instance.OnThreesome();
	}

	void OnLanceIn()
	{

	}
}
