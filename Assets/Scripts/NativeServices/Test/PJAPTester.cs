using UnityEngine;
using System.Collections;
using NativeService;

public class PJAPTester : MonoBehaviour 
{

	// Use this for initialization
	void Start () {
    PJIAP.Instance.InitializePurchasing ();
	}
	
  public void BuyConsumable()
  {
    PJIAP.Instance.BuyConsumable ();
  }
}
