using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    // Start is called before the first frame update

    public LevelTemplate loadedLevelTemplate;
    public GameObject CMBounding;
    public GameObject CM;
    public GameObject player;
    public GameObject respawner;

    public float shrink = 1f;

    private void Awake()
    {
        loadedLevelTemplate = Static.levelTemplate;
        Vector2[] allpts;
        allpts = new Vector2[4];
        allpts[0] = new Vector2(loadedLevelTemplate.BoundingXY1.x + shrink, loadedLevelTemplate.BoundingXY1.y - shrink);
        allpts[1] = new Vector2(loadedLevelTemplate.BoundingXY1.x + shrink, loadedLevelTemplate.BoundingXY2.y + shrink);
        allpts[2] = new Vector2(loadedLevelTemplate.BoundingXY2.x - shrink, loadedLevelTemplate.BoundingXY2.y + shrink);
        allpts[3] = new Vector2(loadedLevelTemplate.BoundingXY2.x - shrink, loadedLevelTemplate.BoundingXY1.y - shrink);
        player.transform.position = loadedLevelTemplate.startingPosition;
        Debug.Log(loadedLevelTemplate.startingPosition);

        CMBounding.GetComponent<PolygonCollider2D>().pathCount = 1;
        CMBounding.GetComponent<PolygonCollider2D>().enabled = false;
        CMBounding.GetComponent<PolygonCollider2D>().SetPath(0, allpts);
        CMBounding.GetComponent<PolygonCollider2D>().enabled = true;

        if(loadedLevelTemplate.lockCamera)
        {
            CM.SetActive(false);
        }
        else
        {
            CM.SetActive(true);
        }
        GameObject terrain = Instantiate(loadedLevelTemplate.levelTerrain, transform);
       // player.GetComponent<Player>().currentLevelData = loadedLevelTemplate;

    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
