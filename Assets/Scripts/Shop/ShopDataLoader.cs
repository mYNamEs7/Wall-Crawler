using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Static;
using UnityEngine;
using UnityEngine.Networking;

namespace Shop
{
    public static class ShopDataLoader
    {
        private const string jsonFileName = "ShopItems.json";
        private static string jsonData;
        public static ShopItems ShopItems { get; private set; }
        public static bool WheelIsBought(int id) => ItemIsBought(ShopItems.characters, id);
        public static bool CarIsBought(int id) => ItemIsBought(ShopItems.ropes, id);

        private static bool ItemIsBought(IEnumerable<ShopItemBase> items, int id) => items.FirstOrDefault(item => item.id == id + 1)!.isBought == 1;

        private static void SaveJson()
        {
            var json = JsonUtility.ToJson(ShopItems, true);
            PlayerPrefs.SetString("GameData", json);
            PlayerPrefs.Save();
            // File.WriteAllText(Path.Combine(Application.streamingAssetsPath, jsonFileName), json);
        }

        public static void SetWheelBoughtById(int id)
        {
            var colorToBuy = ShopItems.characters.FirstOrDefault(color => color.id == id);
            colorToBuy!.isBought = 1;
            StaticData.SelectedRopeId = id;
            SaveJson();
        }
        
        public static void SetItemBoughtById(IEnumerable<ShopItemBase> items, int id)
        {
            var colorToBuy = items.FirstOrDefault(color => color.id == id + 1);
            colorToBuy!.isBought = 1;
            SaveJson();
        }

        public static void ResetJSON()
        {
            ShopItems.characters
                .Where(color => color.id != 1)
                .ToList()
                .ForEach(color => color.isBought = 0);
            
            ShopItems.ropes
                .Where(color => color.id != 1)
                .ToList()
                .ForEach(color => color.isBought = 0);
            
            SaveJson();
        }

        public static async Task LoadData()
        {
            var path = Path.Combine(Application.streamingAssetsPath, jsonFileName);

            var www = UnityWebRequest.Get(path);

            www.SendWebRequest();
            
            while (!www.isDone)
            {
                await Task.Yield();
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                jsonData = www.downloadHandler.text;
            }

            www.Dispose();
            
            if (jsonData != null)
                ShopItems = JsonUtility.FromJson<ShopItems>(jsonData);
            
            if (PlayerPrefs.HasKey("GameData"))
            {
                jsonData = PlayerPrefs.GetString("GameData");
                ShopItems = JsonUtility.FromJson<ShopItems>(jsonData);
            }
            else
            {
                PlayerPrefs.SetString("GameData", jsonData);
                PlayerPrefs.Save();
                ResetJSON();
            }
        }
    }
}
