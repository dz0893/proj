using UnityEngine;
using System.Collections;

public abstract class MapEvent : ScriptableObject
{
	[SerializeField] private MapEvent _nextEvent;
	[SerializeField] private int _playerIndex;
	[SerializeField] private int _cameraPositionCellIndex;
	[SerializeField] private float _delayTime;
	[SerializeField] private float _eventTime;

	public int turnOfActivation { get; private set; }
	
	protected virtual bool momentalEvent => true;
	
	public Player player => BattleMap.instance.playerList[_playerIndex];
	
	public void TryToActivateEvent(int turnOfActivation)
	{
		if (TurnController.currentPlayer == player && (!ActionController.controllerInAction || TurnController.currentPlayer.isAIPlayer))
			Scenario.activateMapEvent.Invoke(this, turnOfActivation);
		else
			AddEventToRow();
	}
	
	private void AddEventToRow()
	{
		player.AddEventToRow(this);
	}
	
	public IEnumerator ActivateEvent(int turnOfActivation)
	{
		SetCameraPosition();
		PlayerUI.lockInput.Invoke();

		if (_delayTime > 0)
			yield return new WaitForSeconds(_delayTime);

		this.turnOfActivation = turnOfActivation;
		
		CurrentEventActivate();
		
		if (_eventTime > 0)
			yield return new WaitForSeconds(_eventTime);

		if (momentalEvent)
			EndEvent();
		
		yield return null;
	}
	
	public void EndEvent()
	{
		PlayerUI.unlockInput.Invoke();
		MiniMap.render.Invoke();
		
		if (_nextEvent != null)
			_nextEvent.TryToActivateEvent(turnOfActivation);
	}
	
	protected virtual void SetCameraPosition()
	{
		GroundCell cameraPositionCell = null;

		if (_cameraPositionCellIndex != 0)
			cameraPositionCell = BattleMap.instance.GetCellWithIndex(_cameraPositionCellIndex);

		if (cameraPositionCell != null)
			CameraController.setCameraPosition.Invoke(cameraPositionCell.transform.position);
	}

	public abstract void CurrentEventActivate();
	
	public void PlayEventRowOnLoad(EventRowObjectSaveInfo objectSaveInfo)
	{
		turnOfActivation = objectSaveInfo.turnOfActivation;
		PlayOnLoad(objectSaveInfo);
		player.RemoveEventFromRow(this);
		_nextEvent?.PlayEventRowOnLoad(objectSaveInfo);
	}
	
	protected virtual void PlayOnLoad(EventRowObjectSaveInfo objectSaveInfo) {}
}
