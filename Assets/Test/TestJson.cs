using System;
using UnityEngine;

namespace Test
{
    
    [System.Serializable]
    public class DataPC
    {
        public DataPC()
        {
        
        }
    
        public string name;
        public int maxLevel;
        public string description;
        public string portrait;
 
        public DataItem[] inventory;
    }
 
    [System.Serializable]
    public class DataItem
    {
        public DataItem() {
        }

        public string name;
        public int cost;
    }

    
    public static class TestJson
    {
        public static string Test()
        {
            DataPC myPC  = new DataPC();
            myPC.name = "Jimbob";
            myPC.maxLevel = 99;
            myPC.description = "Jimbob likes beer and trucks.";
            myPC.portrait = "jimbob.png";
            myPC.inventory = new DataItem[2];
 
            myPC.inventory[0] = new DataItem();
            myPC.inventory[1] = new DataItem();
 
            myPC.inventory[0].name = "Silver Bullet";
            myPC.inventory[0].cost = 5;
 
            myPC.inventory[1].name = "Shotgun";
            myPC.inventory[1].cost = 200;

            try
            {
                //序列化成json
                string myJson =  LitJson.JsonMapper.ToJson(myPC);
                Debug.Log("myJson 1 = " + myJson);
            
                //反序列化成对象
                DataPC data = LitJson.JsonMapper.ToObject<DataPC>(myJson);
                if (data != null)
                {
                    myJson =  LitJson.JsonMapper.ToJson(data);
                    Debug.Log("myJson 2 = " + myJson);
                    return myJson;
                }
                return "error occur !!!";
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                return "error occur !!!";
            }
        }
    }
}