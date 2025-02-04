using System.Collections.Generic;
using UnityEngine;

public class SpellSpawnerScript : MonoBehaviour
{
    public GameObject Spell;
    public double spawnRate = 5;
    private float _timer = 0;
    private static bool _running = true;

    // Start is called before the first frame update
    void Start()
    {
        SpawnSpell();
    }

    // Update is called once per frame
    void Update()
    {
        if (_running)
        {
            if (_timer < spawnRate)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                SpawnSpell();
                _timer = 0;
            }
        }
    }

    void SpawnSpell()
    {
        int randomIndexOf4 = Random.Range(0, 4);
        int randomIndexOf8 = Random.Range(0, 7);

        

        //var spell = Instantiate(Spell, new Vector3(transform.position.x, _numbers[randomIndexOf4], 0), transform.rotation);
        var spell = Instantiate(Spell, new Vector3(transform.position.x, GameObject.Find($"Pointer{randomIndexOf4 + 1}").transform.position.y, 0), transform.rotation);
        spell.GetComponent<SpellMoveScript>().Fret = randomIndexOf4 + 1;
        spell.GetComponent<SpellMoveScript>().sprites[randomIndexOf8].SetActive(true);
    }

    public void Stop()
    {
        _running = false;
    }
}