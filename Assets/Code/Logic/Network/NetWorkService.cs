using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using  Lockstep.Game;
using NetMsg.Common;
using Lockstep.Serialization;

namespace Logic
{
    public class NetWorkService
    {
        private static NetWorkService inst;
    
        public static NetWorkService Instance
        {
            get
            {
                if(inst == null)
                    inst = new NetWorkService();
                return inst;
            }
        }


        private NetClient netClient;

        public void Init()
        {
            netClient = new NetClient();
            netClient.NetMsgHandler += OnNetMsg;
            netClient.DoStart();
        }
        
        void OnNetMsg(ushort opcode, object msg){
            var type = (EMsgSC) opcode;
            switch (type) {
                //login
                // case EMsgSC.L2C_JoinRoomResult: 
                

            }
        }
        
        public void UnInit()
        {
            if (netClient != null)
            {
                netClient.NetMsgHandler -= OnNetMsg;
                netClient.DoDestroy();
            }
        }
        
        public void SendTcp(EMsgSC msgId, BaseMsg body){
            var writer = new Serializer();
            body.Serialize(writer);
            netClient?.SendMessage(msgId, writer.CopyData());
        }

        public void ConnectSrv()
        {
            SendTcp(EMsgSC.C2L_JoinRoom,new Msg_C2L_JoinRoom() {
                RoomId = 0
            });
        }
    }
    
}
