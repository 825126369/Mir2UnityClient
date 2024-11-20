using NetProtocols.Game;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Mir2
{
    public class UserObject: MonoBehaviour, MapObject
    {
        public float fSpeed = 100f;
        bool bAtuoRun = false;

        public SpriteRenderer mEquipWeapon;
        public SpriteRenderer mEquipWeaponEffect;
        public SpriteRenderer mEquipWeapon2;

        public SpriteRenderer mEquipArmour;
        public SpriteRenderer mEquipHead;
        public SpriteRenderer mEquipWingEffect;

        public SpriteRenderer mEquipMount;

        private float InputDelay = 0.4f;
        private float NextRunTime = 0;


        private bool bInit = false;
        private UserData mData;

        public string WeaponLibrary1, WeaponEffectLibrary1, WeaponLibrary2, HairLibrary, WingLibrary, MountLibrary;
        public int Armour, Weapon, WeaponEffect, ArmourOffSet, HairOffSet, WeaponOffSet, WingOffset, MountOffset;
        public byte Hair;
        public byte WingEffect;

        public FrameSet Frames = FrameSet.Player;
        public Frame Frame, WingFrame;
        public int FrameIndex, FrameInterval, EffectFrameIndex, EffectFrameInterval, SlowFrameIndex;

        string BodyLibrary;

        public bool RidingMount, Sprint, FastRun, Fishing, FoundFish, Sneaking;
        public short MountType = -1, TransformType = -1;

        public int DrawFrame, DrawWingFrame;
        public bool SkipFrames = false;
        public int SkipFrameUpdate;
        public PoisonType Poison;
        public List<QueuedAction> ActionFeed = new List<QueuedAction>();
        private QueuedAction RequestAction;
        private QueuedAction NextAction;
        public MirAction CurrentAction;
        public QueuedAction CurrentQueuedAction;

        private TimeOutGenerator mTimeOutGenerator_ForMove = null;

        public void Init()
        {
            if (bInit) return;
            bInit = true;

            mData = DataCenter.Instance.UserData;
            mTimeOutGenerator_ForMove = TimeOutGenerator.New(0.1f);

            InitPos();
            InitEquipEffect();
            SetAction();
        }

        public bool HasClassWeapon
        {
            get
            {
                switch (Weapon / Globals.ClassWeaponCount)
                {
                    default:
                        return mData.Class == MirClass.Wizard || mData.Class == MirClass.Warrior || mData.Class == MirClass.Taoist;
                    case 1:
                        return mData.Class == MirClass.Assassin;
                    case 2:
                        return mData.Class == MirClass.Archer;
                }
            }
        }

        public void InitEquipEffect()
        {
            bool altAnim = false;

            switch (mData.Class)
            {
                case MirClass.Archer:
                    {
                        if (HasClassWeapon)
                        {
                            switch (CurrentAction)
                            {
                                case MirAction.Walking:
                                case MirAction.Running:
                                case MirAction.AttackRange1:
                                case MirAction.AttackRange2:
                                    altAnim = true;
                                    break;
                            }
                        }

                        if (CurrentAction == MirAction.Jump) altAnim = true;
                        if (altAnim)
                        {
                            BodyLibrary = Armour < Mir2Res.ARArmours.Length ? Mir2Res.ARArmours[Armour] : Mir2Res.ARArmours[0];
                            HairLibrary = Hair < Mir2Res.ARHair.Length ? Mir2Res.ARHair[Hair] : null;
                        }
                        else
                        {
                            BodyLibrary = Armour < Mir2Res.CArmours.Length ? Mir2Res.CArmours[Armour] : Mir2Res.CArmours[0];
                            HairLibrary = Hair < Mir2Res.CHair.Length ? Mir2Res.CHair[Hair] : null;
                        }


                        if (HasClassWeapon)
                        {
                            int Index = Weapon - 200;

                            if (altAnim)
                                WeaponLibrary2 = Index < Mir2Res.ARWeaponsS.Length ? Mir2Res.ARWeaponsS[Index] : null;
                            else
                                WeaponLibrary2 = Index < Mir2Res.ARWeapons.Length ? Mir2Res.ARWeapons[Index] : null;

                            WeaponLibrary1 = null;
                        }
                        else
                        {
                            if (Weapon >= 0)
                            {
                                WeaponLibrary1 = Weapon < Mir2Res.CWeapons.Length ? Mir2Res.CWeapons[Weapon] : null;
                                if (WeaponEffect > 0)
                                    WeaponEffectLibrary1 = WeaponEffect < Mir2Res.CWeaponEffect.Length ? Mir2Res.CWeaponEffect[WeaponEffect] : null;
                                else
                                    WeaponEffectLibrary1 = null;

                                WeaponLibrary2 = null;
                            }
                            else
                            {
                                WeaponLibrary1 = null;
                                WeaponEffectLibrary1 = null;
                                WeaponLibrary2 = null;
                            }
                        }

                        if (WingEffect > 0 && WingEffect < 100)
                        {
                            if (altAnim)
                                WingLibrary = (WingEffect - 1) < Mir2Res.ARHumEffect.Length ? Mir2Res.ARHumEffect[WingEffect - 1] : null;
                            else
                                WingLibrary = (WingEffect - 1) < Mir2Res.CHumEffect.Length ? Mir2Res.CHumEffect[WingEffect - 1] : null;
                        }

                        ArmourOffSet = mData.Gender == MirGender.Male ? 0 : altAnim ? 352 : 808;
                        HairOffSet = mData.Gender == MirGender.Male ? 0 : altAnim ? 352 : 808;
                        WeaponOffSet = mData.Gender == MirGender.Male ? 0 : altAnim ? 352 : 416;
                        WingOffset = mData.Gender == MirGender.Male ? 0 : altAnim ? 352 : 840;
                        MountOffset = 0;
                    }
                    break;
                case MirClass.Assassin:
                    {
                        if (HasClassWeapon || Weapon < 0)
                        {
                            switch (CurrentAction)
                            {
                                case MirAction.Standing:
                                case MirAction.Stance:
                                case MirAction.Walking:
                                case MirAction.Running:
                                case MirAction.Die:
                                case MirAction.Struck:
                                case MirAction.Attack1:
                                case MirAction.Attack2:
                                case MirAction.Attack3:
                                case MirAction.Attack4:
                                case MirAction.Sneek:
                                case MirAction.Spell:
                                case MirAction.DashAttack:
                                    altAnim = true;
                                    break;
                            }
                        }

                        if (altAnim)
                        {
                            BodyLibrary = Armour < Mir2Res.AArmours.Length ? Mir2Res.AArmours[Armour] : Mir2Res.AArmours[0];
                            HairLibrary = Hair < Mir2Res.AHair.Length ? Mir2Res.AHair[Hair] : null;
                        }
                        else
                        {
                            BodyLibrary = Armour < Mir2Res.CArmours.Length ? Mir2Res.CArmours[Armour] : Mir2Res.CArmours[0];
                            HairLibrary = Hair < Mir2Res.CHair.Length ? Mir2Res.CHair[Hair] : null;
                        }

                        if (HasClassWeapon)
                        {
                            int Index = Weapon - 100;

                            WeaponLibrary1 = Index < Mir2Res.AWeaponsL.Length ? Mir2Res.AWeaponsR[Index] : null;
                            WeaponLibrary2 = Index < Mir2Res.AWeaponsR.Length ? Mir2Res.AWeaponsL[Index] : null;
                        }
                        else
                        {
                            if (Weapon >= 0)
                            {
                                WeaponLibrary1 = Weapon < Mir2Res.CWeapons.Length ? Mir2Res.CWeapons[Weapon] : null;
                                if (WeaponEffect > 0)
                                    WeaponEffectLibrary1 = WeaponEffect < Mir2Res.CWeaponEffect.Length ? Mir2Res.CWeaponEffect[WeaponEffect] : null;
                                else
                                    WeaponEffectLibrary1 = null;

                                WeaponLibrary2 = null;
                            }
                            else
                            {
                                WeaponLibrary1 = null;
                                WeaponEffectLibrary1 = null;
                                WeaponLibrary2 = null;
                            }
                        }

                        if (WingEffect > 0 && WingEffect < 100)
                        {
                            if (altAnim)
                                WingLibrary = (WingEffect - 1) < Mir2Res.AHumEffect.Length ? Mir2Res.AHumEffect[WingEffect - 1] : null;
                            else
                                WingLibrary = (WingEffect - 1) < Mir2Res.CHumEffect.Length ? Mir2Res.CHumEffect[WingEffect - 1] : null;
                        }

                        ArmourOffSet = mData.Gender == MirGender.Male ? 0 : altAnim ? 512 : 808;
                        HairOffSet = mData.Gender == MirGender.Male ? 0 : altAnim ? 512 : 808;
                        WeaponOffSet = mData.Gender == MirGender.Male ? 0 : altAnim ? 512 : 416;
                        WingOffset = mData.Gender == MirGender.Male ? 0 : altAnim ? 544 : 840;
                        MountOffset = 0;
                    }
                    break;
                case MirClass.Warrior:
                case MirClass.Taoist:
                case MirClass.Wizard:
                    {
                        BodyLibrary = Armour < Mir2Res.CArmours.Length ? Mir2Res.CArmours[Armour] : Mir2Res.CArmours[0];
                        HairLibrary = Hair < Mir2Res.CHair.Length ? Mir2Res.CHair[Hair] : null;

                        if (Weapon >= 0)
                        {
                            WeaponLibrary1 = Weapon < Mir2Res.CWeapons.Length ? Mir2Res.CWeapons[Weapon] : null;
                            if (WeaponEffect > 0)
                                WeaponEffectLibrary1 = WeaponEffect < Mir2Res.CWeaponEffect.Length ? Mir2Res.CWeaponEffect[WeaponEffect] : null;
                            else
                                WeaponEffectLibrary1 = null;
                        }
                        else
                        {
                            WeaponLibrary1 = null;
                            WeaponEffectLibrary1 = null;
                            WeaponLibrary2 = null;
                        }

                        if (WingEffect > 0 && WingEffect < 100)
                        {
                            WingLibrary = (WingEffect - 1) < Mir2Res.CHumEffect.Length ? Mir2Res.CHumEffect[WingEffect - 1] : null;
                        }


                        ArmourOffSet = mData.Gender == MirGender.Male ? 0 : 808;
                        HairOffSet = mData.Gender == MirGender.Male ? 0 : 808;
                        WeaponOffSet = mData.Gender == MirGender.Male ? 0 : 416;
                        WingOffset = mData.Gender == MirGender.Male ? 0 : 840;
                        MountOffset = 0;
                    }
                    break;
                default:
                    break;
            }
        }

        public void DrawBody()
        {
            int nFrameIndex = DrawFrame + ArmourOffSet;
            string mSpriteName = nFrameIndex + ".png";
            string path = Path.Combine(BodyLibrary, mSpriteName);
            Mir2Res.Instance.SetSprite(path, (mSprite)=>
            {
                mEquipArmour.sprite = mSprite;
                mEquipArmour.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
            });
        }

        public void DrawHead()
        {
            if (!string.IsNullOrWhiteSpace(HairLibrary))
            {
                int nFrameIndex = DrawFrame + HairOffSet;
                string mSpriteName = nFrameIndex + ".png";
                string path = Path.Combine(HairLibrary, mSpriteName);
                Mir2Res.Instance.SetSprite(path, (mSprite) =>
                {
                    mEquipHead.sprite = mSprite;
                    mEquipHead.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
                });
            }
        }

        public void DrawWeapon()
        {
            if (Weapon < 0) return;

            if (!string.IsNullOrWhiteSpace(WeaponLibrary1))
            {
                int nFrameIndex = DrawFrame + WeaponOffSet;
                string mSpriteName = nFrameIndex + ".png";
                string path = Path.Combine(WeaponLibrary1, mSpriteName);
                Mir2Res.Instance.SetSprite(path, (mSprite) =>
                {
                    mEquipWeapon.sprite = mSprite;
                    mEquipWeapon.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
                });

                if (!string.IsNullOrWhiteSpace(WeaponEffectLibrary1))
                {
                    path = Path.Combine(WeaponEffectLibrary1, mSpriteName);
                    Mir2Res.Instance.SetSprite(path, (mSprite) =>
                    {
                        mEquipWeaponEffect.sprite = mSprite;
                        mEquipWeaponEffect.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
                    });
                }
            }
        }

        public void DrawWeapon2()
        {
            if (Weapon == -1) return;

            if (!string.IsNullOrWhiteSpace(WeaponLibrary2))
            {
                int nFrameIndex = DrawFrame + WeaponOffSet;
                string mSpriteName = nFrameIndex + ".png";
                string path = Path.Combine(WeaponLibrary2, mSpriteName);
                Mir2Res.Instance.SetSprite(path, (mSprite) =>
                {
                    mEquipWeapon2.sprite = mSprite;
                    mEquipWeapon2.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
                });
            }
        }

        public void DrawWings()
        {
            if (WingEffect <= 0 || WingEffect >= 100) return;
            if (!string.IsNullOrWhiteSpace(WingLibrary))
            {
                int nFrameIndex = DrawWingFrame + WingOffset;
                string mSpriteName = nFrameIndex + ".png";
                string path = Path.Combine(WingLibrary, mSpriteName);
                Mir2Res.Instance.SetSprite(path, (mSprite) =>
                {
                    mEquipWingEffect.sprite = mSprite;
                    mEquipWingEffect.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
                });
            }
        }

        public void DrawMount()
        {
            if (MountType < 0 || !RidingMount) return;

            if (!string.IsNullOrWhiteSpace(MountLibrary))
            {
                int nFrameIndex = DrawFrame - 416 + MountOffset;
                string mSpriteName = nFrameIndex + ".png";
                string path = Path.Combine(MountLibrary, mSpriteName);
                Mir2Res.Instance.SetSprite(path, (mSprite) =>
                {
                    mEquipMount.sprite = mSprite;
                    mEquipMount.GetComponent<CrystalMir2TextureInfo>().SetUrl(path);
                });
            }
        }

        public void Draw()
        {
            mEquipMount.sprite = null;
            mEquipWeapon.sprite = null;
            mEquipArmour.sprite = null;
            mEquipHead.sprite = null;
            mEquipWeapon2.sprite = null;
            mEquipWeaponEffect.sprite = null;
            mEquipWingEffect.sprite = null;

            mEquipMount.sortingOrder = 0;

            if (mData.Direction == MirDirection.Left || mData.Direction == MirDirection.Up || mData.Direction == MirDirection.UpLeft || mData.Direction == MirDirection.DownLeft)
            {
                mEquipWeapon.sortingOrder = 1;
            }
            else
            {
                mEquipWeapon2.sortingOrder = 1;
            }

            mEquipArmour.sortingOrder = 3;
            mEquipHead.sortingOrder = 4;

            if (mData.Direction == MirDirection.UpRight || mData.Direction == MirDirection.Right || mData.Direction == MirDirection.DownRight || mData.Direction == MirDirection.Down)
            {
                mEquipWeapon.sortingOrder = 5;
            }
            else
            {
                mEquipWeapon2.sortingOrder = 5;
            }
            
            DrawMount();
            DrawBody();
            DrawHead();
            DrawWeapon();
            DrawWeapon2();
        }

        public int UpdateFrame(bool skip = true)
        {
            if (Frame == null) return 0;
            if (Poison.HasFlag(PoisonType.Slow) && !skip)
            {
                SkipFrameUpdate++;
                if (SkipFrameUpdate == 2)
                    SkipFrameUpdate = 0;
                else
                    return FrameIndex;
            }
            if (Frame.Reverse) return Math.Abs(--FrameIndex);

            return ++FrameIndex;
        }

        public virtual void ProcessFrames()
        {
            if (Frame == null) return;

            switch (CurrentAction)
            {
                case MirAction.Walking:
                case MirAction.Running:
                case MirAction.MountWalking:
                case MirAction.MountRunning:
                case MirAction.Sneek:
                case MirAction.DashAttack:
                    if (mTimeOutGenerator_ForMove.orTimeOut())
                    {
                        if (SkipFrames) UpdateFrame();
                        if (UpdateFrame(false) >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                    }
                    break;
            }
        }

        public void SetAction()
        {
            if (RequestAction != null)
            {
                if ((ActionFeed.Count == 0) || (ActionFeed.Count == 1 && NextAction.Action == MirAction.Stance))
                {
                    ActionFeed.Clear();
                    ActionFeed.Add(RequestAction);
                    RequestAction = null;
                }
            }
            
            CurrentQueuedAction = null;
            if (ActionFeed.Count == 0)
            {
                CurrentAction = MirAction.Standing;

                Frames.TryGetValue(CurrentAction, out Frame);
                FrameIndex = 0;
                EffectFrameIndex = 0;
                if (Frame != null)
                {
                    FrameInterval = Frame.Interval;
                    EffectFrameInterval = Frame.EffectInterval;
                }
            }
            else
            {
                QueuedAction action = ActionFeed[0];
                ActionFeed.RemoveAt(0);
                CurrentAction = action.Action;

                mData.MapLocation = action.Location;
                MirDirection olddirection = mData.Direction;
                mData.Direction = action.Direction;

                CurrentQueuedAction = action;

                Frames.TryGetValue(CurrentAction, out Frame);
                FrameIndex = 0;
                EffectFrameIndex = 0;
                if (Frame != null)
                {
                    FrameInterval = Frame.Interval;
                    EffectFrameInterval = Frame.EffectInterval;
                }
            }

            if (CurrentQueuedAction != null)
            {
                switch (CurrentAction)
                {
                    case MirAction.Standing:
                    case MirAction.MountStanding:
                        SendTurnDirMsg(CurrentQueuedAction.Direction);
                        break;
                    case MirAction.Walking:
                    case MirAction.MountWalking:
                    case MirAction.Sneek:
                        SendWalkMsg(CurrentQueuedAction.Direction);
                        break;
                    case MirAction.Running:
                    case MirAction.MountRunning:
                        SendRunMsg(CurrentQueuedAction.Direction);
                        break;
                }
            }
        }

        private void InitPos()
        {
            transform.position = GetTargetLocation(mData.MapLocation);
            PrintTool.Log("InitPos: " + mData.MapLocation);
        }

        private Vector3 GetTargetLocation(Vector3Int Location, MirDirection dir, int i = 1)
        {
            var mTargetMapLocation = Functions.PointMove(Location, dir, i);
            return new Vector3(mTargetMapLocation.x * DataCenter.CellWidth, -mTargetMapLocation.y * DataCenter.CellHeight, 0);
        }

        private Vector3 GetTargetLocation(Vector3Int Location)
        {
            return new Vector3(Location.x * DataCenter.CellWidth, -Location.y * DataCenter.CellHeight, 0);
        }

        private void UpdateLocation(Vector3Int Location, MirDirection dir)
        {
            mData.MapLocation = Location;
            mData.Direction = dir;
            transform.position = GetTargetLocation(Location);
        }

        private void Update()
        {
            if (!bInit) return;
            SkipFrames = ActionFeed.Count > 1;

            ProcessFrames();
            Draw();

            if (Frame == null)
            {
                DrawFrame = 0;
                DrawWingFrame = 0;
            }
            else
            {
                DrawFrame = Frame.Start + (Frame.OffSet * (byte)mData.Direction) + FrameIndex;
                DrawWingFrame = Frame.EffectStart + (Frame.EffectOffSet * (byte)mData.Direction) + EffectFrameIndex;
            }

            if (CurrentQueuedAction != null)
            {
                switch (CurrentAction)
                {
                    case MirAction.Walking:
                    case MirAction.Running:
                        if (Frame == null)
                        {
                            break;
                        }

                        Vector3 beginPos = GetTargetLocation(mData.MapLocation, mData.Direction, -1);
                        Vector3 targetPos = GetTargetLocation(mData.MapLocation);

                        int count = Frame.Count;
                        int index = FrameIndex;
                        float fPercent = (index + 1) / (float)count;

                        transform.position = beginPos * (1 - fPercent) + targetPos * fPercent;
                        break;
                    default:
                        break;
                }
            }

            CheckInput();
            if (RequestAction != null)
            {
                SetAction();
            }
        }

        private void CheckInput()
        {
            if (Time.time < InputDelay) return;
            InputDelay = Time.time + 0.2f;

            bool bClickMap = false;
            if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
            {
                float distance = 1000f;
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, distance))
                {
                    BoxCollider mHitCollider = hit.collider.GetComponent<BoxCollider>();
                    if (mHitCollider != null && mHitCollider.gameObject.name == "MapClickBoxCollider")
                    {
                        PrintTool.Log("点击地图：" + mHitCollider.transform.position);
                        bClickMap = true;
                    }
                }
#if UNTIY_EDITOR
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.green);
#endif
            }

            if (bClickMap)
            {
                var direction = MouseDirection();
                bool AutoRun = false;
                if (AutoRun)
                {
                    if (CanRun(direction) && Time.time > NextRunTime)
                    {
                        int distance = RidingMount || Sprint ? 3 : 2;
                        bool fail = false;
                        for (int i = 1; i <= distance; i++)
                        {
                            if (!WorldMgr.Instance.CheckDoorOpen(Functions.PointMove(mData.MapLocation, direction, i)))
                                fail = true;
                        }

                        if (!fail)
                        {
                            RequestAction = new QueuedAction { Action = MirAction.Running, Direction = direction, Location = Functions.PointMove(mData.MapLocation, direction, distance) };
                            return;
                        }
                    }
                    if ((CanWalk(direction, out direction)) && (WorldMgr.Instance.CheckDoorOpen(Functions.PointMove(mData.MapLocation, direction, 1))))
                    {
                        RequestAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(mData.MapLocation, direction, 1) };
                        return;
                    }
                    if (direction != mData.Direction)
                    {
                        RequestAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = mData.MapLocation };
                        return;
                    }
                }
                else if (Input.GetMouseButton(0))
                {
                    if (!WorldMgr.Instance.ValidPoint(Functions.PointMove(mData.MapLocation, direction, 1)))
                    {
                        var mEquipInfo = mData.mEquipList.Find((x) => x.nSlotIndex == (int)EquipmentSlot.Weapon);
                        if (mEquipInfo != null)
                        {
                            var mItemIndex = mEquipInfo.nItemId;
                            var mItemInfo = ExcelTableMgr.Instance.mItemList[(int)mItemIndex];

                            if (mEquipInfo != null && mItemInfo.CanMine)
                            {
                                if (direction != mData.Direction)
                                {
                                    RequestAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = mData.MapLocation };
                                    return;
                                }
                                return;
                            }
                        }
                    }

                    if ((CanWalk(direction, out direction)) && (WorldMgr.Instance.CheckDoorOpen(Functions.PointMove(mData.MapLocation, direction, 1))))
                    {
                        RequestAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(mData.MapLocation, direction, 1) };
                        return;
                    }

                    if (direction != mData.Direction)
                    {
                        RequestAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = mData.MapLocation };
                        return;
                    }
                }
                else if (Input.GetMouseButton(1))
                {
                    if (Functions.InRange(MouseClickMapLocation(), mData.MapLocation, 2))
                    {
                        if (direction != mData.Direction)
                        {
                            RequestAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = mData.MapLocation };
                        }
                        return;
                    }

                    if (CanRun(direction))
                    {
                        int distance = RidingMount || (Sprint && !Sneaking) ? 3 : 2;
                        bool fail = false;
                        for (int i = 0; i <= distance; i++)
                        {
                            if (!WorldMgr.Instance.CheckDoorOpen(Functions.PointMove(mData.MapLocation, direction, i)))
                                fail = true;
                        }
                        if (!fail)
                        {
                            RequestAction = new QueuedAction { Action = MirAction.Running, Direction = direction, Location = Functions.PointMove(mData.MapLocation, direction, RidingMount || (Sprint && !Sneaking) ? 3 : 2) };
                            return;
                        }
                    }
                    if ((CanWalk(direction, out direction)) && (WorldMgr.Instance.CheckDoorOpen(Functions.PointMove(mData.MapLocation, direction, 1))))
                    {
                        RequestAction = new QueuedAction { Action = MirAction.Walking, Direction = direction, Location = Functions.PointMove(mData.MapLocation, direction, 1) };
                        return;
                    }

                    if (direction != mData.Direction)
                    {
                        RequestAction = new QueuedAction { Action = MirAction.Standing, Direction = direction, Location = mData.MapLocation };
                        return;
                    }
                }
            }
        }

        public static Vector3Int MouseClickMapLocation()
        {
            var WorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            return new Vector3Int((int)(WorldPos.x / DataCenter.CellWidth), -(int)(WorldPos.y / DataCenter.CellHeight));
        }

        private bool CanWalk(MirDirection dir)
        {
            return WorldMgr.Instance.EmptyCell(Functions.PointMove(mData.MapLocation, dir, 1));
        }

        private bool CanWalk(MirDirection dir, out MirDirection outDir)
        {
            outDir = dir;

            if (WorldMgr.Instance.EmptyCell(Functions.PointMove(mData.MapLocation, dir, 1)))
                return true;

            dir = Functions.NextDir(outDir);
            if (WorldMgr.Instance.EmptyCell(Functions.PointMove(mData.MapLocation, dir, 1)))
            {
                outDir = dir;
                return true;
            }

            dir = Functions.PreviousDir(outDir);
            if (WorldMgr.Instance.EmptyCell(Functions.PointMove(mData.MapLocation, dir, 1)))
            {
                outDir = dir;
                return true;
            }

            return false;
        }

        private bool CanRun(MirDirection dir)
        {
            if (CanWalk(dir) && WorldMgr.Instance.EmptyCell(Functions.PointMove(mData.MapLocation, dir, 2)))
            {
                if (RidingMount)
                {
                    return WorldMgr.Instance.EmptyCell(Functions.PointMove(mData.MapLocation, dir, 3));
                }

                return true;
            }

            return false;
        }

        private MirDirection GetDirection(Vector3 dir)
        {
            if (dir.x > 0)
            {
                if (dir.y < 0)
                    return MirDirection.DownRight;
                if (dir.y > 0)
                    return MirDirection.UpRight;
                return MirDirection.Right;
            }

            if (dir.x < 0)
            {
                if (dir.y < 0)
                    return MirDirection.DownLeft;
                if (dir.y > 0)
                    return MirDirection.UpLeft;
                return MirDirection.Left;
            }
            return dir.y < 0 ? MirDirection.Down : MirDirection.Up;
        }

        public MirDirection MouseDirection()
        {
            var Dir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            Dir = Vector3.Normalize(new Vector3(Dir.x, Dir.y, 0));
            //Vector3Int p = MouseClickMapLocation();
            //if (Functions.InRange(mData.MapLocation, p, 2))
            //    return Functions.DirectionFromPoint(mData.MapLocation, p);

            float fAngle = 360 / 8;
            float fNowAngle = Mathf.Acos(Mathf.Abs(Dir.x)) / MathF.PI * 180;

            if(Dir.x > 0)
            {
                if(Dir.y > 0)
                {
                    if(fNowAngle <  fAngle / 2)
                    {
                        return MirDirection.Right;
                    }
                    else if (fNowAngle < fAngle + fAngle / 2)
                    {
                        return MirDirection.UpRight;
                    }
                    else
                    {
                        return MirDirection.Up;
                    }
                }
                else
                {
                    if (fNowAngle < fAngle / 2)
                    {
                        return MirDirection.Right;
                    }
                    else if (fNowAngle < fAngle + fAngle / 2)
                    {
                        return MirDirection.DownRight;
                    }
                    else
                    {
                        return MirDirection.Down;
                    }
                }
            }
            else
            {
                if (Dir.y > 0)
                {
                    if (fNowAngle < fAngle / 2)
                    {
                        return MirDirection.Left;
                    }
                    else if (fNowAngle < fAngle + fAngle / 2)
                    {
                        return MirDirection.UpLeft;
                    }
                    else
                    {
                        return MirDirection.Up;
                    }
                }
                else
                {
                    if (fNowAngle < fAngle / 2)
                    {
                        return MirDirection.Left;
                    }
                    else if (fNowAngle < fAngle + fAngle / 2)
                    {
                        return MirDirection.DownLeft;
                    }
                    else
                    {
                        return MirDirection.Down;
                    }
                }
            }

            return GetDirection(Dir);
        }

        //---------------------------------------网络消息-----------------------------------------------------------

        private void SendTurnDirMsg(MirDirection Direction)
        {
            var mSendMsg = new packet_cs_request_TurnDir();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_TURNDIR, mSendMsg);
        }

        private void SendWalkMsg(MirDirection Direction)
        {
            var mSendMsg = new packet_cs_request_Walk();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_WALK, mSendMsg);
        }

        private void SendRunMsg(MirDirection Direction)
        {
            var mSendMsg = new packet_cs_request_Run();
            mSendMsg.Direction = (uint)Direction;
            NetClientGameMgr.SendNetData(NetProtocolCommand.CS_REQUEST_RUN, mSendMsg);
        }

        public void HandleServerLocation(Vector3Int Location, MirDirection dir)
        {
            if (mData.MapLocation == Location && mData.Direction == dir) return;

            UpdateLocation(Location, dir);
            ActionFeed.Clear();
            SetAction();

            WorldMgr.Instance.MapMgr.UpdateMap();
        }

    }
}