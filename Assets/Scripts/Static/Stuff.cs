using UnityEngine;

namespace Static
{
    public class Stuff
    {
        private const string KEY_STUFF = "StuffKey";
        
        public int GetStuffByIndex(int i) => PlayerPrefs.GetInt(KEY_STUFF + i, 0);

        public void SetStuffByIndex(int i, int value)
        {
            PlayerPrefs.SetInt(KEY_STUFF + i, value);
            PlayerPrefs.Save();
        }
    }
}
