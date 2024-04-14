using UnityEngine;

public class Mine : Structure
{
	public bool isGoldMine { get; private set; }
	
	[SerializeField] private Sprite _oreMineIdleSprite;

	[SerializeField] private int _goldIncome;
	[SerializeField] private int _oreIncome;
	private int currentIncome;
	
	public override void StartTurn()
	{
		position.currentOreValue -= currentIncome;
		
		if (position.currentOreValue <= 0)
		{
			position.SetTerrainType(new Sand());
			Death();
		}
	}

	protected override void CurrentStructureInit(GroundCell positionCell)
	{
		SetMine(positionCell);
	}
	
	private void SetMine(GroundCell positionCell)
	{
		if (positionCell.terrainType == TerrainType.GoldDeposit)
		{
			isGoldMine = true;
			player.ChangeGoldIncome(_goldIncome);
			currentIncome = _goldIncome;
		}
		else
		{
			isGoldMine = false;
			player.ChangeOreIncome(_oreIncome);
			currentIncome = _oreIncome;
			_objectRenderer.spriteRenderer.sprite = _oreMineIdleSprite;
		}
	}
	
	public override void LocalDeath()
	{
		if (isGoldMine)
			player.ChangeGoldIncome(-_goldIncome);
		else
			player.ChangeOreIncome(-_oreIncome);
		
		currentHealth = 0;
		
		position.onCellObject = null;
		position = null;
	}
}
