using System;
using System.Collections;
using System.Linq;
using GameCycle;
using Shop;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class StreakRewardUI : MarkedUI
    {
        public static event Action<int> StartGetReward;
        
        public static event Action<int, Vector3> OnMoneyAdded;
        public static event Action<int, ItemType> InitRewardMenu;
        public static event Action<int, ItemType, int, ItemType> InitTwiceRewardMenu;
        
        private enum Rare
        {
            Normal,
            Epic,
            Legend
        }

        [SerializeField] private Rare _rare;
        [SerializeField] private int _index;
        [SerializeField] private bool _isWithoutAnim;

        private Transform _moneyImage;
        private Transform _skinImage;
        private Transform _webImage;

        private int _money;
        private int _isWebReward = -1;
        private float _delay;
        private int _winStreak;
        
        protected override void OnEnable()
        {
            if (_winStreak == StaticData.WinStreak) return;

            switch (_rare)
            {
                case Rare.Normal:
                    break;
                case Rare.Epic:
                    _index += 7;
                    break;
                case Rare.Legend:
                    _index += 14;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _moneyImage = transform.GetChild(0).GetChild(0);
            _skinImage = transform.GetChild(0).GetChild(1);
            _webImage = transform.GetChild(0).GetChild(2);
            
            ArrowUI.OnStreakCompleted += ArrowUIOnOnStreakCompleted;
            
            base.OnEnable();
            _markImage.GetComponentInChildren<Animation>().enabled = true;

            if(_isWithoutAnim)
                SetMarked();
            else
                SetAnimatedMarked();

            StartCoroutine(SetRewards());
            // if (StaticData.WinStreak == 8)
            //     ArrowUIOnOnStreakCompleted();
        }

        private IEnumerator SetRewards()
        {
            yield return null;
            
            SetReward();
        }

        private void OnDisable()
        {
            switch (_rare)
            {
                case Rare.Normal:
                    break;
                case Rare.Epic:
                    _index -= 7;
                    break;
                case Rare.Legend:
                    _index -= 14;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            _winStreak = StaticData.WinStreak;
            ArrowUI.OnStreakCompleted -= ArrowUIOnOnStreakCompleted;
        }

        private void ArrowUIOnOnStreakCompleted()
        {
            StartCoroutine(GetReward());
        }

        private IEnumerator GetReward()
        {
            var delay = _rare switch
            {
                Rare.Normal => 4,
                Rare.Epic => 4,
                Rare.Legend => 6,
                _ => throw new ArgumentOutOfRangeException()
            };
            
            StartGetReward?.Invoke(delay);
            
            print("REWARDS!!!!!!!!");
            yield return new WaitForSeconds(_delay);
            
            if(_money != 0)
                OnMoneyAdded?.Invoke(_money, transform.position);
            else if (_isWebReward != -1)
            {
                if (_rare == Rare.Legend || _isWebReward == 3)
                {
                    GameController.Instance.GetReward(2);
                    InitTwiceRewardMenu?.Invoke(FindObjectOfType<ShopLoader>(true).GetRandomNotBoughtCharacterId(),
                        ItemType.Character, FindObjectOfType<ShopLoader>(true).GetRandomNotBoughtRopeId(),
                        ItemType.Rope);
                    yield break;
                }
                
                GameController.Instance.GetReward(1);
                if (_isWebReward == 0)
                    InitRewardMenu?.Invoke(FindObjectOfType<ShopLoader>(true).GetRandomNotBoughtCharacterId(),
                        ItemType.Character);
                else
                    InitRewardMenu?.Invoke(FindObjectOfType<ShopLoader>(true).GetRandomNotBoughtRopeId(),
                        ItemType.Rope);
            }
        }

        private void DeactivateAllImages()
        {
            gameObject.SetActive(false);
        }

        private void WebActivate()
        {
            _moneyImage.gameObject.SetActive(false);
            _skinImage.gameObject.SetActive(false);
            _webImage.gameObject.SetActive(true);
        }
        
        private void SkinActivate()
        {
            _moneyImage.gameObject.SetActive(false);
            _skinImage.gameObject.SetActive(true);
            _webImage.gameObject.SetActive(false);
        }
        
        private void MoneyActivate()
        {
            _moneyImage.gameObject.SetActive(true);
            _skinImage.gameObject.SetActive(false);
            _webImage.gameObject.SetActive(false);
        }

        private void SetReward()
        {
            switch (_rare)
            {
                case Rare.Normal:
                    switch (_index)
                    {
                        case 1:
                            _money = 100;
                            _delay = 1;
                            MoneyActivate();
                            break;
                        case 3:
                            _money = 100;
                            _delay = 2;
                            MoneyActivate();
                            break;
                        case 5:
                            print(StaticData.FirstRewardState);
                            switch (StaticData.FirstRewardState)
                            {
                                case 0:
                                    _money = 200;
                                    MoneyActivate();
                                    break;
                                case 1:
                                    WebActivate();
                                    break;
                                case 2:
                                    _money = 200;
                                    MoneyActivate();
                                    break;
                            }
                            
                            _delay = 3;
                            break;
                        case 7:
                            switch (StaticData.FirstRewardState)
                            {
                                case 0:
                                    _money = 400;
                                    MoneyActivate();
                                    break;
                                case 1:
                                    SkinActivate();
                                    _isWebReward = 3;
                                    break;
                                case 2:
                                    WebActivate();
                                    _isWebReward = 2;
                                    break;
                            }
                            
                            _delay = 4;
                            break;
                        default:
                            DeactivateAllImages();
                            break;
                    }
                    break;
                case Rare.Epic:
                    switch (_index)
                    {
                        case 8:
                            _money = 100;
                            _delay = 1;
                            MoneyActivate();
                            break;
                        case 9:
                            _money = 150;
                            _delay = 2;
                            MoneyActivate();
                            break;
                        case 11:
                            _money = 200;
                            _delay = 3;
                            MoneyActivate();
                            break;
                        case 14:
                            _isWebReward = StaticData.IsWebStreakReward;
                            _delay = 4;
                            
                            if (_isWebReward == 1)
                                WebActivate();
                            else
                                SkinActivate();
                            break;
                        default:
                            DeactivateAllImages();
                            break;
                    }
                    break;
                case Rare.Legend:
                    switch (_index)
                    {
                        case 15:
                            _money = 150;
                            _delay = 1;
                            MoneyActivate();
                            break;
                        case 16:
                            _money = 200;
                            _delay = 2;
                            MoneyActivate();
                            break;
                        case 17:
                            _money = 250;
                            _delay = 3;
                            MoneyActivate();
                            break;
                        case 18:
                            _isWebReward = 1;
                            _delay = 6;
                            WebActivate();
                            break;
                        case 19:
                            _money = 250;
                            _delay = 4;
                            MoneyActivate();
                            break;
                        case 20:
                            _money = 300;
                            _delay = 5;
                            MoneyActivate();
                            break;
                        case 21:
                            _isWebReward = 0;
                            _delay = 6;
                            SkinActivate();
                            break;
                        default:
                            DeactivateAllImages();
                            break;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            _moneyImage.GetComponentInChildren<Text>().text = $"{_money}";
        }
        
        private void SetMarked()
        {
            if (_index > StaticData.WinStreak - 1) return;

            _markImage.GetComponentInChildren<Animation>().enabled = false;
            SetCleared();
        }

        private void SetAnimatedMarked()
        {
            if (_index < StaticData.WinStreak - 1)
            {
                _markImage.GetComponentInChildren<Animation>().enabled = false;
                SetCleared();
            }
            else if(_index == StaticData.WinStreak - 1)
                SetCleared();
        }
    }
}
