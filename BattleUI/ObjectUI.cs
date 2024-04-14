using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectUI : MonoBehaviour
{
	[SerializeField] protected Transform _container;
	
	[SerializeField] protected PortraitUI _portraitUI;
	
	protected abstract void Render(NullObject obj);
	
	protected virtual bool canBeOpenedByOtherPlayer => true;
	
	public void OnObjectUI(NullObject obj)
	{
		if (obj.player.id == CustomNetworkManager.localPlayerId && !obj.player.isAIPlayer || canBeOpenedByOtherPlayer)
		{
			_container.gameObject.SetActive(true);
		
			Render(obj);
		}
	}
	
	public virtual void OffObjectUI()
	{
		_container.gameObject.SetActive(false);
	}
}
