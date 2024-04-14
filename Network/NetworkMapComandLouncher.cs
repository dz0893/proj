using UnityEngine;
using Mirror;

public class NetworkMapComandLouncher : NetworkBehaviour
{
    public void SkipTurn()
    {
        if (isServer)
        {
            RpcEndTurn();
        }
        else
            CmdEndTurn();
    }

    [ClientRpc]
    private void RpcEndTurn()
    {
        TurnStateUI.rpcSkipTurn.Invoke();
    }

    [Command]
    private void CmdEndTurn()
    {
        RpcEndTurn();
    }

    public void RecruitUnit(int cellIndex, int playerId, int unitDataIndex)
    {
        if (isServer)
            RpcRecruitUnit(cellIndex, playerId, unitDataIndex);
        else
            CmdRecruitUnit(cellIndex, playerId, unitDataIndex);
    }

    [ClientRpc]
    private void RpcRecruitUnit(int cellIndex, int playerId, int unitDataIndex)
    {
        RecruitUnitCell.recruitUnitInCellWithCurrentPosition.Invoke(cellIndex, playerId, unitDataIndex);
    }

    [Command]
    private void CmdRecruitUnit(int cellIndex, int playerId, int unitDataIndex)
    {
        RpcRecruitUnit(cellIndex, playerId, unitDataIndex);
    }

    public void ResearchUpgrade(int playerId, int upgradeIndex)
    {
        if (isServer)
            RcpResearchUpgrade(playerId, upgradeIndex);
        else
            CmdResearchUpgrade(playerId, upgradeIndex);
    }

    [ClientRpc]
    private void RcpResearchUpgrade(int playerId, int upgradeIndex)
    {
        UpgradeCell.onlineResearchUpgrade(playerId, upgradeIndex);
    }

    [Command]
    private void CmdResearchUpgrade(int playerId, int upgradeIndex)
    {
        RcpResearchUpgrade(playerId, upgradeIndex);
    }

    public void ReviveHero(int cellIndex, int playerId)
    {
        if (isServer)
            RpcReviveHero(cellIndex, playerId);
        else
            CmdReviveHero(cellIndex, playerId);
    }

    [ClientRpc]
    private void RpcReviveHero(int cellIndex, int playerId)
    {
        RecruitPointUI.onlineReviveHero.Invoke(cellIndex, playerId);
    }

    [Command]
    private void CmdReviveHero(int cellIndex, int playerId)
    {
        RpcReviveHero(cellIndex, playerId);
    }

    public void MakeAction(int targetIndex, int unitPositionIndex, int unitActionIndex)
    {
        if (isServer)
        {
            RpcMakeAction(targetIndex, unitPositionIndex, unitActionIndex);
        }
        else
        {
            CmdMakeAction(targetIndex, unitPositionIndex, unitActionIndex);
        }
    }

    [ClientRpc]
    private void RpcMakeAction(int targetIndex, int unitPositionIndex, int unitActionIndex)
    {
        MapController.onlineAction.Invoke(targetIndex, unitPositionIndex, unitActionIndex);
    }

    [Command]
    private void CmdMakeAction(int targetIndex, int unitPositionIndex, int unitActionIndex)
    {
        RpcMakeAction(targetIndex, unitPositionIndex, unitActionIndex);
    }




    public void GlobalAction(int targetIndex, int actionIndex)
    {
        if (isServer)
        {
            RpcGlobalAction(targetIndex, actionIndex);
        }
        else
        {
            CmdGlobalAction(targetIndex, actionIndex);
        }
    }

    [ClientRpc]
    private void RpcGlobalAction(int targetIndex, int actionIndex)
    {
        MapController.onlineGlobalAction.Invoke(targetIndex, actionIndex);
    }

    [Command]
    private void CmdGlobalAction(int targetIndex, int actionIndex)
    {
        RpcGlobalAction(targetIndex, actionIndex);
    }
    
    public void DestroyPlayer(int playerId)
    {
        if (isServer)
        {
            RpcDestroyPlayer(playerId);
        }
        else
        {
            Debug.Log(playerId);
            CmdDestroyPlayer(playerId);
        }
    }

    [ClientRpc]
    private void RpcDestroyPlayer(int playerId)
    {
        TurnController.killPlayer.Invoke(playerId);
    }

    [Command]
    private void CmdDestroyPlayer(int playerId)
    {
        RpcDestroyPlayer(playerId);
    }
}
