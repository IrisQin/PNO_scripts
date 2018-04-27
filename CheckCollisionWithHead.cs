using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCollisionWithHead : MonoBehaviour {

    GameObject coffee;
    GameObject donut;
    GameObject cigarette;
    DonutController donutController;
    SphereCollider head;
    float volume = 0;
    int smokingStatus = 0;

    [FMODUnity.EventRef]
    public string coffeeSFX;

    [FMODUnity.EventRef]
    public string donutSFX;

    [FMODUnity.EventRef]
    public string inhaleCigSFX;

    [FMODUnity.EventRef]
    public string exhaleCigSFX;

    // Use this for initialization
    void Start () {
        coffee = GameObject.FindGameObjectWithTag("Coffee");
        donut = GameObject.FindGameObjectWithTag("Donut");
        cigarette = GameObject.FindGameObjectWithTag("Cigarette");
        donutController = donut.GetComponent<DonutController>();
        head = GetComponent<SphereCollider>();
        StartCoroutine(activateCollider());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == cigarette)
        {
            cigarette.transform.Find("emberprefab").gameObject.SetActive(false);
            cigarette.transform.Find("emberprefab (1)").gameObject.SetActive(true);
            FMODUnity.RuntimeManager.PlayOneShot(inhaleCigSFX, cigarette.transform.position);
        }

        if (other.gameObject == coffee)
        {
            FMODUnity.RuntimeManager.PlayOneShot(coffeeSFX, coffee.transform.position);
            Debug.Log("coffee entered");
        }

        if (other.gameObject == donut)
        {
            FMODUnity.RuntimeManager.PlayOneShot(donutSFX, donut.transform.position);
            donutController.eatDonut();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        /*if (other.gameObject == cigarette)
        {
            volume = GameObject.FindGameObjectWithTag("blowprefab").GetComponent<detectVolume>().volume;
            float minPar = 10f;
            float maxPar = 300f;
            float minParSize = 0.5f;
            float maxParSize = 3.2f;
            float maxVol = 0.08f;
            float minVol = 0.03f;
            ParticleSystem par = transform.Find("blowOprefab").gameObject.GetComponent<ParticleSystem>();
            //ParticleSystem par2 = transform.Find("blowOprefab").Find("BlowO2").gameObject.GetComponent<ParticleSystem>();
            if (volume > minVol && smokingStatus ==0)
            {
                smokingStatus = 1;
                Debug.Log(volume);
                cigarette.transform.Find("emberprefab").gameObject.SetActive(true);
                cigarette.transform.Find("emberprefab (1)").gameObject.SetActive(false);
                par.Play();
                var emission = par.emission;
                emission.rateOverTime = minPar + (volume - minVol) * (maxPar - minPar) / (maxVol - minVol);
                //ParticleSystem.MainModule main = par2.main;
                //main.startSizeX = minParSize + (volume - minVol) * (maxParSize - minParSize) / (maxVol - minVol);
                //main.startSizeY = minParSize + (volume - minVol) * (maxParSize - minParSize) / (maxVol - minVol);
                //main.startSizeZ = minParSize + (volume - minVol) * (maxParSize - minParSize) / (maxVol - minVol);
                //par2.Play();
                FMODUnity.RuntimeManager.PlayOneShot(exhaleCigSFX, transform.position);
            }
            if (!par.isPlaying) {
                smokingStatus = 0;
            }
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == cigarette)
        {
            cigarette.transform.Find("emberprefab").gameObject.SetActive(true);
            cigarette.transform.Find("emberprefab (1)").gameObject.SetActive(false);
            transform.Find("blowOprefab").gameObject.GetComponent<ParticleSystem>().Play();
            FMODUnity.RuntimeManager.PlayOneShot(exhaleCigSFX, transform.position);
        }
    }

    IEnumerator activateCollider()
    {
        yield return new WaitForSeconds(1f);
        head.enabled = true;
    }
}
