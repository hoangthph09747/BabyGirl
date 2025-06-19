using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiGuideHand : MonoBehaviour
{
    public static MultiGuideHand instance;

    [SerializeField] private GuideHand guideHandPrefab;

    private Dictionary<string, GuideHand> guideHands = new Dictionary<string, GuideHand>();

    private void Awake()
    {
        instance = this;
    }

    public void InitOneGuideHand(string id)
    {
        var guide = Instantiate(guideHandPrefab);
        guideHands[id] = guide;
    }

    public void RemoveGuideHand(string id)
    {
        Destroy(guideHands[id].gameObject);    
        guideHands.Remove(id);
    }

    public GuideHand GetGuideHand(string id)
    {
        if (!guideHands.ContainsKey(id))
            InitOneGuideHand(id);

        return guideHands[id];
    }

    public void DestrouAllGuideHands()
    {
        foreach (var guide in guideHands)
            Destroy(guide.Value.gameObject);
        guideHands.Clear();
    }

    private void OnDestroy()
    {
        instance = null;
    }
}
