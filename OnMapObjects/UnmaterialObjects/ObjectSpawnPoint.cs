using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawnPoint : UnmaterialObject
{
    public override bool startTurnWithDelay => true;

    [SerializeField] private List<NullObject> _objectList;

    private System.Random random = new System.Random();

    public override void StartTurn()
	{
		if (position.onCellObject == null && player.maxUnitLimit - player.currentUnitLimit >= _objectList[0].leadershipCost)
			SpawnObject();
	}

    private void SpawnObject()
    {
        NullObject obj = BattleMap.initObject.Invoke(_objectList[random.Next(_objectList.Count)], player, position);
    }
}
