using System;
using System.Linq;
using UnityEngine;

namespace Mir2
{
    class MonsterObject : MapObject
    {
        public SpriteRenderer mMonsterSpriteRenderer;

        public override ObjectType Race
        {
            get { return ObjectType.Monster; }
        }

        public override bool Blocking
        {
            get { return AI == 64 || (AI == 81 && Direction == (MirDirection)6) ? false : !Dead; }
        }

        public Vector3Int ManualLocationOffset
        {
            get
            {
                switch (BaseImage)
                {
                    case Monster.EvilMir:
                        return new Vector3Int(-21, -15);
                    case Monster.PalaceWall2:
                    case Monster.PalaceWallLeft:
                    case Monster.PalaceWall1:
                    case Monster.GiGateSouth:
                    case Monster.GiGateWest:
                    case Monster.SSabukWall1:
                    case Monster.SSabukWall2:
                    case Monster.SSabukWall3:
                        return new Vector3Int(-10, 0);
                    case Monster.GiGateEast:
                        return new Vector3Int(-45, 7);
                    default:
                        return new Vector3Int(0, 0);
                }
            }
        }

        public Monster BaseImage;
        public byte Effect;
        public bool Skeleton;

        public FrameSet Frames = new FrameSet();
        public Frame Frame;
        public int FrameIndex, FrameInterval, EffectFrameIndex;

        public uint TargetID;
        public Vector3Int TargetPoint;

        public bool Stoned;
        public byte Stage;
        public int BaseSound;

        public long ShockTime;
        public bool BindingShotCenter;

        public Color OldNameColor;

        public SpellEffect CurrentEffect;

        public MonsterObject(uint objectID) : base(objectID) { }

        //public void Load(S.ObjectMonster info, bool update = false)
        //{
        //    Name = info.Name;
        //    NameColour = info.NameColour;
        //    BaseImage = info.Image;

        //    OldNameColor = NameColour;

        //    CurrentLocation = info.Location;
        //    MapLocation = info.Location;
        //    if (!update) MapControl.Instance.AddObject(this);

        //    Effect = info.Effect;
        //    AI = info.AI;
        //    Light = info.Light;

        //    Direction = info.Direction;
        //    Dead = info.Dead;
        //    Poison = info.Poison;
        //    Skeleton = info.Skeleton;
        //    Hidden = info.Hidden;
        //    ShockTime = CMain.Time + info.ShockTime;
        //    BindingShotCenter = info.BindingShotCenter;

        //    Buffs = info.Buffs;

        //    if (Stage != info.ExtraByte)
        //    {
        //        switch (BaseImage)
        //        {
        //            case Monster.GreatFoxSpirit:
        //                if (update)
        //                {
        //                    Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], 335, 20, 3000, this));
        //                    AudioMgr.Instance.PlaySound(BaseSound + 8);
        //                }
        //                break;
        //            case Monster.HellLord:
        //                {
        //                    Effects.Clear();

        //                    var effects = MapControl.Effects.Where(x => x.Library == Mir2Res.Monsters[(ushort)Monster.HellLord]);

        //                    foreach (var effect in effects)
        //                        effect.Repeat = false;

        //                    if (info.ExtraByte > 3)
        //                        Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 21, 6, 600, this) { Repeat = true });
        //                    else
        //                    {
        //                        if (info.ExtraByte > 2)
        //                            MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 105, 6, 600, new Vector3Int(100, 84)) { Repeat = true });
        //                        if (info.ExtraByte > 1)
        //                            MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 111, 6, 600, new Vector3Int(96, 81)) { Repeat = true });
        //                        if (info.ExtraByte > 0)
        //                            MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 123, 6, 600, new Vector3Int(93, 84)) { Repeat = true });
        //                    }

        //                    AudioMgr.Instance.PlaySound(BaseSound + 9);
        //                }
        //                break;
        //        }
        //    }

        //    Stage = info.ExtraByte;

        //    //Library
        //    switch (BaseImage)
        //    {
        //        case Monster.EvilMir:
        //        case Monster.DragonStatue:
        //            BodyLibrary = Mir2Res.Dragon;
        //            break;
        //        case Monster.EvilMirBody:
        //            break;
        //        case Monster.Catapult:
        //        case Monster.ChariotBallista:
        //        case Monster.Ballista:
        //        case Monster.Trebuchet:
        //        case Monster.CanonTrebuchet:
        //            BodyLibrary = Mir2Res.Siege[((ushort)BaseImage) - 940];
        //            break;
        //        case Monster.SabukGate:
        //        case Monster.PalaceWallLeft:
        //        case Monster.PalaceWall1:
        //        case Monster.PalaceWall2:
        //        case Monster.GiGateSouth:
        //        case Monster.GiGateEast:
        //        case Monster.GiGateWest:
        //        case Monster.SSabukWall1:
        //        case Monster.SSabukWall2:
        //        case Monster.SSabukWall3:
        //        case Monster.NammandGate1:
        //        case Monster.NammandGate2:
        //        case Monster.SabukWallSection:
        //        case Monster.NammandWallSection:
        //        case Monster.FrozenDoor:
        //            BodyLibrary = Mir2Res.Gates[((ushort)BaseImage) - 950];
        //            break;
        //        case Monster.BabyPig:
        //        case Monster.Chick:
        //        case Monster.Kitten:
        //        case Monster.BabySkeleton:
        //        case Monster.Baekdon:
        //        case Monster.Wimaen:
        //        case Monster.BlackKitten:
        //        case Monster.BabyDragon:
        //        case Monster.OlympicFlame:
        //        case Monster.BabySnowMan:
        //        case Monster.Frog:
        //        case Monster.BabyMonkey:
        //        case Monster.AngryBird:
        //        case Monster.Foxey:
        //        case Monster.MedicalRat:
        //            BodyLibrary = Mir2Res.Pets[((ushort)BaseImage) - 10000];
        //            break;
        //        case Monster.HellBomb1:
        //        case Monster.HellBomb2:
        //        case Monster.HellBomb3:
        //            BodyLibrary = Mir2Res.Monsters[(ushort)Monster.HellLord];
        //            break;
        //        case Monster.CaveStatue:
        //            BodyLibrary = Mir2Res.Monsters[(ushort)Monster.CaveStatue];
        //            break;
        //        default:
        //            BodyLibrary = Mir2Res.Monsters[(ushort)BaseImage];
        //            break;
        //    }

        //    if (Skeleton)
        //        ActionFeed.Add(new QueuedAction { Action = MirAction.Skeleton, Direction = Direction, Location = CurrentLocation });
        //    else if (Dead)
        //        ActionFeed.Add(new QueuedAction { Action = MirAction.Dead, Direction = Direction, Location = CurrentLocation });

        //    BaseSound = (ushort)BaseImage * 10;

        //    //Special Actions
        //    switch (BaseImage)
        //    {
        //        case Monster.BoneFamiliar:
        //        case Monster.Shinsu:
        //        case Monster.HolyDeva:
        //        case Monster.HellKnight1:
        //        case Monster.HellKnight2:
        //        case Monster.HellKnight3:
        //        case Monster.HellKnight4:
        //        case Monster.LightningBead:
        //        case Monster.HealingBead:
        //        case Monster.PowerUpBead:
        //            if (!info.Extra) ActionFeed.Add(new QueuedAction { Action = MirAction.Appear, Direction = Direction, Location = CurrentLocation });
        //            break;
        //        case Monster.FrostTiger:
        //        case Monster.FlameTiger:
        //            SitDown = info.Extra;
        //            break;
        //        case Monster.ZumaStatue:
        //        case Monster.ZumaGuardian:
        //        case Monster.FrozenZumaStatue:
        //        case Monster.FrozenZumaGuardian:
        //        case Monster.ZumaTaurus:
        //        case Monster.DemonGuard:
        //        case Monster.Turtlegrass:
        //        case Monster.ManTree:
        //        case Monster.EarthGolem:
        //        case Monster.AssassinScroll:
        //        case Monster.WarriorScroll:
        //        case Monster.TaoistScroll:
        //        case Monster.WizardScroll:
        //        case Monster.PurpleFaeFlower:
        //            Stoned = info.Extra;
        //            break;
        //    }

        //    //Frames
        //    switch (BaseImage)
        //    {
        //        case Monster.GreatFoxSpirit:
        //            Frames = FrameSet.GreatFoxSpirit[Stage];
        //            break;
        //        case Monster.DragonStatue:
        //            Frames = FrameSet.DragonStatue[(byte)Direction];
        //            break;
        //        case Monster.HellBomb1:
        //        case Monster.HellBomb2:
        //        case Monster.HellBomb3:
        //            Frames = FrameSet.HellBomb[((ushort)BaseImage) - 903];
        //            break;
        //        case Monster.CaveStatue:
        //            Frames = FrameSet.CaveStatue[(byte)Direction];
        //            break;
        //        default:
        //            if (BodyLibrary != null)
        //            {
        //                Frames = BodyLibrary.Frames ?? FrameSet.DefaultMonster;
        //            }
        //            break;
        //    }

        //    SetAction();
        //    SetCurrentEffects();

        //    if (CurrentAction == MirAction.Standing)
        //    {
        //        PlayAppearSound();

        //        if (Frame != null)
        //        {
        //            FrameIndex = CMain.Random.Next(Frame.Count);
        //        }
        //    }
        //    else if (CurrentAction == MirAction.SitDown)
        //    {
        //        PlayAppearSound();
        //    }

        //    NextMotion -= NextMotion % 100;

        //    if (Mir2Settings.Effect)
        //    {
        //        switch (BaseImage)
        //        {
        //            case Monster.Weaver:
        //            case Monster.VenomWeaver:
        //            case Monster.ArmingWeaver:
        //            case Monster.ValeBat:
        //            case Monster.CrackingWeaver:
        //            case Monster.GreaterWeaver:
        //                Effects.Add(new Effect(Mir2Res.Effect, 601, 1, 1 * Frame.Interval, this) { DrawBehind = true, Repeat = true }); // Blue effect                        
        //                break;
        //            case Monster.CrystalWeaver:
        //            case Monster.FrozenZumaGuardian:
        //            case Monster.FrozenRedZuma:
        //            case Monster.FrozenZumaStatue:
        //                Effects.Add(new Effect(Mir2Res.Effect, 600, 1, 1 * Frame.Interval, this) { DrawBehind = true, Repeat = true }); // Blue effect
        //                break;
        //            case Monster.CaveStatue:
        //                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CaveStatue], 10, 8, 2400, this) { Blend = true, Repeat = true });
        //                break;
        //        }
        //    }

        //    ProcessBuffs();
        //}

        public void ProcessBuffs()
        {
            for (int i = 0; i < Buffs.Count; i++)
            {
                AddBuffEffect(Buffs[i]);
            }
        }

        public override bool ShouldDrawHealth()
        {
            //string name = string.Empty;
            //if (Name.Contains("(")) name = Name.Substring(Name.IndexOf("(") + 1, Name.Length - Name.IndexOf("(") - 2);

            //return Name.EndsWith(string.Format("({0})", User.Name)) || MirScenes.Dialogs.GroupDialog.GroupList.Contains(name);
            return false;
        }

        public override void Process()
        {
            bool update = CMain.Time >= NextMotion || GameScene.CanMove;
            SkipFrames = ActionFeed.Count > 1;

            ProcessFrames();

            if (Frame == null)
            {
                DrawFrame = 0;
                DrawWingFrame = 0;
            }
            else
            {
                DrawFrame = Frame.Start + (Frame.OffSet * (byte)Direction) + FrameIndex;
                DrawWingFrame = Frame.EffectStart + (Frame.EffectOffSet * (byte)Direction) + EffectFrameIndex;
            }


            #region Moving OffSet

            switch (CurrentAction)
            {
                case MirAction.Walking:
                case MirAction.Running:
                case MirAction.Pushed:
                case MirAction.Jump:
                case MirAction.DashL:
                case MirAction.DashR:
                case MirAction.DashAttack:
                    if (Frame == null)
                    {
                        OffSetMove = Vector3Int.zero;
                        Movement = CurrentLocation;
                        break;
                    }
                    int i = CurrentAction == MirAction.Running ? 2 : 1;

                    if (CurrentAction == MirAction.Jump) i = -JumpDistance;
                    if (CurrentAction == MirAction.DashAttack) i = JumpDistance;

                    Movement = Functions.PointMove(CurrentLocation, Direction, CurrentAction == MirAction.Pushed ? 0 : -i);

                    int count = Frame.Count;
                    int index = FrameIndex;

                    if (CurrentAction == MirAction.DashR || CurrentAction == MirAction.DashL)
                    {
                        count = 3;
                        index %= 3;
                    }

                    switch (Direction)
                    {
                        case MirDirection.Up:
                            OffSetMove = new Vector3Int(0, (int)((MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.UpRight:
                            OffSetMove = new Vector3Int((int)((-MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.Right:
                            OffSetMove = new Vector3Int((int)((-MapControl.CellWidth * i / (float)(count)) * (index + 1)), 0);
                            break;
                        case MirDirection.DownRight:
                            OffSetMove = new Vector3Int((int)((-MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((-MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.Down:
                            OffSetMove = new Vector3Int(0, (int)((-MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.DownLeft:
                            OffSetMove = new Vector3Int((int)((MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((-MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                        case MirDirection.Left:
                            OffSetMove = new Vector3Int((int)((MapControl.CellWidth * i / (float)(count)) * (index + 1)), 0);
                            break;
                        case MirDirection.UpLeft:
                            OffSetMove = new Vector3Int((int)((MapControl.CellWidth * i / (float)(count)) * (index + 1)), (int)((MapControl.CellHeight * i / (float)(count)) * (index + 1)));
                            break;
                    }

                    OffSetMove = new Vector3Int(OffSetMove.x % 2 + OffSetMove.x, OffSetMove.y % 2 + OffSetMove.y);
                    break;
                default:
                    OffSetMove = Vector3Int.zero;
                    Movement = CurrentLocation;
                    break;
            }

            #endregion

            DrawY = Movement.y > CurrentLocation.y ? Movement.y : CurrentLocation.y;

            DrawLocation = new Vector3Int((Movement.x - User.Movement.x + MapControl.OffSetX) * MapControl.CellWidth, (Movement.y - User.Movement.y + MapControl.OffSetY) * MapControl.CellHeight);
            DrawLocation += new Vector3Int(-OffSetMove.x, -OffSetMove.y, 0);
            DrawLocation += User.OffSetMove;
            DrawLocation = DrawLocation + ManualLocationOffset;
            DrawLocation += GlobalDisplayLocationOffset;

            if (BodyLibrary != null && update)
            {
                var mLibraryMgr = (MLibraryMgr.Instance.AddOrGet(BodyLibrary));
                var Size = mLibraryMgr.GetTrueSize(DrawFrame);

                FinalDrawLocation = DrawLocation + (Vector3Int)(mLibraryMgr.GetOffSet(DrawFrame));
                DisplayRectangle = new Rect(DrawLocation.x,DrawLocation.y, Size.x, Size.y);
            }

            for (int i = 0; i < Effects.Count; i++)
                Effects[i].Process();

            Color newColour = Poison switch
            {
                _ when (Poison & PoisonType.DelayedExplosion) == PoisonType.DelayedExplosion => Mir2Color.Orange,
                _ when (Poison & (PoisonType.Paralysis | PoisonType.LRParalysis)) != 0 => Mir2Color.Gray,
                _ when (Poison & PoisonType.Frozen) == PoisonType.Frozen => Mir2Color.Blue,
                _ when (Poison & PoisonType.Blindness) == PoisonType.Blindness => Mir2Color.MediumVioletRed,
                _ when (Poison & (PoisonType.Stun | PoisonType.Dazed)) != 0 => Mir2Color.Yellow,
                _ when (Poison & PoisonType.Slow) == PoisonType.Slow => Mir2Color.Purple,
                _ when (Poison & PoisonType.Bleeding) == PoisonType.Bleeding => Mir2Color.DarkRed,
                _ when (Poison & PoisonType.Red) == PoisonType.Red => Mir2Color.Red,
                _ when (Poison & PoisonType.Green) == PoisonType.Green => Mir2Color.Green,
                _ => Mir2Color.White
            };

            if (newColour != DrawColour)
            {
                DrawColour = newColour;
                MapControl.Instance.TextureValid = false;
            }
        }

        public bool SetAction()
        {
            if (NextAction != null && !GameScene.CanMove)
            {
                switch (NextAction.Action)
                {
                    case MirAction.Walking:
                    case MirAction.Running:
                    case MirAction.Pushed:
                        return false;
                }
            }

            //IntelligentCreature
            switch (BaseImage)
            {
                case Monster.BabyPig:
                case Monster.Chick:
                case Monster.Kitten:
                case Monster.BabySkeleton:
                case Monster.Baekdon:
                case Monster.Wimaen:
                case Monster.BlackKitten:
                case Monster.BabyDragon:
                case Monster.OlympicFlame:
                case Monster.BabySnowMan:
                case Monster.Frog:
                case Monster.BabyMonkey:
                case Monster.AngryBird:
                case Monster.Foxey:
                case Monster.MedicalRat:
                    BodyLibrary = Mir2Res.Pets[((ushort)BaseImage) - 10000];
                    break;
            }

            if (ActionFeed.Count == 0)
            {
                CurrentAction = Stoned ? MirAction.Stoned : MirAction.Standing;
                if (CurrentAction == MirAction.Standing) CurrentAction = SitDown ? MirAction.SitDown : MirAction.Standing;

                Frames.TryGetValue(CurrentAction, out Frame);

                if (MapLocation != CurrentLocation)
                {
                    MapControl.Instance.RemoveObject(this);
                    MapLocation = CurrentLocation;
                    MapControl.Instance.AddObject(this);
                }

                FrameIndex = 0;

                if (Frame == null) return false;

                FrameInterval = Frame.Interval;
            }
            else
            {
                QueuedAction action = ActionFeed[0];
                ActionFeed.RemoveAt(0);

                CurrentActionLevel = 0;
                CurrentAction = action.Action;
                CurrentLocation = action.Location;
                Direction = action.Direction;

                Vector3Int temp;
                switch (CurrentAction)
                {
                    case MirAction.Walking:
                    case MirAction.Running:
                    case MirAction.Pushed:
                        int i = CurrentAction == MirAction.Running ? 2 : 1;
                        temp = Functions.PointMove(CurrentLocation, Direction, CurrentAction == MirAction.Pushed ? 0 : -i);
                        break;
                    case MirAction.Jump:
                    case MirAction.DashAttack:
                        temp = Functions.PointMove(CurrentLocation, Direction, JumpDistance);
                        break;
                    default:
                        temp = CurrentLocation;
                        break;
                }

                temp = new Vector3Int(action.Location.x, temp.y > CurrentLocation.y ? temp.y : CurrentLocation.y);

                if (MapLocation != temp)
                {
                    MapControl.Instance.RemoveObject(this);
                    MapLocation = temp;
                    MapControl.Instance.AddObject(this);
                }


                switch (CurrentAction)
                {
                    case MirAction.Pushed:
                        Frames.TryGetValue(MirAction.Walking, out Frame);
                        break;
                    case MirAction.Jump:
                        Frames.TryGetValue(MirAction.Jump, out Frame);
                        break;
                    case MirAction.DashAttack:
                        Frames.TryGetValue(MirAction.DashAttack, out Frame);
                        break;
                    case MirAction.AttackRange1:
                        if (!Frames.TryGetValue(CurrentAction, out Frame))
                            Frames.TryGetValue(MirAction.Attack1, out Frame);
                        break;
                    case MirAction.AttackRange2:
                        if (!Frames.TryGetValue(CurrentAction, out Frame))
                            Frames.TryGetValue(MirAction.Attack2, out Frame);
                        break;
                    case MirAction.AttackRange3:
                        if (!Frames.TryGetValue(CurrentAction, out Frame))
                            Frames.TryGetValue(MirAction.Attack3, out Frame);
                        break;
                    case MirAction.Special:
                        if (!Frames.TryGetValue(CurrentAction, out Frame))
                            Frames.TryGetValue(MirAction.Attack1, out Frame);
                        break;
                    case MirAction.Skeleton:
                        if (!Frames.TryGetValue(CurrentAction, out Frame))
                            Frames.TryGetValue(MirAction.Dead, out Frame);
                        break;
                    case MirAction.Hide:
                        switch (BaseImage)
                        {
                            case Monster.Shinsu1:
                                BodyLibrary = Mir2Res.Monsters[(ushort)Monster.Shinsu];
                                BaseImage = Monster.Shinsu;
                                BaseSound = (ushort)BaseImage * 10;
                                Frames = MLibraryMgr.Instance.AddOrGet(BodyLibrary).Frames ?? FrameSet.DefaultMonster;
                                Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                            default:
                                Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                        }
                        break;
                    case MirAction.Dead:
                        switch (BaseImage)
                        {
                            case Monster.Shinsu:
                            case Monster.Shinsu1:
                            case Monster.HolyDeva:
                            case Monster.GuardianRock:
                            case Monster.CharmedSnake://SummonSnakes
                            case Monster.HellKnight1:
                            case Monster.HellKnight2:
                            case Monster.HellKnight3:
                            case Monster.HellKnight4:
                            case Monster.HellBomb1:
                            case Monster.HellBomb2:
                            case Monster.HellBomb3:
                                Remove();
                                return false;
                            default:
                                Frames.TryGetValue(CurrentAction, out Frame);
                                break;
                        }
                        break;
                    default:
                        Frames.TryGetValue(CurrentAction, out Frame);
                        break;

                }

                FrameIndex = 0;

                if (Frame == null) return false;

                FrameInterval = Frame.Interval;

                Vector3Int front = Functions.PointMove(CurrentLocation, Direction, 1);

                switch (CurrentAction)
                {
                    case MirAction.Appear:
                        PlaySummonSound();
                        switch (BaseImage)
                        {
                            case Monster.HellKnight1:
                            case Monster.HellKnight2:
                            case Monster.HellKnight3:
                            case Monster.HellKnight4:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)BaseImage], 448, 10, 600, this));
                                break;
                        }
                        break;
                    case MirAction.Show:
                        PlayPopupSound();
                        break;
                    case MirAction.Pushed:
                        FrameIndex = Frame.Count - 1;
                       // GameScene.Scene.Redraw();
                        break;
                    case MirAction.Jump:
                        PlayJumpSound();
                        switch (BaseImage)
                        {
                            // Sanjian
                            case Monster.FurbolgGuard:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgGuard], 414 + (int)Direction * 6, 6, Frame.Count * Frame.Interval, this));
                                break;

                            case Monster.Armadillo:
                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)BaseImage], 592, 12, 800, CurrentLocation, CMain.Time + 500));
                                break;
                        }
                        break;
                    case MirAction.DashAttack:
                        PlayDashSound();
                        break;
                    case MirAction.Walking:
                        //GameScene.Scene.Redraw();
                        break;
                    case MirAction.Running:
                        PlayRunSound();
                        //GameScene.Scene.Redraw();
                        break;
                    case MirAction.Attack1:
                        PlayAttackSound();
                        CurrentActionLevel = (byte)action.Params[1];
                        switch (BaseImage)
                        {
                            case Monster.FlamingWooma:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlamingWooma], 224 + (int)Direction * 7, 7, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.ZumaTaurus:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ZumaTaurus], 244 + (int)Direction * 8, 8, 8 * FrameInterval, this));
                                break;
                            case Monster.MinotaurKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.MinotaurKing], 272 + (int)Direction * 6, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.FlamingMutant:
                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlamingMutant], 314, 6, 600, front));
                                break;
                            case Monster.DemonWolf:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DemonWolf], 336 + (int)Direction * 9, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.AncientBringer:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AncientBringer], 512 + (int)Direction * 6, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.Manticore:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Manticore], 505 + (int)Direction * 3, 3, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.YinDevilNode:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.YinDevilNode], 26, 26, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.YangDevilNode:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.YangDevilNode], 26, 26, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.GreatFoxSpirit:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], 355, 20, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.EvilMir:
                                Effects.Add(new Effect(Mir2Res.Dragon, 60, 8, 8 * Frame.Interval, this));
                                Effects.Add(new Effect(Mir2Res.Dragon, 68, 14, 14 * Frame.Interval, this));
                                byte random = (byte)CMain.Random.Next(7);
                                for (int i = 0; i <= 7 + random; i++)
                                {
                                    Vector3Int source = new Vector3Int(User.CurrentLocation.x + CMain.Random.Next(-7, 7), User.CurrentLocation.y + CMain.Random.Next(-7, 7));
                                    MapControl.Effects.Add(new Effect(Mir2Res.Dragon, 230 + (CMain.Random.Next(5) * 10), 5, 400, source, CMain.Time + CMain.Random.Next(1000)));
                                }
                                break;
                            case Monster.CrawlerLave:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CrawlerLave], 224 + (int)Direction * 6, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.HellKeeper:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellKeeper], 32, 8, 8 * Frame.Interval, this));
                                break;
                            case Monster.IcePillar:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePillar], 12, 6, 6 * 100, this));
                                break;
                            case Monster.TrollKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TrollKing], 288, 6, 6 * Frame.Interval, this));
                                break;
                            case Monster.HellBomb1:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 61, 7, 7 * Frame.Interval, this));
                                break;
                            case Monster.HellBomb2:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 79, 9, 9 * Frame.Interval, this));
                                break;
                            case Monster.HellBomb3:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellLord], 97, 8, 8 * Frame.Interval, this));
                                break;
                            case Monster.WitchDoctor:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WitchDoctor], 328, 20, 20 * Frame.Interval, this));
                                break;
                            case Monster.SeedingsGeneral:
                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 1064 + (int)Direction * 9, 9, 9 * Frame.Interval, front, CMain.Time));
                                break;
                            case Monster.RestlessJar:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RestlessJar], 384, 7, 7 * Frame.Interval, this));
                                break;
                            case Monster.AssassinBird:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AssassinBird], 392, 9, 9 * Frame.Interval, this));
                                break;
                            case Monster.Nadz:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Nadz], 280 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                break;
                            case Monster.AvengingWarrior:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengingWarrior], 272 + (int)Direction * 5, 5, 7 * Frame.Interval, this));
                                break;
                            case Monster.FlyingStatue:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlyingStatue], 334, 10, 10 * 100, this));
                                break;
                            case Monster.HoodedSummoner:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HoodedSummoner], 352, 12, 12 * Frame.Interval, this));
                                break;
                        }
                        break;
                    case MirAction.Attack2:
                        PlaySecondAttackSound();
                        CurrentActionLevel = (byte)action.Params[1];
                        switch (BaseImage)
                        {
                            case Monster.CrystalSpider:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CrystalSpider], 272 + (int)Direction * 10, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.Yimoogi:
                            case Monster.RedYimoogi:
                            case Monster.Snake10:
                            case Monster.Snake11:
                            case Monster.Snake12:
                            case Monster.Snake13:
                            case Monster.Snake14:
                            case Monster.Snake15:
                            case Monster.Snake16:
                            case Monster.Snake17:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)BaseImage], 304, 6, Frame.Count * Frame.Interval, this));
                                Effects.Add(new Effect(Mir2Res.Magic2, 1280, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.HellCannibal:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellCannibal], 310 + (int)Direction * 12, 12, 12 * Frame.Interval, this));
                                break;
                            case Monster.ManectricKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManectricKing], 640 + (int)Direction * 10, 10, 10 * 100, this));
                                break;
                            case Monster.AncientBringer:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AncientBringer], 568 + (int)Direction * 10, 10, 13 * Frame.Interval, this));
                                break;
                            case Monster.DarkBeast:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkBeast], 296 + (int)Direction * 4, 4, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.LightBeast:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.LightBeast], 296 + (int)Direction * 4, 4, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.FireCat:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FireCat], 248 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.BloodBaboon:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BloodBaboon], 312 + (int)Direction * 7, 7, 7 * Frame.Interval, this));
                                break;
                            case Monster.TwinHeadBeast:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TwinHeadBeast], 352 + (int)Direction * 7, 7, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.Nadz:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Nadz], 320 + (int)Direction * 7, 7, 7 * Frame.Interval, this));
                                break;
                            case Monster.DarkCaptain:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1214, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.DragonWarrior:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DragonWarrior], 576, 7, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.FlyingStatue:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlyingStatue], 352, 10, 10 * 100, this));
                                break;
                            case Monster.HornedSorceror:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedSorceror], 552 + (int)Direction * 9, 9, 900, this));
                                break;
                            case Monster.HoodedSummoner:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HoodedSummoner], 364, 10, 10 * Frame.Interval, this));
                                break;
                        }

                        if ((ushort)BaseImage >= 10000)
                        {
                            PlayPetSound();
                        }
                        break;
                    case MirAction.Attack3:
                        PlayThirdAttackSound();
                        CurrentActionLevel = (byte)action.Params[1];
                        switch (BaseImage)
                        {
                            case Monster.Yimoogi:
                            case Monster.RedYimoogi:
                            case Monster.Snake10:
                            case Monster.Snake11:
                            case Monster.Snake12:
                            case Monster.Snake13:
                            case Monster.Snake14:
                            case Monster.Snake15:
                            case Monster.Snake16:
                            case Monster.Snake17:
                                AudioMgr.Instance.PlaySound(BaseSound + 9);
                                Effects.Add(new Effect(Mir2Res.Magic2, 1330, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.Behemoth:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Behemoth], 697 + (int)Direction * 7, 7, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.SandSnail:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SandSnail], 448, 9, 900, this) { Blend = true });
                                break;
                            case Monster.PeacockSpider:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PeacockSpider], 755, 21, 21 * Frame.Interval, this));
                                break;
                            case Monster.DragonWarrior:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DragonWarrior], 632 + (int)Direction * 4, 4, 8 * Frame.Interval, this));
                                break;
                        }
                        break;
                    case MirAction.Attack4:
                        PlayFourthAttackSound();
                        CurrentActionLevel = (byte)action.Params[1];
                        switch (BaseImage)
                        {
                            case Monster.DarkOmaKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], 1702, 13, 13 * Frame.Interval, this));
                                break;
                        }
                        break;
                    case MirAction.Attack5:
                        PlayFithAttackSound();
                        CurrentActionLevel = (byte)action.Params[1];
                        break;
                    case MirAction.AttackRange1:
                        PlayRangeSound();
                        TargetID = (uint)action.Params[0];
                        CurrentActionLevel = (byte)action.Params[4];
                        switch (BaseImage)
                        {
                            case Monster.KingScorpion:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingScorpion], 544 + (int)Direction * 8, 8, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.DarkDevil:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkDevil], 272 + (int)Direction * 8, 8, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.ShamanZombie:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ShamanZombie], 232 + (int)Direction * 12, 6, Frame.Count * Frame.Interval, this));
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ShamanZombie], 328, 12, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.GuardianRock:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GuardianRock], 12, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.GreatFoxSpirit:
                                byte random = (byte)CMain.Random.Next(4);
                                for (int i = 0; i <= 4 + random; i++)
                                {
                                    Vector3Int source = new Vector3Int(User.CurrentLocation.x + CMain.Random.Next(-7, 7), User.CurrentLocation.y + CMain.Random.Next(-7, 7));
                                    MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], 375 + (CMain.Random.Next(3) * 20), 20, 1400, source, CMain.Time + CMain.Random.Next(600)));
                                }
                                break;
                            case Monster.EvilMir:
                                Effects.Add(new Effect(Mir2Res.Dragon, 90 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.DragonStatue:
                                Effects.Add(new Effect(Mir2Res.Dragon, 310 + ((int)Direction / 3) * 20, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.TurtleKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TurtleKing], 946, 10, Frame.Count * Frame.Interval, User));
                                break;
                            case Monster.HellBolt:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellBolt], 304, 11, 11 * 100, this));
                                break;
                            case Monster.WitchDoctor:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WitchDoctor], 304, 9, 9 * 100, this));
                                break;
                            case Monster.FlyingStatue:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlyingStatue], 304, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.ManectricKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManectricKing], 720, 12, 12 * 100, this));
                                break;
                            case Monster.IcePillar:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePillar], 26, 6, 8 * 100, this) { Start = CMain.Time + 750 });
                                break;
                            case Monster.ElementGuard:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ElementGuard], 320 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                break;
                            case Monster.KingGuard:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingGuard], 737, 9, 9 * Frame.Interval, this));
                                break;
                            case Monster.CatShaman:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CatShaman], 738, 8, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.SeedingsGeneral:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 1184 + (int)Direction * 9, 9, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.RestlessJar:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RestlessJar], 471, 5, 500, this));
                                break;
                            case Monster.AvengingSpirit:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengingSpirit], 344 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                break;
                            case Monster.ClawBeast:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ClawBeast], 504 + (int)Direction * 8, 8, 8 * Frame.Interval, this));
                                break;
                            case Monster.DarkCaptain:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1200, 13, 13 * Frame.Interval, this));
                                break;
                            case Monster.FloatingRock:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FloatingRock], 216, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.FrozenKnight:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenKnight], 384 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                break;
                            case Monster.IcePhantom:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePhantom], 672, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.BlackTortoise:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackTortoise], 404 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                break;
                            case Monster.AssassinScroll:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AssassinScroll], 296, 3, 3 * Frame.Interval, this));
                                break;
                            case Monster.PurpleFaeFlower:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PurpleFaeFlower], 328, 3, 3 * Frame.Interval, this));
                                break;
                            case Monster.WarriorScroll:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WarriorScroll], 296, 8, 8 * Frame.Interval, this));
                                break;
                            case Monster.WizardScroll:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WizardScroll], 296, 4, 4 * Frame.Interval, this));
                                break;
                        }
                        break;
                    case MirAction.AttackRange2:
                        PlaySecondRangeSound();
                        TargetID = (uint)action.Params[0];
                        CurrentActionLevel = (byte)action.Params[4];
                        switch (BaseImage)
                        {
                            case Monster.TurtleKing:
                                byte random = (byte)CMain.Random.Next(4);
                                for (int i = 0; i <= 4 + random; i++)
                                {
                                    Vector3Int source = new Vector3Int(User.CurrentLocation.x + CMain.Random.Next(-7, 7), User.CurrentLocation.y + CMain.Random.Next(-7, 7));

                                    Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.TurtleKing], CMain.Random.Next(2) == 0 ? 922 : 934, 12, 1200, source, CMain.Time + CMain.Random.Next(600));
                                    ef.Played += (o, e) => AudioMgr.Instance.PlaySound(20000 + (ushort)Spell.HellFire * 10 + 1);
                                    MapControl.Effects.Add(ef);
                                }
                                break;
                            case Monster.SeedingsGeneral:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 1256, 9, 900, this));
                                break;
                            case Monster.PeacockSpider: //BROKEN
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PeacockSpider], 776, 30, 30 * Frame.Interval, this));
                                break;
                            case Monster.DarkCaptain:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1234, 13, 13 * Frame.Interval, this));
                                break;
                            case Monster.IcePhantom:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePhantom], 672, 10, 10 * Frame.Interval, this));
                                break;
                        }
                        break;
                    case MirAction.AttackRange3:
                        PlayThirdRangeSound();
                        TargetID = (uint)action.Params[0];
                        CurrentActionLevel = (byte)action.Params[4];
                        switch (BaseImage)
                        {
                            case Monster.TurtleKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TurtleKing], 946, 10, Frame.Count * Frame.Interval, User));
                                break;
                        }
                        break;
                    case MirAction.Struck:
                        uint attackerID = (uint)action.Params[0];
                        StruckWeapon = -2;
                        if (action.Params.Count > 1)
                        {
                            StruckWeapon = (int)action.Params[1];
                        }
                        else if (MapControl.Objects.TryGetValue(attackerID, out MapObject ob))
                        {
                            if (ob.Race == ObjectType.Player)
                            {
                                PlayerObject player = (PlayerObject)ob;
                                StruckWeapon = player.Weapon;
                                if (player.Class == MirClass.Assassin && StruckWeapon > -1)
                                    StruckWeapon = 1;
                            }
                        }
                        PlayFlinchSound();
                        PlayStruckSound();

                        switch (BaseImage)
                        {
                            case Monster.GlacierBeast:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GlacierBeast], 304, 6, 400, this));
                                break;
                        }
                        break;
                    case MirAction.Die:
                        switch (BaseImage)
                        {
                            case Monster.ManectricKing:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManectricKing], 504 + (int)Direction * 9, 9, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.DarkDevil:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkDevil], 336, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.ShamanZombie:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ShamanZombie], 224, 8, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.RoninGhoul:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RoninGhoul], 224, 10, Frame.Count * FrameInterval, this));
                                break;
                            case Monster.BoneCaptain:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BoneCaptain], 224 + (int)Direction * 10, 10, Frame.Count * FrameInterval, this));
                                break;
                            case Monster.RightGuard:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RightGuard], 296, 5, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.LeftGuard:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.LeftGuard], 296 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                break;
                            case Monster.FrostTiger:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrostTiger], 304, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.Yimoogi:
                            case Monster.RedYimoogi:
                            case Monster.Snake10:
                            case Monster.Snake11:
                            case Monster.Snake12:
                            case Monster.Snake13:
                            case Monster.Snake14:
                            case Monster.Snake15:
                            case Monster.Snake16:
                            case Monster.Snake17:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Yimoogi], 352, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.YinDevilNode:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.YinDevilNode], 52, 20, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.YangDevilNode:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.YangDevilNode], 52, 20, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.BlackFoxman:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackFoxman], 224, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.VampireSpider:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.VampireSpider], 296, 5, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.CharmedSnake:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CharmedSnake], 40, 8, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.Manticore:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Manticore], 592, 9, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.ValeBat:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ValeBat], 224, 20, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.SpiderBat:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SpiderBat], 224, 20, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.VenomWeaver:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.VenomWeaver], 224, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.HellBolt:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellBolt], 325, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.SabukGate:
                                Effects.Add(new Effect(Mir2Res.Gates[(ushort)Monster.SabukGate - 950], 24, 10, Frame.Count * Frame.Interval, this) { Light = -1 });
                                break;
                            case Monster.WingedTigerLord:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WingedTigerLord], 650 + (int)Direction * 5, 5, Frame.Count * FrameInterval, this));
                                break;
                            case Monster.HellKnight1:
                            case Monster.HellKnight2:
                            case Monster.HellKnight3:
                            case Monster.HellKnight4:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)BaseImage], 448, 10, 600, this));
                                break;
                            case Monster.IceGuard:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IceGuard], 256, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.DeathCrawler:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DeathCrawler], 313, 11, Frame.Count * Frame.Interval, this));
                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DeathCrawler], 304, 9, 900, CurrentLocation, CMain.Time + 900));
                                break;
                            case Monster.BurningZombie:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BurningZombie], 373, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.FrozenZombie:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenZombie], 360, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.EarthGolem:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.EarthGolem], 432, 9, 9 * Frame.Interval, this));
                                break;
                            case Monster.CreeperPlant:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CreeperPlant], 266, 6, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.SackWarrior:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SackWarrior], 384, 9, Frame.Count * Frame.Interval, this));
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SackWarrior], 393, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.FrozenSoldier:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenSoldier], 256, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.FrozenGolem:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenGolem], 456, 7, Frame.Count * Frame.Interval, this));
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenGolem], 463, 10, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.DragonWarrior:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DragonWarrior], 504 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                break;
                            case Monster.FloatingRock:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FloatingRock], 152, 8, Frame.Count * Frame.Interval, this));
                                break;
                            case Monster.AvengingSpirit:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengingSpirit], 442 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                break;
                            case Monster.TaoistScroll:
                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TaoistScroll], 282, 11, 11 * Frame.Interval, this));
                                break;
                        }
                        PlayDieSound();
                        break;
                    case MirAction.Dead:
                       // GameScene.Scene.Redraw();
                        MapControl.Instance.SortObject(this);
                        if (MouseObject == this) MouseObjectID = 0;
                        if (TargetObject == this) TargetObjectID = 0;
                        if (MagicObject == this) MagicObjectID = 0;

                        for (int i = 0; i < Effects.Count; i++)
                            Effects[i].Remove();

                        DeadTime = CMain.Time;

                        break;
                }
            }

            MapControl.Instance.TextureValid = false;
            NextMotion = CMain.Time + FrameInterval;
            return true;
        }

        public void SetCurrentEffects()
        {
            //BindingShot
            if (BindingShotCenter && ShockTime > CMain.Time)
            {
                int effectid = TrackableEffect.GetOwnerEffectID(ObjectID);
                if (effectid >= 0)
                    TrackableEffect.effectlist[effectid].RemoveNoComplete();

                TrackableEffect NetDropped = new TrackableEffect(new Effect(Mir2Res.MagicC, 7, 1, 1000, this) { Repeat = true, RepeatUntil = (ShockTime - 1500) });
                NetDropped.Complete += (o1, e1) =>
                {
                    AudioMgr.Instance.PlaySound(20000 + 130 * 10 + 6);//sound M130-6
                    Effects.Add(new TrackableEffect(new Effect(Mir2Res.MagicC, 8, 8, 700, this)));
                };
                Effects.Add(NetDropped);
            }
            else if (BindingShotCenter && ShockTime <= CMain.Time)
            {
                int effectid = TrackableEffect.GetOwnerEffectID(ObjectID);
                if (effectid >= 0)
                    TrackableEffect.effectlist[effectid].Remove();

                //AudioMgr.Instance.PlaySound(20000 + 130 * 10 + 6);//sound M130-6
                //Effects.Add(new TrackableEffect(new Effect(Mir2Res.ArcherMagic, 8, 8, 700, this)));

                ShockTime = 0;
                BindingShotCenter = false;
            }

        }


        private void ProcessFrames()
        {
            if (Frame == null) return;

            switch (CurrentAction)
            {
                case MirAction.Walking:
                    if (!GameScene.CanMove) return;

                    MapControl.Instance.TextureValid = false;

                    if (SkipFrames) UpdateFrame();

                    if (UpdateFrame() >= Frame.Count)
                    {
                        FrameIndex = Frame.Count - 1;
                        SetAction();
                    }
                    else
                    {
                        switch (FrameIndex)
                        {
                            case 1:
                                PlayWalkSound(true);
                                break;
                            case 4:
                                PlayWalkSound(false);
                                break;
                        }
                    }
                    break;
                case MirAction.Running:
                    if (!GameScene.CanMove) return;

                    MapControl.Instance.TextureValid = false;

                    if (SkipFrames) UpdateFrame();

                    if (UpdateFrame() >= Frame.Count)
                    {
                        FrameIndex = Frame.Count - 1;
                        SetAction();
                    }
                    break;
                case MirAction.Pushed:
                    if (!GameScene.CanMove) return;

                    MapControl.Instance.TextureValid = false;

                    FrameIndex -= 2;

                    if (FrameIndex < 0)
                    {
                        FrameIndex = 0;
                        SetAction();
                    }
                    break;
                case MirAction.Jump:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.DashAttack:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;
                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.HornedSorceror:
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedSorceror], 644 + (int)Direction * 5, 5, 500, CurrentLocation));
                                                break;
                                        }
                                    }
                                    break;
                            }
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Show:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            switch (BaseImage)
                            {
                                case Monster.ZumaStatue:
                                case Monster.ZumaGuardian:
                                case Monster.RedThunderZuma:
                                case Monster.FrozenRedZuma:
                                case Monster.FrozenZumaStatue:
                                case Monster.FrozenZumaGuardian:
                                case Monster.ZumaTaurus:
                                case Monster.DemonGuard:
                                case Monster.EarthGolem:
                                case Monster.Turtlegrass:
                                case Monster.ManTree:
                                case Monster.AssassinScroll:
                                case Monster.WarriorScroll:
                                case Monster.TaoistScroll:
                                case Monster.WizardScroll:
                                case Monster.PurpleFaeFlower:
                                    Stoned = false;
                                    break;
                                case Monster.Shinsu:
                                    BodyLibrary = Mir2Res.Monsters[(ushort)Monster.Shinsu1];
                                    BaseImage = Monster.Shinsu1;
                                    BaseSound = (ushort)BaseImage * 10;
                                    Frames = MLibraryMgr.Instance.AddOrGet(BodyLibrary).Frames ?? FrameSet.DefaultMonster;
                                    break;
                            }

                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 1:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.CreeperPlant:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CreeperPlant], 250, 6, 6 * 100, this));
                                                break;
                                        }
                                        break;
                                    }
                            }

                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Hide:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            switch (BaseImage)
                            {
                                case Monster.CannibalPlant:
                                case Monster.WaterDragon:
                                case Monster.CreeperPlant:
                                case Monster.EvilCentipede:
                                case Monster.DigOutZombie:
                                case Monster.Armadillo:
                                case Monster.ArmadilloElder:
                                    Remove();
                                    return;
                                case Monster.ZumaStatue:
                                case Monster.ZumaGuardian:
                                case Monster.RedThunderZuma:
                                case Monster.FrozenRedZuma:
                                case Monster.FrozenZumaStatue:
                                case Monster.FrozenZumaGuardian:
                                case Monster.ZumaTaurus:
                                case Monster.DemonGuard:
                                case Monster.EarthGolem:
                                case Monster.Turtlegrass:
                                case Monster.ManTree:
                                case Monster.AssassinScroll:
                                case Monster.WarriorScroll:
                                case Monster.TaoistScroll:
                                case Monster.WizardScroll:
                                case Monster.PurpleFaeFlower:
                                    Stoned = true;
                                    return;
                            }


                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Appear:
                case MirAction.Standing:
                case MirAction.Stoned:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            if (CurrentAction == MirAction.Standing)
                            {
                                switch (BaseImage)
                                {
                                    case Monster.SnakeTotem://SummonSnakes Totem
                                        if (TrackableEffect.GetOwnerEffectID(this.ObjectID, "SnakeTotem") < 0)
                                            Effects.Add(new TrackableEffect(new Effect(Mir2Res.Monsters[(ushort)Monster.SnakeTotem], 2, 10, 1500, this) { Repeat = true }, "SnakeTotem"));
                                        break;
                                    case Monster.PalaceWall1:
                                        //Effects.Add(new Effect(Mir2Res.Effect, 196, 1, 1000, this) { DrawBehind = true, d });
                                        //Mir2Res.Effect.Draw(196, DrawLocation, Color.White, true);
                                        //Mir2Res.Effect.DrawBlend(196, DrawLocation, Color.White, true);
                                        break;
                                }
                            }

                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Attack1:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;
                        Vector3Int front = Functions.PointMove(CurrentLocation, Direction, 1);

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            if (SetAction())
                            {
                                switch (BaseImage)
                                {
                                    case Monster.EvilCentipede:
                                        Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.EvilCentipede], 42, 10, 600, this));
                                        break;
                                    case Monster.ToxicGhoul:
                                        AudioMgr.Instance.PlaySound(BaseSound + 4);
                                        Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ToxicGhoul], 224 + (int)Direction * 6, 6, 600, this));
                                        break;
                                }
                            }
                        }
                        else
                        {
                            switch (FrameIndex)
                            {                                
                                case 1:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.Kirin:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Kirin], 776 + (int)Direction * 5, 5, 500, this));
                                                break;
                                            case Monster.Bear:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Bear], 321 + (int)Direction * 4, 4, Frame.Count * Frame.Interval, this));
                                                break;
                                            case Monster.Jar1:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Jar1], 128 + (int)Direction * 3, 3, 300, this));
                                                break;
                                            case Monster.Jar2:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Jar2], 624 + (int)Direction * 8, 8, 800, this));
                                                break;
                                            case Monster.AntCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AntCommander], 368 + (int)Direction * 8, 8, 8 * Frame.Interval, this));
                                                break;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.FurbolgCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgCommander], 325 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;



                                            case Monster.StainHammerCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StainHammerCat], 240 + (int)Direction * 4, 4, Frame.Count * Frame.Interval, this));
                                                break;
                                            case Monster.HornedWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedWarrior], 752 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.FrozenFighter:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenFighter], 336 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.PurpleFaeFlower: // Slash effect on mob
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PurpleFaeFlower], 436 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        PlaySwingSound();
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.Furball:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Furball], 256 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;

                                            case Monster.FurbolgWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgWarrior], 320 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;





                                            case Monster.RightGuard:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RightGuard], 272 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.LeftGuard:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.LeftGuard], 272 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.Shinsu1:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Shinsu1], 224 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.DeathCrawler:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DeathCrawler], 248 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.FrozenZombie:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenZombie], 312 + (int)Direction * 5, 5, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.TucsonWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TucsonWarrior], 296 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.ArmadilloElder:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ArmadilloElder], 488 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.Mandrill:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Mandrill], 264 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.ArmedPlant:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ArmedPlant], 256 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.AvengerPlant:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengerPlant], 224 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.AvengingSpirit:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengingSpirit], 280 + (int)Direction * 8, 8, 8 * Frame.Interval, this));
                                                break;
                                            case Monster.SackWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SackWarrior], 344 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.Bear:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Bear], 321 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;
                                            case Monster.FrozenKnight:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenKnight], 360 + (int)Direction * 3, 3, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.IcePhantom:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePhantom], 640 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;
                                            case Monster.DragonWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DragonWarrior], 552 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.BurningZombie:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BurningZombie], 312 + (int)Direction * 5, 5, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.DarkCaptain:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1168 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.BlackTortoise:
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackTortoise], 360, 6, 6 * Frame.Interval, front, CMain.Time));
                                                break;
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.GeneralMeowMeow:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GeneralMeowMeow], 416 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;
                                            case Monster.Armadillo:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Armadillo], 480 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.StainHammerCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StainHammerCat], 272 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.StrayCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StrayCat], 528 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;
                                            case Monster.OmaSlasher:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaSlasher], 304 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;
                                            case Monster.AvengerPlant:
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengerPlant], 248, 6, 600, front, CMain.Time));
                                                break;
                                            case Monster.ManTree:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManTree], 472 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.HornedMage:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedMage], 783, 9, 800, this));
                                                break;
                                            case Monster.DarkWraith:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkWraith], 720 + (int)Direction * 3, 3, 300, this));                                                
                                                Effect darkWraithEffect = new Effect(Mir2Res.Monsters[(ushort)Monster.DarkWraith], 744, 6, 600, front, CMain.Time);
                                                MapControl.Effects.Add(darkWraithEffect);
                                                break;
                                            case Monster.FightingCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FightingCat], 208 + (int)Direction * 3, 3, 4 * Frame.Interval, this) { Blend = true });
                                                break;
                                            case Monster.FlamingMutant:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlamingMutant], 304, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.DarkOmaKing:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], 1568 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;
                                            case Monster.WaterDragon:
                                                Effect waterDragonEffect = new Effect(Mir2Res.Monsters[(ushort)Monster.WaterDragon], 905, 9, 900, front, CMain.Time + 300);
                                                MapControl.Effects.Add(waterDragonEffect); 
                                                break;
                                            case Monster.PurpleFaeFlower: // Animation on the target.
                                                Effect purpleFaeFlowerEffect = new Effect(Mir2Res.Monsters[(ushort)Monster.PurpleFaeFlower], 483, 7, 700, front, CMain.Time + 300);
                                                MapControl.Effects.Add(purpleFaeFlowerEffect);
                                                break;
                                        }
                                    }
                                    break;
                                case 5:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.FurbolgGuard:
                                                MapObject ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgGuard], 384, 7, 600, ob));
                                                break;



                                            case Monster.FlyingStatue:
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlyingStatue], 344, 8, 800, front, CMain.Time));
                                                break;
                                            case Monster.OmaAssassin:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaAssassin], 312 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;
                                            case Monster.PlagueCrab:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PlagueCrab], 488 + (int)Direction * 7, 7, 7 * Frame.Interval, this));
                                                break;
                                            case Monster.ScalyBeast:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ScalyBeast], 344 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.HornedSorceror:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedSorceror], 536 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.SnowWolfKing:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowWolfKing], 456 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.FrozenAxeman:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenAxeman], 528 + (int)Direction * 3, 3, 300, this));
                                                break;
                                            case Monster.FrozenMagician:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMagician], 840 + (int)Direction * 4, 4, 400, this));
                                                Effect frozenMagicianEffect = new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMagician], 872, 6, 600, front, CMain.Time + 300);
                                                MapControl.Effects.Add(frozenMagicianEffect);
                                                break;
                                        }
                                        break;
                                    }
                                case 6:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.FrozenMiner:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMiner], 432 + (int)Direction * 3, 3, 300, this));
                                                Vector3Int source = Functions.PointMove(CurrentLocation, Direction, 1);
                                                Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMiner], 456, 6, 600, source, CMain.Time + 300);
                                                MapControl.Effects.Add(ef);
                                                break;
                                            case Monster.SnowYeti:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowYeti], 504 + (int)Direction * 4, 4, 400, this));
                                                break;
                                            case Monster.IceCrystalSoldier:
                                                source = Functions.PointMove(CurrentLocation, Direction, 1);
                                                ef = new Effect(Mir2Res.Monsters[(ushort)Monster.IceCrystalSoldier], 464, 6, 600, source, CMain.Time);
                                                MapControl.Effects.Add(ef);
                                                break;
                                        }
                                        break;
                                    }
                                case 7:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.AxePlant:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AxePlant], 256 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                                break;
                                            case Monster.TreeQueen://Fire Bombardment
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TreeQueen], 35, 13, 1300, this) { Blend = true });
                                                break;
                                            case Monster.HornedCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 784 + (int)Direction * 3, 3, 300, this) { Blend = true });
                                                break;
                                        }
                                        break;
                                    }
                                case 8:
                                    {
                                        MapObject ob = MapControl.GetObject(TargetID);

                                        switch (BaseImage)
                                        {
                                            case Monster.FurbolgCommander:
                                                if (ob != null)
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgCommander], 320, 5, 500, ob));
                                                break;
                                        }
                                        break;
                                    }
                                case 9:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.SeedingsGeneral:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 736 + (int)Direction * 9, 9, 900, this));
                                                break;
                                        }
                                        break;
                                    }
                            }
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.SitDown:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Attack2:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;
                        if (SkipFrames) UpdateFrame();
                        Vector3Int front = Functions.PointMove(CurrentLocation, Direction, 1);

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 1:
                                    {                                        
                                        switch (BaseImage)
                                        {
                                            case Monster.BabySnowMan:
                                                if (FrameIndex == 1)
                                                {
                                                    if (TrackableEffect.GetOwnerEffectID(this.ObjectID, "SnowmanSnow") < 0)
                                                        Effects.Add(new TrackableEffect(new Effect(Mir2Res.Pets[((ushort)BaseImage) - 10000], 208, 11, 1500, this), "SnowmanSnow"));
                                                }
                                                break;
                                            case Monster.CannibalTentacles:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CannibalTentacles], 400 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                                break;
                                            case Monster.DarkOmaKing:
                                                AudioMgr.Instance.PlaySound(BaseSound + 6, false, 800);
                                                AudioMgr.Instance.PlaySound(BaseSound + 6, false, 1500);
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], 1600, 30, 30 * Frame.Interval, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 2:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.FurbolgWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgWarrior], 360 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;


                                            case Monster.BlackHammerCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 648 + (int)Direction * 11, 11, 11 * Frame.Interval, this));
                                                break;
                                            case Monster.HornedCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 1142 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.SnowWolfKing:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowWolfKing], 480, 9, 9 * Frame.Interval, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 3:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.GlacierBeast:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GlacierBeast], 310 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                break;



                                            case Monster.KingGuard:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingGuard], 773, 10, 1000, this) { Blend = true });
                                                break;
                                            case Monster.Behemoth:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Behemoth], 768, 10, Frame.Count * Frame.Interval, this));
                                                break;
                                            case Monster.FlameQueen:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlameQueen], 720, 9, Frame.Count * Frame.Interval, this));
                                                break;
                                            case Monster.DemonGuard:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DemonGuard], 288 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.TucsonMage:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TucsonMage], 296 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                                break;
                                            case Monster.Armadillo:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Armadillo], 496 + (int)Direction * 12, 12, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.CatWidow:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CatWidow], 256 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.SnowWolf:                                                
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowWolf], 328, 9, Frame.Count * Frame.Interval, this));
                                                break;
                                            case Monster.BlackTortoise:                                                
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackTortoise], 366 + (int)Direction * 4, 4, 4 * Frame.Interval, this));
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackTortoise], 398, 7, 7 * Frame.Interval, front, CMain.Time));
                                                break;
                                            case Monster.TreeQueen:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TreeQueen], 66, 16, 16 * Frame.Interval, this));
                                                break;
                                            case Monster.RhinoWarrior:
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RhinoWarrior], 376, 8, 800, front, CMain.Time));
                                                break;
                                            case Monster.FrozenFighter:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenFighter], 384 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.GlacierSnail:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GlacierSnail], 344 + (int)Direction * 5, 5, 5 * Frame.Interval, this));
                                                break;





                                            case Monster.DemonWolf:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DemonWolf], 312 + (int)Direction * 3, 3, 300, this));
                                                break;
                                            case Monster.TucsonWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TucsonWarrior], 344 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                                break;
                                            case Monster.PeacockSpider:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PeacockSpider], 592 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                                break;
                                            case Monster.SackWarrior:                                                
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SackWarrior], 368 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.ManTree:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManTree], 488 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                            case Monster.DarkWraith:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkWraith], 750 + (int)Direction * 5, 5, 500, this));                                                
                                                Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.DarkWraith], 790, 6, 600, front, CMain.Time);
                                                MapControl.Effects.Add(ef);
                                                break;
                                            case Monster.HornedWarrior:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedWarrior], 832 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                                break;
                                            case Monster.Turtlegrass:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Turtlegrass], 360 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.AntCommander:
                                                Effect ef1 = new Effect(Mir2Res.Monsters[(ushort)Monster.AntCommander], 484, 6, 600, front, CMain.Time);
                                                MapControl.Effects.Add(ef1);
                                                break;
                                        }
                                        break;
                                    }
                                case 5:
                                    {   
                                        switch (BaseImage)
                                        {
                                            case Monster.GeneralMeowMeow:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GeneralMeowMeow], 456 + (int)Direction * 7, 7, 7 * Frame.Interval, this) { Blend = true });
                                                break;
                                            case Monster.TucsonGeneral:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TucsonGeneral], 544, 8, 8 * Frame.Interval, this));
                                                break;
                                            case Monster.FrozenMiner:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMiner], 462 + (int)Direction * 5, 5, 500, this));
                                                break;
                                            case Monster.IceCrystalSoldier:                                                
                                                Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.IceCrystalSoldier], 470, 6, 600, front, CMain.Time);
                                                MapControl.Effects.Add(ef);
                                                break;
                                            case Monster.Bear:
                                                Effect bleedEffect = new Effect(Mir2Res.Monsters[(ushort)Monster.Bear], 312, 9, 900, front, CMain.Time);
                                                MapControl.Effects.Add(bleedEffect);
                                                break;
                                            case Monster.FlyingStatue:
                                                Effect flyingStatueEffect1 = new Effect(Mir2Res.Monsters[(ushort)Monster.FlyingStatue], 362, 8, 800, front, CMain.Time);
                                                MapControl.Effects.Add(flyingStatueEffect1);
                                                break;
                                            case Monster.HornedCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 784 + (int)Direction * 3, 3, 300, this) { Blend = true });
                                                break;
                                        }
                                        break;

                                    }
                                case 6:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.SeedingsGeneral:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 1136 + (int)Direction * 6, 6, 600, this));
                                                break;
                                            case Monster.OmaBlest:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaBlest], 392 + (int)Direction * 5, 5, 500, this));
                                                break;
                                            case Monster.WereTiger:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WereTiger], 344 + (int)Direction * 6, 6, 600, this));
                                                break;
                                            case Monster.Kirin:
                                                Vector3Int source2 = Functions.PointMove(CurrentLocation, Direction, 2);
                                                Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.Kirin], 816, 8, 800, source2, CMain.Time);
                                                MapControl.Effects.Add(ef);
                                                break;
                                            case Monster.FrozenAxeman:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenAxeman], 558 + (int)Direction * 3, 3, 300, this));
                                                break;
                                            case Monster.SnowYeti:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowYeti], 536 + (int)Direction * 3, 3, 300, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 7:
                                    {   
                                        switch (BaseImage)
                                        {
                                            case Monster.ElephantMan:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ElephantMan], 368, 9, 900, this) { Blend = true });
                                                break;
                                            case Monster.ScalyBeast:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ScalyBeast], 368 + (int)Direction * 3, 3, 3 * Frame.Interval, this) { Blend = false });
                                                break;
                                        }
                                    }
                                    break;
                                case 8:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.StrayCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StrayCat], 584 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 9:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.ScalyBeast:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ScalyBeast], 392, 3, 300, this) { Blend = true });
                                                break;
                                        }
                                    }
                                    break;
                                case 10:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.StoningStatue:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StoningStatue], 642, 15, 15 * 100, this));
                                                AudioMgr.Instance.PlaySound(BaseSound + 7);
                                                break;
                                        }
                                    }
                                    break;
                                case 11:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.BlackHammerCat:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 736 + (int)Direction * 8, 8, 800, this));
                                                break;
                                        }
                                    }
                                    break;
                                case 19:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.StoningStatue:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StoningStatue], 624, 8, 8 * 100, this));
                                                break;
                                        }
                                    }
                                    break;
                            }
                            if (FrameIndex == 3) PlaySwingSound();
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Attack3:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 1:
                                    switch (BaseImage)
                                    {
                                        case Monster.OlympicFlame:
                                            if (TrackableEffect.GetOwnerEffectID(this.ObjectID, "CreatureFlame") < 0)
                                                Effects.Add(new TrackableEffect(new Effect(Mir2Res.Pets[((ushort)BaseImage) - 10000], 280, 4, 800, this), "CreatureFlame"));
                                            break;
                                        case Monster.GasToad:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GasToad], 440, 9, 9 * Frame.Interval, this));
                                            break;
                                        case Monster.HornedCommander:
                                            {
                                                //Spin
                                                int loops = CurrentActionLevel;
                                                int duration = 7 * FrameInterval;
                                                int totalDuration = loops * duration;

                                                if (FrameLoop == null)
                                                {
                                                    for (int i = 0; i < loops; i++)
                                                    {
                                                        AudioMgr.Instance.PlaySound(8451, false, 0 + (i * duration));
                                                    }

                                                    Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 808 + (int)Direction * 7, 7, duration, this) { Repeat = true, RepeatUntil = CMain.Time + totalDuration });
                                                }

                                                LoopFrame(FrameIndex, 3, FrameInterval, totalDuration);

                                            }
                                            break;
                                        case Monster.SnowWolfKing:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowWolfKing], 489 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                            break;
                                    }
                                    break;
                                case 2:
                                    break;
                                case 3:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.WingedTigerLord:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WingedTigerLord], 632, 8, 600, this, 0, true));
                                                break;
                                            case Monster.DarkWraith:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkWraith], 796 + (int)Direction * 5, 5, 500, this));
                                                break;
                                            case Monster.HornedSorceror:
                                                {
                                                    int loops = CurrentActionLevel;
                                                    int duration = 5 * FrameInterval;
                                                    int totalDuration = loops * duration;

                                                    if (FrameLoop == null)
                                                    {
                                                        for (int i = 0; i < loops; i++)
                                                        {
                                                            AudioMgr.Instance.PlaySound(BaseSound + 7, false, 0 + (i * duration));
                                                        }
                                                        Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedSorceror], 971 + (int)Direction * 5, 5, duration, this) { Repeat = true, RepeatUntil = CMain.Time + totalDuration });
                                                    }

                                                    LoopFrame(FrameIndex, 1, FrameInterval, totalDuration);
                                                }
                                                break;
                                        }
                                    }
                                    break;
                                case 4:
                                    switch (BaseImage)
                                    {
                                        case Monster.OlympicFlame:
                                            if (TrackableEffect.GetOwnerEffectID(this.ObjectID, "CreatureSmoke") < 0)
                                                Effects.Add(new TrackableEffect(new Effect(Mir2Res.Pets[((ushort)BaseImage) - 10000], 256, 3, 1000, this), "CreatureSmoke"));
                                            break;
                                        case Monster.Kirin:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Kirin], 824 + (int)Direction * 7, 7, 700, this));
                                            break;
                                        case Monster.FrozenAxeman:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenAxeman], 588 + (int)Direction * 3, 3, 300, this));
                                            break;
                                        case Monster.ManTree:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManTree], 504 + (int)Direction * 2, 2, 200, this));
                                            break;
                                        case Monster.DragonWarrior:
                                            Vector3Int source = Functions.PointMove(CurrentLocation, Direction, 1);
                                            Effect effect = new Effect(Mir2Res.Monsters[(ushort)Monster.DragonWarrior], 664, 6, 600, source, CMain.Time + 300);
                                            MapControl.Effects.Add(effect); 
                                            break;
                                    }
                                    break;
                                case 5:
                                    switch (BaseImage)
                                    {
                                        case Monster.WhiteMammoth:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WhiteMammoth], 376, 5, Frame.Count * Frame.Interval, this));
                                            break;
                                        case Monster.ManTree:
                                            Vector3Int source = Functions.PointMove(CurrentLocation, Direction, 1);
                                            Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.ManTree], 520, 8, 800, source, CMain.Time, drawBehind: true);
                                            MapControl.Effects.Add(ef);
                                            break;
                                        case Monster.HornedSorceror:
                                            AudioMgr.Instance.PlaySound(BaseSound + 5);
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedSorceror], 624, 10, 10 * Frame.Interval, this));
                                            break;
                                        case Monster.HornedCommander:
                                            AudioMgr.Instance.PlaySound(8452, false);
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 864, 8, 8 * Frame.Interval, this));
                                            break;
                                    }
                                    break;
                                case 6:
                                    switch (BaseImage)
                                    {
                                        case Monster.RestlessJar:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RestlessJar], 512, 7, 700, this));
                                            break;
                                    }
                                    break;
                                case 10:
                                    switch (BaseImage)
                                    {
                                        case Monster.StrayCat:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.StrayCat], 728 + (int)Direction * 10, 10, 1000, this));
                                            break;
                                    }
                                    break;
                            }

                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Attack4:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 1:
                                    switch (BaseImage)
                                    {
                                        case Monster.HornedCommander:
                                            {
                                                //Rockfall
                                                int loops = CurrentActionLevel;
                                                int duration = 5 * FrameInterval;
                                                int totalDuration = loops * duration;

                                                if (FrameLoop == null)
                                                {
                                                    for (int i = 0; i < loops; i++)
                                                    {
                                                        AudioMgr.Instance.PlaySound(8453, false, 0 + (i * duration));
                                                    }

                                                    Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 1026 + (int)Direction * 5, 5, duration, this) { Repeat = true, RepeatUntil = CMain.Time + totalDuration });
                                                }

                                                LoopFrame(FrameIndex, 3, FrameInterval, totalDuration);
                                            }
                                            break;
                                    }
                                    break;
                                case 3:
                                    switch (BaseImage)
                                    {
                                        case Monster.SnowWolfKing:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowWolfKing], 581 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                            break;
                                    }
                                    break;
                                case 5:
                                    switch (BaseImage)
                                    {
                                        case Monster.HornedCommander:
                                            AudioMgr.Instance.PlaySound(8454);
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 1078 + (int)Direction * 8, 8, 8 * Frame.Interval, this));
                                            break;
                                    }                                      
                                    break;
                            }

                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Attack5:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.HornedCommander:
                                                LoopFrame(FrameIndex, 2, FrameInterval, CurrentActionLevel * 1000);
                                                break;
                                        }
                                        break;
                                    }
                            }

                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.AttackRange1:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            switch (BaseImage)
                            {
                                case Monster.DragonStatue:
                                    MapObject ob = MapControl.GetObject(TargetID);
                                    if (ob != null)
                                    {
                                        ob.Effects.Add(new Effect(Mir2Res.Dragon, 350, 35, 1200, ob));
                                        AudioMgr.Instance.PlaySound(BaseSound + 6);
                                    }
                                    break;



                            }
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            if (FrameIndex == 2) PlaySwingSound();
                            MapObject ob = null;
                            Missile missile;
                            switch (FrameIndex)
                            {
                                case 1:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.GuardianRock:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Magic2, 1410, 10, 400, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.OmaWitchDoctor:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 792 + (int)Direction * 7, 7, 7 * Frame.Interval, this));
                                                break;
                                            case Monster.FloatingRock:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FloatingRock], 159 + (int)Direction * 7, 7, 7 * Frame.Interval, this));
                                                break;
                                            case Monster.KingHydrax:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingHydrax], 368 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.BlueSoul:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlueSoul], 240, 10, 700, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 7);
                                                }
                                                break;
                                            case Monster.HornedCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 872 + (int)Direction * 7, 7, 7 * Frame.Interval, this));
                                                break;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.LeftGuard:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.LeftGuard], 336 + (int)Direction * 3, 3, 3 * Frame.Interval, this));
                                                break;
                                            case Monster.DreamDevourer:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DreamDevourer], 264, 7, 7 * Frame.Interval, ob));
                                                }
                                                break;
                                            case Monster.DarkDevourer:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 480, 7, 7 * Frame.Interval, ob));
                                                }
                                                break;
                                            case Monster.ManectricClaw:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManectricClaw], 304 + (int)Direction * 10, 10, 10 * Frame.Interval, this));
                                                break;
                                            case Monster.FlameSpear:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlameSpear], 544 + (int)Direction * 10, 10, 10 * 100, this));
                                                break;
                                            case Monster.FrozenMagician:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMagician], 512 + (int)Direction * 6, 6, 6 * Frame.Interval, this));
                                                break;
                                            case Monster.PurpleFaeFlower:
                                                missile = CreateProjectile(331, Mir2Res.Monsters[(int)Monster.PurpleFaeFlower], true, 6, 60, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PurpleFaeFlower], 427, 9, 900, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                        }
                                        break;
                                    }
                                case 3:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.Jar2:
                                                missile = CreateProjectile(688, Mir2Res.Monsters[(ushort)Monster.Jar2], true, 4, 50, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Jar2], 752, 8, 500, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.HardenRhino:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HardenRhino], 392, 5, 5 * Frame.Interval, this));
                                                break;
                                            case Monster.HornedArcher:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedArcher], 336 + (int)Direction * 3, 3, 3 * Frame.Interval, this, drawBehind: true));
                                                break;
                                            case Monster.ColdArcher:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ColdArcher], 336 + (int)Direction * 4, 4, 4 * Frame.Interval, this, drawBehind: true));
                                                break;
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.FurbolgArcher:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    CreateProjectile(344, Mir2Res.Monsters[(ushort)Monster.FurbolgArcher], false, 5, 30, 0);
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.FurbolgGuard:
                                                missile = CreateProjectile(391, Mir2Res.Monsters[(ushort)Monster.FurbolgGuard], false, 1, 30, 0);
                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgGuard], 407, 7, 600, missile.Target));
                                                        AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                    };
                                                }
                                                break;
                                            case Monster.AxeSkeleton:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(224, Mir2Res.Monsters[(ushort)Monster.AxeSkeleton], false, 3, 30, 0);
                                                break;
                                            case Monster.Dark:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(224, Mir2Res.Monsters[(ushort)Monster.Dark], false, 3, 30, 0);
                                                break;
                                            case Monster.ZumaArcher:
                                            case Monster.BoneArcher:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    CreateProjectile(224, Mir2Res.Monsters[(ushort)Monster.ZumaArcher], false, 1, 30, 0);
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.RedThunderZuma:
                                            case Monster.FrozenRedZuma:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Dragon, 400 + CMain.Random.Next(3) * 10, 5, 300, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.BoneLord:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(784, Mir2Res.Monsters[(ushort)Monster.BoneLord], true, 6, 30, 0, direction16: false);
                                                break;
                                            case Monster.RightGuard:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Magic2, 10, 5, 300, ob));
                                                }
                                                break;
                                            case Monster.LeftGuard:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    CreateProjectile(10, Mir2Res.Magic, true, 6, 30, 4);
                                                }
                                                break;
                                            case Monster.MinotaurKing:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.MinotaurKing], 320, 20, 1000, ob));
                                                }
                                                break;
                                            case Monster.FrostTiger:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    CreateProjectile(410, Mir2Res.Magic2, true, 4, 30, 6);
                                                }
                                                break;





                                            case Monster.Yimoogi:
                                            case Monster.RedYimoogi:
                                            case Monster.Snake10:
                                            case Monster.Snake11:
                                            case Monster.Snake12:
                                            case Monster.Snake13:
                                            case Monster.Snake14:
                                            case Monster.Snake15:
                                            case Monster.Snake16:
                                            case Monster.Snake17:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Magic2, 1250, 15, 1000, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.HolyDeva:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Magic2, 10, 5, 300, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.CrossbowOma:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(38, Mir2Res.Monsters[(ushort)Monster.CrossbowOma], false, 1, 30, 6);
                                                break;
                                            case Monster.DarkCrossbowOma:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(38, Mir2Res.Monsters[(ushort)Monster.DarkCrossbowOma], false, 1, 30, 6);
                                                break;
                                            case Monster.WingedOma:
                                            case Monster.DarkWingedOma:
                                                missile = CreateProjectile(224, Mir2Res.Monsters[(ushort)Monster.WingedOma], false, 6, 30, 0, direction16: false);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WingedOma], 272, 2, 150, missile.Target) { Blend = false });
                                                    };
                                                }
                                                break;
                                            case Monster.FlamingMutant:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlamingMutant], 320, 10, 1000, ob.CurrentLocation, CMain.Time) { Blend = true });
                                                    //AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.PoisonHugger:
                                                missile = CreateProjectile(208, Mir2Res.Monsters[(ushort)Monster.PoisonHugger], true, 1, 30, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PoisonHugger], 224, 5, 150, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.RedFoxman:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RedFoxman], 224, 9, 300, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.WhiteFoxman:
                                                missile = CreateProjectile(1160, Mir2Res.Magic, true, 3, 30, 7);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WhiteFoxman], 352, 10, 600, missile.Target));
                                                        AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                    };
                                                }
                                                break;
                                            case Monster.TrapRock:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TrapRock], 26, 10, 600, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.HedgeKekTal:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(38, Mir2Res.Monsters[(ushort)Monster.HedgeKekTal], false, 4, 30, 6);
                                                break;
                                            case Monster.BigHedgeKekTal:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(38, Mir2Res.Monsters[(ushort)Monster.BigHedgeKekTal], false, 4, 30, 6);
                                                break;
                                            case Monster.EvilMir:
                                                missile = CreateProjectile(60, Mir2Res.Dragon, true, 10, 10, 0);

                                                if (missile.Direction > 12)
                                                    missile.Direction = 12;
                                                if (missile.Direction < 7)
                                                    missile.Direction = 7;

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Dragon, 200, 20, 600, missile.Target));
                                                    };
                                                }
                                                break;
                                            case Monster.ArcherGuard:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(38, Mir2Res.Monsters[(ushort)Monster.ArcherGuard], false, 3, 30, 6);
                                                break;
                                            case Monster.SpittingToad:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(280, Mir2Res.Monsters[(ushort)Monster.SpittingToad], true, 6, 30, 0);
                                                break;
                                            case Monster.ArcherGuard2:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(38, Mir2Res.Monsters[(ushort)Monster.ArcherGuard], false, 3, 30, 6);
                                                break;
                                            case Monster.ArcherGuard3:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(104, Mir2Res.Monsters[(ushort)Monster.ArcherGuard3], false, 3, 30, 0);
                                                break;
                                            case Monster.FinialTurtle:
                                                missile = CreateProjectile(272, Mir2Res.Monsters[(ushort)Monster.FinialTurtle], true, 3, 30, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FinialTurtle], 320, 10, 500, missile.Target) { Blend = true });
                                                        AudioMgr.Instance.PlaySound(20000 + (ushort)Spell.FrostCrunch * 10 + 2);
                                                    };
                                                }
                                                break;
                                            case Monster.HellBolt:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HellBolt], 315, 10, 600, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.WitchDoctor:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    missile = CreateProjectile(313, Mir2Res.Monsters[(ushort)Monster.WitchDoctor], true, 5, 30, -5, direction16: false);

                                                    if (missile.Target != null)
                                                    {
                                                        missile.Complete += (o, e) =>
                                                        {
                                                            if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                            missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WitchDoctor], 318, 10, 600, missile.Target));
                                                            AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                        };
                                                    }
                                                }
                                                break;
                                            case Monster.WingedTigerLord:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WingedTigerLord], 640, 10, 800, ob, CMain.Time + 400, false));
                                                }
                                                break;
                                            case Monster.TrollBomber:
                                                missile = CreateProjectile(208, Mir2Res.Monsters[(ushort)Monster.TrollBomber], false, 4, 40, -4, direction16: false);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        AudioMgr.Instance.PlaySound(BaseSound + 9);
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TrollBomber], 212, 6, 600, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.TrollStoner:
                                                AudioMgr.Instance.PlaySound(BaseSound + 9);
                                                missile = CreateProjectile(208, Mir2Res.Monsters[(ushort)Monster.TrollStoner], false, 4, 40, -4, direction16: false);
                                                break;
                                            case Monster.FlameMage:
                                                missile = CreateProjectile(544, Mir2Res.Monsters[(ushort)Monster.FlameMage], true, 3, 20, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlameMage], 592, 10, 1000, missile.Target));
                                                    };
                                                }
                                                break;
                                            case Monster.FlameScythe:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlameScythe], 586, 9, 900, ob));
                                                }
                                                break;
                                            case Monster.FlameAssassin:
                                                missile = CreateProjectile(592, Mir2Res.Monsters[(ushort)Monster.FlameAssassin], true, 3, 20, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlameAssassin], 640, 6, 600, missile.Target));
                                                    };
                                                }
                                                break;
                                            case Monster.FlameQueen:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FlameQueen], 729, 10, Frame.Count * Frame.Interval, this));
                                                break;
                                            case Monster.AncientBringer:
                                                missile = CreateProjectile(688, Mir2Res.Monsters[(ushort)Monster.AncientBringer], true, 4, 50, 0, direction16: false);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AncientBringer], 720, 10, 1000, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.RestlessJar:
                                                missile = CreateProjectile(476, Mir2Res.Monsters[(ushort)Monster.RestlessJar], true, 2, 100, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RestlessJar], 508, 3, 300, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.IceGuard:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IceGuard], 262, 6, 600, ob) { Blend = true });
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.ElementGuard:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ElementGuard], 360 + (int)ob.Direction * 7, 7, 7 * Frame.Interval, ob) { Blend = true });
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.KingGuard:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingGuard], 746, 7, 700, ob) { Blend = true });
                                                }
                                                break;
                                            case Monster.BurningZombie:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BurningZombie], 361, 12, 1000, ob));
                                                }
                                                break;
                                            case Monster.FrozenZombie:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenZombie], 352, 8, 1000, ob));
                                                }
                                                break;
                                            case Monster.CatShaman:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CatShaman], 720, 12, 1500, ob) { Blend = false });
                                                }
                                                break;
                                            case Monster.GeneralMeowMeow:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GeneralMeowMeow], 512, 10, 1000, ob) { Blend = true });
                                                }
                                                break;
                                            case Monster.CannibalTentacles:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(472, Mir2Res.Monsters[(ushort)Monster.CannibalTentacles], true, 8, 100, 0, direction16: false);
                                                break;
                                            case Monster.SwampWarrior:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SwampWarrior], 392, 8, 800, ob) { Blend = true });
                                                }
                                                break;
                                            case Monster.PeacockSpider:
                                                missile = CreateProjectile(664, Mir2Res.Monsters[(ushort)Monster.PeacockSpider], true, 5, 100, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PeacockSpider], 744, 11, 1100, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.RhinoPriest:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RhinoPriest], 376, 9, 900, ob));
                                                }
                                                break;
                                            case Monster.TreeGuardian:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TreeGuardian], 544, 8, 800, ob));
                                                }
                                                break;
                                            case Monster.CreeperPlant:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CreeperPlant], 250, 6, 600, ob.CurrentLocation, CMain.Time) { Blend = true });
                                                    MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CreeperPlant], 256, 10, 1000, ob.CurrentLocation, CMain.Time) { Blend = false });
                                                }
                                                break;
                                            case Monster.FloatingWraith:
                                                missile = CreateProjectile(248, Mir2Res.Monsters[(ushort)Monster.FloatingWraith], true, 2, 20, 0, direction16: true);
                                                break;
                                            case Monster.AvengingSpirit:
                                                missile = CreateProjectile(368, Mir2Res.Monsters[(ushort)Monster.AvengingSpirit], true, 4, 40, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengingSpirit], 432, 10, 1000, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.AvengingWarrior:
                                                missile = CreateProjectile(312, Mir2Res.Monsters[(ushort)Monster.AvengingWarrior], true, 5, 50, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AvengingWarrior], 392, 7, 700, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.LightningBead:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.LightningBead], 65, 12, 1200, ob));
                                                }
                                                break;
                                            case Monster.HealingBead:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HealingBead], 61, 11, 1100, ob));
                                                }
                                                break;
                                            case Monster.PowerUpBead:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PowerUpBead], 64, 6, 600, ob));
                                                }
                                                break;
                                            case Monster.DarkOmaKing:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], 1715, 13, 1300, ob.CurrentLocation));
                                                }
                                                break;
                                            case Monster.ChieftainArcher:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    if (CurrentActionLevel == 0)
                                                    {
                                                        missile = CreateProjectile(312, Mir2Res.Monsters[(ushort)Monster.ChieftainArcher], true, 5, 50, 0, direction16: true);

                                                        if (missile.Target != null)
                                                        {
                                                            missile.Complete += (o, e) =>
                                                            {
                                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                                missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ChieftainArcher], 392, 6, 600, missile.Target));
                                                            };
                                                        }
                                                    }
                                                    else if (CurrentActionLevel == 1)
                                                    {
                                                        missile = CreateProjectile(398, Mir2Res.Monsters[(ushort)Monster.ChieftainArcher], true, 5, 50, 0, direction16: true);

                                                        if (missile.Target != null)
                                                        {
                                                            missile.Complete += (o, e) =>
                                                            {
                                                                if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                                missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ChieftainArcher], 478, 6, 600, missile.Target));
                                                            };
                                                        }
                                                    }
                                                    else
                                                    {
                                                        missile = CreateProjectile(484, Mir2Res.Monsters[(ushort)Monster.ChieftainArcher], true, 5, 50, 0, direction16: true);
                                                    }
                                                }
                                                break;
                                            case Monster.ManTree:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ManTree], 520, 8, 1000, ob));
                                                }
                                                break;
                                            case Monster.ClawBeast:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ClawBeast], 568, 7, 700, ob));
                                                }
                                                break;
                                            case Monster.BlackTortoise:
                                                Vector3Int source = Functions.PointMove(CurrentLocation, Direction, 2);

                                                missile = CreateProjectile(444, Mir2Res.Monsters[(ushort)Monster.BlackTortoise], true, 6, 60, 0, direction16: true);
                                                
                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BlackTortoise], 540, 6, 600, missile.Target));
                                                    };
                                                }
                                                break;
                                            case Monster.DragonArcher:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(416, Mir2Res.Monsters[(ushort)Monster.DragonArcher], true, 5, 50, 0);
                                                break;
                                            case Monster.HornedMage:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedMage], 768, 9, 800, ob) { Blend = false });
                                                }
                                                break; 
                                            case Monster.HornedArcher:
                                                missile = CreateProjectile(360, Mir2Res.Monsters[(ushort)Monster.HornedArcher], true, 3, 50, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedArcher], 408, 6, 500, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.KingHydrax:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingHydrax], 416, 9, 900, ob) { DrawBehind = true });
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.WaterDragon:
                                                missile = CreateProjectile(800, Mir2Res.Monsters[(ushort)Monster.WaterDragon], true, 6, 60, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WaterDragon], 896, 9, 900, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.AntCommander:
                                                missile = CreateProjectile(432, Mir2Res.Monsters[(ushort)Monster.AntCommander], true, 3, 100, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AntCommander], 480, 3, 300, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.AssassinScroll:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AssassinScroll], 299, 8, 800, ob));
                                                }
                                                break;
                                            case Monster.TaoistScroll:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TaoistScroll], 272, 10, 1000, ob) { Blend = true });                                                    
                                                }
                                                break;
                                            case Monster.WarriorScroll:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WarriorScroll], 304, 11, 1100, ob) { Blend = true });
                                                }
                                                break;
                                            case Monster.WizardScroll:
                                                missile = CreateProjectile(300, Mir2Res.Monsters[(ushort)Monster.WizardScroll], true, 5, 50, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WizardScroll], 380, 8, 800, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.Catapult:
                                                missile = CreateProjectile(256, Mir2Res.Siege[(ushort)Monster.Catapult - 940], false, 4, 40, 0);
                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Siege[(ushort)Monster.Catapult - 940], 288, 10, 1000, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.ChariotBallista:
                                                missile = CreateProjectile(38, Mir2Res.Siege[(ushort)Monster.ChariotBallista - 940], false, 3, 30, 6);
                                                break;
                                        }
                                        break;
                                    }//end of case 4
                                case 5:
                                    switch (BaseImage)
                                    {
                                        case Monster.OmaCannibal:
                                            missile = CreateProjectile(360, Mir2Res.Monsters[(ushort)Monster.OmaCannibal], true, 6, 60, 0);

                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                    missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaCannibal], 456, 7, 700, missile.Target) { Blend = true });
                                                };
                                            }
                                            break;
                                        case Monster.OmaMage:
                                            missile = CreateProjectile(392, Mir2Res.Monsters[(ushort)Monster.OmaMage], true, 8, 80, 0, direction16: true);

                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                    missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaMage], 520, 9, 600, missile.Target) { Blend = true });
                                                };
                                            }
                                            break;
                                        case Monster.OmaWitchDoctor:
                                            ob = MapControl.GetObject(TargetID);
                                            if (ob != null)
                                            {
                                                AudioMgr.Instance.PlaySound(BaseSound + 7);
                                                ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 848, 11, 1100, ob) { Blend = true });
                                            }
                                            break;
                                        case Monster.SnowYeti:
                                            missile = CreateProjectile(560, Mir2Res.Monsters[(ushort)Monster.SnowYeti], true, 6, 20, 0);
                                            break;
                                        case Monster.MudZombie:
                                            ob = MapControl.GetObject(TargetID);
                                            if (ob != null)
                                            {
                                                ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.MudZombie], 304, 7, 700, ob) { Blend = false } );
                                            }
                                            break;
                                        case Monster.DarkSpirit:
                                            missile = CreateProjectile(512, Mir2Res.Monsters[(ushort)Monster.DarkSpirit], true, 6, 60, 0);

                                            if (missile.Target != null)
                                            {
                                                missile.Complete += (o, e) =>
                                                {
                                                    if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                    missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.DarkSpirit], 608, 10, 1000, missile.Target));
                                                };
                                            }
                                            break;
                                    }

                                    break;
                                case 6:
                                    {
                                        switch (BaseImage)
                                        {
                                            // Sanjian
                                            case Monster.FurbolgCommander:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgCommander], 357, 6, 600, ob) { DrawBehind = true });
                                                }
                                                break;


                                            case Monster.HornedMage:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedMage], 777, 6, 800, ob) /*{ Blend = false }*/);
                                                }
                                                break;
                                            case Monster.FloatingRock:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FloatingRock], 226, 10, 1000, ob) /*{ Blend = false }*/);
                                                }
                                                break;
                                            case Monster.FrozenArcher:
                                                if (MapControl.GetObject(TargetID) != null)
                                                    CreateProjectile(264, Mir2Res.Monsters[(ushort)Monster.FrozenArcher], true, 5, 80, 0);
                                                break;
                                            case Monster.FrozenMagician:
                                                missile = CreateProjectile(560, Mir2Res.Monsters[(ushort)Monster.FrozenMagician], true, 6, 30, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMagician], 656, 6, 600, missile.Target));
                                                    };
                                                }
                                                break;
                                            case Monster.IceCrystalSoldier:
                                                Vector3Int source = Functions.PointMove(CurrentLocation, Direction, 1);
                                                Effect ef = new Effect(Mir2Res.Monsters[(ushort)Monster.IceCrystalSoldier], 476, 8, 800, source, CMain.Time);
                                                MapControl.Effects.Add(ef);
                                                break;
                                            case Monster.ColdArcher:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ColdArcher], 368 + (int)Direction * 2, 2, 2 * FrameInterval, this));
                                                break;
                                            case Monster.HoodedSummoner:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HoodedSummoner], 374, 13, 1300, ob) { Blend = true });
                                                }
                                                break;
                                        }
                                        break;
                                    }
                                case 7:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.FrozenKnight:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenKnight], 456, 10, 1000, ob));
                                                }
                                                break;
                                            case Monster.IcePhantom:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePhantom], 682, 10, 1000, ob) { Blend = true });
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6); //is this the correct sound for this mob attack?
                                                }
                                                break;
                                            case Monster.FloatingRock:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FloatingRock], 226, 10, 1000, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                        }
                                        break;
                                    }
                                case 8:
                                    {
                                        break;
                                    }
                                case 9:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.ColdArcher:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ColdArcher], 384, 10, 1000, ob));
                                                }
                                                break;
                                                
                                        }
                                        break;
                                    }
                            }
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.AttackRange2:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            MapObject ob = null;
                            Missile missile;
                            switch (FrameIndex)
                            {
                                case 1:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.RestlessJar:
                                                var point = Functions.PointMove(CurrentLocation, Direction, 2);
                                                MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RestlessJar], 391 + (int)Direction * 10, 10, 10 * Frame.Interval, point));
                                                break;
                                            case Monster.KingHydrax:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingHydrax], 425 + (int)Direction * 6, 6, 600, this));
                                                break;
                                        }
                                        break;
                                    }
                                case 2:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.FrozenMagician:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMagician], 662 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                                break;

                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.FurbolgArcher:
                                                if (MapControl.GetObject(TargetID) != null)
                                                {
                                                    missile = CreateProjectile(429, Mir2Res.Monsters[(ushort)Monster.FurbolgArcher], false, 5, 30, 0);

                                                    if (missile.Target != null)
                                                    {
                                                        missile.Complete += (o, e) =>
                                                        {
                                                            if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                            missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgArcher], 424, 5, 500, missile.Target));
                                                            AudioMgr.Instance.PlaySound(BaseSound + 7);
                                                        };
                                                    }
                                                }
                                                break;




                                            case Monster.RedFoxman:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RedFoxman], 233, 10, 400, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 7);
                                                }
                                                break;
                                            case Monster.WhiteFoxman:
                                                missile = CreateProjectile(1160, Mir2Res.Magic, true, 3, 30, 7);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WhiteFoxman], 362, 15, 1800, missile.Target));
                                                        AudioMgr.Instance.PlaySound(BaseSound + 7);
                                                    };
                                                }
                                                break;
                                            case Monster.TrollKing:
                                                missile = CreateProjectile(294, Mir2Res.Monsters[(ushort)Monster.TrollKing], false, 4, 40, -4, direction16: false);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        AudioMgr.Instance.PlaySound(BaseSound + 9);
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TrollKing], 298, 6, 600, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.AncientBringer:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.AncientBringer], 740, 14, 2000, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.IceGuard:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IceGuard], 268, 5, 500, ob) { Blend = true });
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.CatShaman:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CatShaman], 732, 6, 500, ob));
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                                                }
                                                break;
                                            case Monster.TucsonGeneral:
                                                missile = CreateProjectile(592, Mir2Res.Monsters[(ushort)Monster.TucsonGeneral], true, 9, 30, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TucsonGeneral], 736, 9, 900, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.RhinoPriest:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.RhinoPriest], 448, 7, 700, ob) { Blend = true });
                                                }
                                                break;
                                            case Monster.TreeGuardian:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TreeGuardian], 648 + ((int)Direction * 10), 10, 1000, ob.CurrentLocation));
                                                }
                                                break;
                                            case Monster.OmaWitchDoctor:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 859, 10, 1000, ob) { Blend = true });
                                                }
                                                break;
                                            case Monster.KingHydrax:
                                                missile = CreateProjectile(473, Mir2Res.Monsters[(ushort)Monster.KingHydrax], true, 4, 50, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingHydrax], 537, 6, 600, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.IcePhantom:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePhantom], 772, 7, 700, ob) { Blend = true });
                                                    AudioMgr.Instance.PlaySound(BaseSound + 6); //is this the correct sound for this mob attack?
                                                }
                                                break;
                                            case Monster.ColdArcher:
                                                missile = CreateProjectile(394, Mir2Res.Monsters[(ushort)Monster.ColdArcher], true, 3, 50, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.ColdArcher], 442, 6, 500, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.HornedArcher:
                                                missile = CreateProjectile(414, Mir2Res.Monsters[(ushort)Monster.HornedArcher], true, 3, 50, 0, direction16: true);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        if (missile.Target.CurrentAction == MirAction.Dead) return;
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedArcher], 462, 6, 500, missile.Target) { Blend = true });
                                                    };
                                                }
                                                break;
                                            case Monster.HornedCommander:
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 938 + (int)Direction * 9, 9, 9 * Frame.Interval, this));
                                                Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.HornedCommander], 1010 + (int)Direction * 2, 2, 2 * Frame.Interval, this));
                                                break;
                                        }
                                        break;
                                    }
                                case 8:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.FrozenMagician:
                                                missile = CreateProjectile(734, Mir2Res.Monsters[(ushort)Monster.FrozenMagician], true, 6, 30, 0);

                                                if (missile.Target != null)
                                                {
                                                    missile.Complete += (o, e) =>
                                                    {
                                                        missile.Target.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FrozenMagician], 830, 10, 1000, missile.Target));
                                                    };
                                                }
                                                break;
                                        }
                                        break;

                                    }
                            }
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.AttackRange3:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            MapObject ob = null;
                            switch (FrameIndex)
                            {
                                case 4:
                                    {
                                        switch (BaseImage)
                                        {
                                            case Monster.TucsonGeneral:
                                                ob = MapControl.GetObject(TargetID);
                                                if (ob != null)
                                                {
                                                    ob.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.TucsonGeneral], 745, 17, 1000, ob) { Blend = true });
                                                }
                                                break;
                                        }
                                    }
                                    break;
                            }
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Struck:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            SetAction();
                        }
                        else
                        {
                            NextMotion += FrameInterval;
                        }
                    }
                    break;

                case MirAction.Die:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            ActionFeed.Clear();
                            ActionFeed.Add(new QueuedAction { Action = MirAction.Dead, Direction = Direction, Location = CurrentLocation });
                            SetAction();
                        }
                        else
                        {
                            switch (FrameIndex)
                            {
                                case 1:
                                    switch (BaseImage)
                                    {

                                        // Sanjian
                                        case Monster.FurbolgCommander:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.FurbolgCommander], 320, 5, 5 * Frame.Interval, this));
                                            break;
                                        case Monster.Furball:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Furball], 288, 8, Frame.Count * Frame.Interval, this));
                                            break;
                                        case Monster.GlacierBeast:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.GlacierBeast], 342, 12, 1200, this) { Blend = true });
                                            break;


                                        case Monster.PoisonHugger:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.PoisonHugger], 224, 5, Frame.Count * FrameInterval, this));
                                            break;
                                        case Monster.Hugger:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Hugger], 256, 8, Frame.Count * FrameInterval, this));
                                            break;
                                        case Monster.MutatedHugger:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.MutatedHugger], 128, 7, Frame.Count * FrameInterval, this));
                                            break;
                                        case Monster.CyanoGhast:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.CyanoGhast], 681, 7, Frame.Count * FrameInterval, this));
                                            break;
                                        case Monster.Hydrax:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Hydrax], 240, 5, 5 * Frame.Interval, this));
                                            break;
                                    }
                                    break;
                                case 3:
                                    PlayDeadSound();
                                    switch (BaseImage)
                                    {
                                        case Monster.BoneSpearman:
                                        case Monster.BoneBlademan:
                                        case Monster.BoneArcher:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BoneSpearman], 224, 8, Frame.Count * FrameInterval, this));
                                            break;
                                        case Monster.WoodBox:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.WoodBox], 104, 6, 6 * Frame.Interval, this) { Blend = true });
                                            break;
                                        case Monster.BoulderSpirit:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.BoulderSpirit], 64, 8, 8 * Frame.Interval, this) { Blend = true });
                                            break;
                                    }
                                    break;
                                    // Sanjian
                                case 4:
                                    PlayDeadSound();
                                    break;
                                case 5:
                                    switch (BaseImage)
                                    {
                                        case Monster.KingHydrax:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.KingHydrax], 543 + (int)Direction * 7, 7, 700, this));
                                            break;
                                        case Monster.Bear:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.Bear], 353, 9, Frame.Count * Frame.Interval, this));
                                            break;
                                        case Monster.SnowWolfKing:
                                            Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.SnowWolfKing], 605, 10, 10 * Frame.Interval, this) { DrawBehind = true });
                                            break;
                                    }
                                    break;
                                case 9:
                                    switch (BaseImage)
                                    {
                                        case Monster.IcePhantom:
                                            MapControl.Effects.Add(new Effect(Mir2Res.Monsters[(ushort)Monster.IcePhantom], 692 + (int)Direction * 10, 10, 10 * Frame.Interval, CurrentLocation));
                                            break;
                                    }
                                    break;
                            }

                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Revive:
                    if (CMain.Time >= NextMotion)
                    {
                        MapControl.Instance.TextureValid = false;

                        if (SkipFrames) UpdateFrame();

                        if (UpdateFrame() >= Frame.Count)
                        {
                            FrameIndex = Frame.Count - 1;
                            ActionFeed.Clear();
                            ActionFeed.Add(new QueuedAction { Action = MirAction.Standing, Direction = Direction, Location = CurrentLocation });
                            SetAction();
                        }
                        else
                        {
                            if (FrameIndex == 3)
                                PlayReviveSound();
                            NextMotion += FrameInterval;
                        }
                    }
                    break;
                case MirAction.Dead:                
                    break;

            }

            if ((CurrentAction == MirAction.Standing || CurrentAction == MirAction.SitDown) && NextAction != null)
                SetAction();
            else if (CurrentAction == MirAction.Dead && NextAction != null && (NextAction.Action == MirAction.Skeleton || NextAction.Action == MirAction.Revive))
                SetAction();
        }

        public int UpdateFrame()
        {
            if (Frame == null) return 0;

            if (FrameLoop != null)
            {
                if (FrameLoop.CurrentCount > FrameLoop.Loops)
                {
                    FrameLoop = null;
                }
                else if (FrameIndex >= FrameLoop.End)
                {
                    FrameIndex = FrameLoop.Start - 1;
                    FrameLoop.CurrentCount++;
                }
            }

            if (Frame.Reverse) return Math.Abs(--FrameIndex);
            return ++FrameIndex;
        }

        public override Missile CreateProjectile(int baseIndex, string library, bool blend, int count, int interval, int skip, int lightDistance = 6, bool direction16 = true, Color? lightColour = null, uint targetID = 0)
        {
            if (targetID == 0)
            {
                targetID = TargetID;
            }

            MapObject ob = MapControl.GetObject(targetID);

            var targetPoint = TargetPoint;

            if (ob != null) targetPoint = ob.CurrentLocation;

            int duration = Functions.MaxDistance(CurrentLocation, targetPoint) * 50;

            Missile missile = new Missile(library, baseIndex, duration / interval, duration, this, targetPoint, direction16)
            {
                Target = ob,
                Interval = interval,
                FrameCount = count,
                Blend = blend,
                Skip = skip,
                Light = lightDistance,
                LightColour = lightColour == null ? Mir2Color.White : (Color)lightColour
            };

            Effects.Add(missile);

            return missile;
        }

        private void PlaySummonSound()
        {
            switch (BaseImage)
            {
                case Monster.HellKnight1:
                case Monster.HellKnight2:
                case Monster.HellKnight3:
                case Monster.HellKnight4:
                case Monster.LightningBead:
                case Monster.HealingBead:
                case Monster.PowerUpBead:
                    AudioMgr.Instance.PlaySound(BaseSound + 0);
                    return;
                case Monster.BoneFamiliar:
                case Monster.Shinsu:
                case Monster.HolyDeva:
                    AudioMgr.Instance.PlaySound(BaseSound + 5);
                    return;
            }
        }

        private void PlayWalkSound(bool left = true)
        {
            if (left)
            {
                switch (BaseImage)
                {
                    case Monster.WingedTigerLord:
                    case Monster.PoisonHugger:
                    case Monster.SnowWolfKing:
                    case Monster.Catapult:
                    case Monster.ChariotBallista:
                        AudioMgr.Instance.PlaySound(BaseSound + 8);
                        return;
                }
            }
            else
            {
                switch (BaseImage)
                {
                    case Monster.WingedTigerLord:
                    case Monster.AvengerPlant:
                        AudioMgr.Instance.PlaySound(BaseSound + 8);
                        return;
                    case Monster.PoisonHugger:
                    case Monster.SnowWolfKing:
                        AudioMgr.Instance.PlaySound(BaseSound + 9);
                        return;
                }
            }
        }

        public void PlayAppearSound()
        {
            switch (BaseImage)
            {
                case Monster.CannibalPlant:
                case Monster.WaterDragon:
                case Monster.EvilCentipede:
                case Monster.CreeperPlant:
                    return;
                case Monster.ZumaArcher:
                case Monster.ZumaStatue:
                case Monster.ZumaGuardian:
                case Monster.RedThunderZuma:
                case Monster.FrozenRedZuma:
                case Monster.FrozenZumaStatue:
                case Monster.FrozenZumaGuardian:
                case Monster.ZumaTaurus:
                case Monster.DemonGuard:
                case Monster.Turtlegrass:
                case Monster.ManTree:
                case Monster.EarthGolem:
                case Monster.AssassinScroll:
                case Monster.WarriorScroll:
                case Monster.TaoistScroll:
                case Monster.WizardScroll:
                case Monster.PurpleFaeFlower:
                    if (Stoned) return;
                    break;
                case Monster.DragonStatue:
                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                    return;
            }

            AudioMgr.Instance.PlaySound(BaseSound);
        }

        public void PlayPopupSound()
        {
            switch (BaseImage)
            {
                case Monster.ZumaTaurus:
                case Monster.DigOutZombie:
                case Monster.Armadillo:
                case Monster.ArmadilloElder:
                    AudioMgr.Instance.PlaySound(BaseSound + 5);
                    return;
                case Monster.Shinsu:
                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                    return;
            }
            AudioMgr.Instance.PlaySound(BaseSound);
        }

        public void PlayRunSound()
        {
            switch (BaseImage)
            {
                case Monster.HardenRhino:
                    AudioMgr.Instance.PlaySound(BaseSound + 8);
                    break;
            }
        }

        public void PlayJumpSound()
        {
            switch (BaseImage)
            {
                case Monster.Armadillo:
                case Monster.ArmadilloElder:
                case Monster.ChieftainArcher:
                    AudioMgr.Instance.PlaySound(BaseSound + 8);
                    break;
            }
        }

        public void PlayDashSound()
        {
            switch (BaseImage)
            {
                case Monster.HornedSorceror:
                    AudioMgr.Instance.PlaySound(BaseSound + 9);
                    break;
            }
        }

        public void PlayFlinchSound()
        {
            switch (BaseImage)
            {
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 2);
                    break;
            }
        }
        public void PlayStruckSound()
        {
            switch(BaseImage)
            {
                case Monster.EvilMir:
                    AudioMgr.Instance.PlaySound(SoundList.StruckEvilMir);
                    return;
            }

            switch (StruckWeapon)
            {
                case 0:
                case 23:
                case 28:
                case 40:
                    AudioMgr.Instance.PlaySound(SoundList.StruckWooden);
                    break;
                case 1:
                case 12:
                    AudioMgr.Instance.PlaySound(SoundList.StruckShort);
                    break;
                case 2:
                case 8:
                case 11:
                case 15:
                case 18:
                case 20:
                case 25:
                case 31:
                case 33:
                case 34:
                case 37:
                case 41:
                    AudioMgr.Instance.PlaySound(SoundList.StruckSword);
                    break;
                case 3:
                case 5:
                case 7:
                case 9:
                case 13:
                case 19:
                case 24:
                case 26:
                case 29:
                case 32:
                case 35:
                    AudioMgr.Instance.PlaySound(SoundList.StruckSword2);
                    break;
                case 4:
                case 14:
                case 16:
                case 38:
                    AudioMgr.Instance.PlaySound(SoundList.StruckAxe);
                    break;
                case 6:
                case 10:
                case 17:
                case 22:
                case 27:
                case 30:
                case 36:
                case 39:
                    AudioMgr.Instance.PlaySound(SoundList.StruckShort);
                    break;
                case 21:
                    AudioMgr.Instance.PlaySound(SoundList.StruckClub);
                    break;
            }
        }
        public void PlayAttackSound()
        {
            switch (BaseImage)
            {
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 1);
                    break;
            }
        }

        public void PlaySecondAttackSound()
        {
            switch (BaseImage)
            {
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 6);
                    break;

            }            
        }

        public void PlayThirdAttackSound()
        {
            switch (BaseImage)
            {
                case Monster.DarkCaptain:
                case Monster.HornedSorceror:
                case Monster.HornedCommander:
                    return;
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 7);
                    return;
            }
        }

        public void PlayFourthAttackSound()
        {
            switch (BaseImage)
            {
                case Monster.HornedCommander:
                    return;
                case Monster.SnowWolfKing:
                    AudioMgr.Instance.PlaySound(BaseSound + 5);
                    return;
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 8);
                    return;
            }
        }

        public void PlayFithAttackSound()
        {
            AudioMgr.Instance.PlaySound(BaseSound + 9);
        }

        public void PlaySwingSound()
        {
            switch (BaseImage)
            {
                case Monster.DarkCaptain:
                case Monster.EvilMir:
                case Monster.DragonStatue:
                    return;
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 4);
                    return;
            }
        }
        public void PlayDieSound()
        {
            switch (BaseImage)
            {
                default:
                    AudioMgr.Instance.PlaySound(BaseSound + 3);
                    return;
            }
        }

        public void PlayDeadSound()
        {
            switch (BaseImage)
            {
                case Monster.CaveBat:
                case Monster.HellKnight1:
                case Monster.HellKnight2:
                case Monster.HellKnight3:
                case Monster.HellKnight4:
                case Monster.CyanoGhast:
                case Monster.WoodBox:
                    AudioMgr.Instance.PlaySound(BaseSound + 5);
                    return;
            }
        }
        public void PlayReviveSound()
        {
            switch (BaseImage)
            {
                case Monster.ClZombie:
                case Monster.NdZombie:
                case Monster.CrawlerZombie:
                    AudioMgr.Instance.PlaySound(SoundList.ZombieRevive);
                    return;
            }
        }
        public void PlayRangeSound()
        {
            switch (BaseImage)
            {
                case Monster.RedThunderZuma:
                case Monster.FrozenRedZuma:
                case Monster.KingScorpion:
                case Monster.DarkDevil:
                case Monster.Khazard:
                case Monster.BoneLord:
                case Monster.LeftGuard:
                case Monster.RightGuard:
                case Monster.FrostTiger:
                case Monster.GreatFoxSpirit:
                case Monster.BoneSpearman:
                case Monster.MinotaurKing:
                case Monster.WingedTigerLord:
                case Monster.ManectricClaw:
                case Monster.ManectricKing:
                case Monster.HellBolt:
                case Monster.WitchDoctor:
                case Monster.FlameSpear:
                case Monster.FlameMage:
                case Monster.FlameScythe:
                case Monster.FlameAssassin:
                case Monster.FlameQueen:
                case Monster.DarkDevourer:
                case Monster.DreamDevourer:
                case Monster.FlyingStatue:
                case Monster.IceGuard:
                case Monster.ElementGuard:
                case Monster.KingGuard:
                case Monster.Yimoogi:
                case Monster.RedYimoogi:
                case Monster.Snake10:
                case Monster.Snake11:
                case Monster.Snake12:
                case Monster.Snake13:
                case Monster.Snake14:
                case Monster.Snake15:
                case Monster.Snake16:
                case Monster.Snake17:
                case Monster.BurningZombie:
                case Monster.MudZombie:
                case Monster.FrozenZombie:
                case Monster.UndeadWolf:
                case Monster.CatShaman:
                case Monster.CannibalTentacles:
                case Monster.SwampWarrior:
                case Monster.GeneralMeowMeow:
                case Monster.RhinoPriest:
                case Monster.HardenRhino:
                case Monster.TreeGuardian:
                case Monster.OmaCannibal:
                case Monster.OmaMage:
                case Monster.OmaWitchDoctor:
                case Monster.CreeperPlant:
                case Monster.AvengingSpirit:
                case Monster.AvengingWarrior:
                case Monster.PeacockSpider:
                case Monster.FlamingMutant:
                case Monster.KingHydrax:
                case Monster.DarkCaptain:
                case Monster.DarkOmaKing:
                case Monster.HornedMage:
                case Monster.FrozenKnight:
                case Monster.IcePhantom:
                case Monster.WaterDragon:
                case Monster.BlackTortoise:
                case Monster.EvilMir:
                case Monster.DragonStatue:
                    AudioMgr.Instance.PlaySound(BaseSound + 5);
                    return;
                case Monster.AncientBringer:
                case Monster.SeedingsGeneral:
                    AudioMgr.Instance.PlaySound(BaseSound + 7);
                    return;
                case Monster.RestlessJar:
                    AudioMgr.Instance.PlaySound(BaseSound + 8);
                    return;
                case Monster.TucsonGeneral:
                    return;
                default:
                    PlayAttackSound();
                    return;
            }
        }
        public void PlaySecondRangeSound()
        {
            switch (BaseImage)
            {
                case Monster.TucsonGeneral:
                    AudioMgr.Instance.PlaySound(BaseSound + 5);
                    return;
                case Monster.TurtleKing:
                    return;
                case Monster.KingGuard:
                case Monster.TreeGuardian:
                case Monster.DarkCaptain:
                case Monster.HornedCommander:
                    AudioMgr.Instance.PlaySound(BaseSound + 7);
                    return;
                case Monster.AncientBringer:
                case Monster.SeedingsGeneral:
                    AudioMgr.Instance.PlaySound(BaseSound + 8);
                    return;
                default:
                    PlaySecondAttackSound();
                    return;
            }
        }

        public void PlayThirdRangeSound()
        {
            switch (BaseImage)
            {
                case Monster.TucsonGeneral:
                    AudioMgr.Instance.PlaySound(BaseSound + 7);
                    return;
                default:
                    PlayThirdAttackSound();
                    return;
            }
        }

        public void PlayPickupSound()
        {
            AudioMgr.Instance.PlaySound(SoundList.PetPickup);
        }

        public void PlayPetSound()
        {
            int petSound = (ushort)BaseImage - 10000 + 10500;

            switch (BaseImage)
            {
                case Monster.Chick:
                case Monster.BabyPig:
                case Monster.Kitten:
                case Monster.BabySkeleton:
                case Monster.Baekdon:
                case Monster.Wimaen:
                case Monster.BlackKitten:
                case Monster.BabyDragon:
                case Monster.OlympicFlame:
                case Monster.BabySnowMan:
                case Monster.Frog:
                case Monster.BabyMonkey:
                case Monster.AngryBird:
                case Monster.Foxey:
                case Monster.MedicalRat:
                    AudioMgr.Instance.PlaySound(petSound);
                    break;
            }
        }
        public override void Draw()
        {
            //DrawBehindEffects(Settings.Effect);

            //float oldOpacity = DXManager.Opacity;
            //if (Hidden && !DXManager.Blending) DXManager.SetOpacity(0.5F);

            //if (BodyLibrary == null || Frame == null) return;

            //bool oldGrayScale = DXManager.GrayScale;
            //Color drawColour = ApplyDrawColour();
            
            //if (!DXManager.Blending && Frame.Blend)
            //    BodyLibrary.DrawBlend(DrawFrame, DrawLocation, drawColour, true);
            //else
            //    BodyLibrary.Draw(DrawFrame, DrawLocation, drawColour, true);

            //DXManager.SetGrayscale(oldGrayScale);
            //DXManager.SetOpacity(oldOpacity);
        }


        public override bool MouseOver(Vector3Int p)
        {
            return MapControl.MapLocation == CurrentLocation || BodyLibrary != null && MLibraryMgr.Instance.AddOrGet(BodyLibrary).VisiblePixel(DrawFrame, (Vector2Int)(p - FinalDrawLocation), false);
        }

        public override void DrawBehindEffects(bool effectsEnabled)
        {
            for (int i = 0; i < Effects.Count; i++)
            {
                if (!Effects[i].DrawBehind) continue;
                Effects[i].Draw();
            }
        }


        public override void DrawEffects(bool effectsEnabled)
        {
            if (!effectsEnabled) return;

            for (int i = 0; i < Effects.Count; i++)
            {
                if (Effects[i].DrawBehind) continue;
                Effects[i].Draw();
            }

            switch (BaseImage)
            {
                case Monster.Scarecrow:
                    switch (CurrentAction)
                    {
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Scarecrow], 224 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.CaveMaggot:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex >= 1)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CaveMaggot], 175 + FrameIndex + (int)Direction * 5, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.Skeleton:
                case Monster.BoneFighter:
                case Monster.AxeSkeleton:
                case Monster.BoneWarrior:
                case Monster.BoneElite:
                case Monster.BoneWhoo:
                    switch (CurrentAction)
                    {
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Skeleton], 224 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.WoomaTaurus:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WoomaTaurus], 224 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.Dung:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex >= 1)
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Dung], 223 + FrameIndex + (int)Direction * 5, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.WedgeMoth:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WedgeMoth], 224 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.RedThunderZuma:
                case Monster.FrozenRedZuma:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.RedThunderZuma], 320 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                        case MirAction.Pushed:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.RedThunderZuma], 352 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.RedThunderZuma], 400 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.RedThunderZuma], 448 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.RedThunderZuma], 464 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.KingHog:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingHog], 224 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.DarkDevil:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevil], 342 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.BoneLord:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 400 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                        case MirAction.Pushed:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 432 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 480 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 528 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 576 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 624 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BoneLord], 640 + FrameIndex + (int)Direction * 20, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.HolyDeva:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HolyDeva], 226 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                        case MirAction.Pushed:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HolyDeva], 258 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HolyDeva], 306 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HolyDeva], 354 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            if (FrameIndex <= 6)
                            {
                                Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HolyDeva], 370 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                        case MirAction.Appear:
                            if (FrameIndex >= 5)
                            {
                                Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HolyDeva], 418 + FrameIndex - 5, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.YinDevilNode:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.YinDevilNode], 22 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.YangDevilNode:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.YangDevilNode], 22 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.OmaKing:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaKing], 624 + FrameIndex + (int)Direction * 4 - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaKing], 656 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaKing], 304 + FrameIndex + (int)Direction * 20, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.BlackFoxman:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackFoxman], (234 + FrameIndex + (int)Direction * 4) - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;

                case Monster.ManectricKing:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricKing], 360 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricKing], 392 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricKing], 440 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricKing], 576 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricKing], 488 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.ManectricStaff:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricStaff], 296 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.ManectricBlest:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            if (FrameIndex >= 4)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricBlest], (328 + FrameIndex + (int)Direction * 4) - 4, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                        case MirAction.Attack3:
                            if (FrameIndex >= 2)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManectricBlest], (360 + FrameIndex + (int)Direction * 5) - 2, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.KingGuard:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 392 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 424 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 472 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 616 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Pushed:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 352 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 520 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 664 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.KingGuard], 728 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.Jar2:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Jar2], 312 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Jar2], 392 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Jar2], 440 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Jar2], 520 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Jar2], 544 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.SeedingsGeneral:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 536 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 568 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 704 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 776 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Dead:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 1015 + FrameIndex + (int)Direction * 1, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 984 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 1008 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 848 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.SeedingsGeneral], 912 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;

                case Monster.HellSlasher:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex >= 2 && FrameIndex < 6)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellSlasher], (304 + FrameIndex + (int)Direction * 4) - 2, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;

                case Monster.HellPirate:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellPirate], (280 + FrameIndex + (int)Direction * 4) - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;

                case Monster.HellCannibal:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellCannibal], 304 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;

                case Monster.HellKeeper:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellKeeper], 40 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.Manticore:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Manticore], (536 + FrameIndex + (int)Direction * 4) - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.GuardianRock:
                    switch (CurrentAction)
                    {
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.GuardianRock], 8 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.ThunderElement:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ThunderElement], 44 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                        case MirAction.Pushed:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ThunderElement], 54 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ThunderElement], 64 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ThunderElement], 74 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ThunderElement], 78 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.CloudElement:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CloudElement], 44 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                        case MirAction.Pushed:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CloudElement], 54 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CloudElement], 64 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CloudElement], 74 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CloudElement], 78 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.GreatFoxSpirit:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], Frame.Start + 30 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], Frame.Start + 30 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], Frame.Start + 30 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.GreatFoxSpirit], 318 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.TaoistGuard:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TaoistGuard], 80 + ((int)Direction * 3) + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.CyanoGhast: //mob glow effect
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CyanoGhast], 448 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CyanoGhast], 480 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CyanoGhast], 528 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CyanoGhast], 576 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                        case MirAction.Revive:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CyanoGhast], 592 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.MutatedManworm:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.MutatedManworm], 285 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.MutatedManworm], 333 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.CrazyManworm:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CrazyManworm], 272 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;

                case Monster.Behemoth:
                    switch (CurrentAction)
                    {
                        case MirAction.Walking:
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], 464 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Standing:
                        case MirAction.Revive:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], 512 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                        case MirAction.Attack3:
                        case MirAction.AttackRange1:
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], 592 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            if (FrameIndex >= 4)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], (667 + FrameIndex + (int)Direction * 2) - 4, DrawLocation, Mir2Color.White, true);
                            }
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], 592 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            if (FrameIndex >= 1)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], 658 + FrameIndex - 1, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }

                    if (CurrentAction != MirAction.Dead)
                    {
                        Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Behemoth], 648 + FrameIndex, DrawLocation, Mir2Color.White, true);
                    }
                    break;

                case Monster.DarkDevourer:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 272 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 304 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 352 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 540 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 400 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                        case MirAction.Revive:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkDevourer], 416 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;

                case Monster.DreamDevourer:
                    switch (CurrentAction)
                    {
                        case MirAction.AttackRange1:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DreamDevourer], 320 + (FrameIndex + (int)Direction * 5) - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;

                case Monster.TurtleKing:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 456 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 488 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 536 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 616 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                        case MirAction.Revive:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 632 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 704 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 752 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 800 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange3:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TurtleKing], 848 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;

                case Monster.WingedTigerLord:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex >= 2)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WingedTigerLord], 584 + (FrameIndex + (int)Direction * 6) - 2, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                        case MirAction.Attack2:
                            if (FrameIndex >= 2)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WingedTigerLord], 560 + (FrameIndex + (int)Direction * 3) - 2, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.StoningStatue:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex == 4)
                            {
                                AudioMgr.Instance.PlaySound(BaseSound + 5);
                            }
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.StoningStatue], 464 + FrameIndex + (int)Direction * 20, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.FlameSpear:
                case Monster.FlameMage:
                case Monster.FlameScythe:
                case Monster.FlameAssassin:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 272 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 304 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 352 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 400 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 416 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 496 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            if (BaseImage == Monster.FlameScythe && (int)Direction > 0)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 544 + FrameIndex + (int)Direction * 6 - 6, DrawLocation, Mir2Color.White, true);
                            }
                            else if (BaseImage == Monster.FlameAssassin)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 544 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.FlameQueen:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FlameQueen], 360 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FlameQueen], 392 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FlameQueen], 440 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FlameQueen], 488 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FlameQueen], 504 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FlameQueen], 584 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.HellKnight1:
                case Monster.HellKnight2:
                case Monster.HellKnight3:
                case Monster.HellKnight4:
                    switch (CurrentAction)
                    {
                        case MirAction.Appear:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 224 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 224 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 256 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 304 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 352 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 368 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)BaseImage], 400 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.HellLord:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                        case MirAction.Attack1:
                        case MirAction.Struck:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellLord], 15, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellLord], 16 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Dead:
                            Mir2Res.Draw(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HellLord], 20, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.WaterGuard:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            if (FrameIndex >= 4)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterGuard], 264 + (FrameIndex + (int)Direction * 3) - 4, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.HardenRhino:
                    switch (CurrentAction)
                    {
                        case MirAction.Running:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HardenRhino], 397 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.AncientBringer:
                    switch (CurrentAction)
                    {
                        case MirAction.AttackRange1:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.AncientBringer], (648 + FrameIndex + (int)Direction * 5) - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break; //on mob
                        case MirAction.AttackRange2:
                            if (FrameIndex >= 3)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.AncientBringer], (730 + FrameIndex + (int)Direction * 10) - 3, DrawLocation, Mir2Color.White, true);
                            }
                            break; //on mob
                    }
                    break;
                case Monster.BurningZombie:
                    switch (CurrentAction)
                    {
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BurningZombie], 352 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.MudZombie:
                    switch (CurrentAction)
                    {
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.MudZombie], 304 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.BlackHammerCat:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 336 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 368 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 416 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 472 + FrameIndex + (int)Direction * 12, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 568 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.BlackHammerCat], 589 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.StrayCat:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack3:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.StrayCat], 632 + FrameIndex + (int)Direction * 12, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.CatShaman:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 360 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 392 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 472 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 520 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 576 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 746 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 632 + FrameIndex + (int)Direction * 2, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.CatShaman], 648 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.TucsonGeneral:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            if (FrameIndex >= 2)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TucsonGeneral], (504 + FrameIndex + (int)Direction * 5) - 2, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.RhinoWarrior:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.RhinoWarrior], 320 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.TreeGuardian:
                    switch (CurrentAction)
                    {
                        case MirAction.Attack2:
                            if (FrameIndex >= 5)
                            {
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.TreeGuardian], 608 + FrameIndex + (int)Direction * 5, DrawLocation, Mir2Color.White, true);
                            }
                            break;
                    }
                    break;
                case Monster.OmaWitchDoctor:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 400 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 472 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 520 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 576 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 632 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 704 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.OmaWitchDoctor], 727 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.PlagueCrab:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PlagueCrab], 248 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PlagueCrab], 280 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PlagueCrab], 328 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PlagueCrab], 392 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PlagueCrab], 423 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.ClawBeast:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ClawBeast], 256 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ClawBeast], 288 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ClawBeast], 336 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ClawBeast], 416 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ClawBeast], 440 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.DarkCaptain:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 584 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 664 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 728 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 784 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 840 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 896 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack3:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 952 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange3:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1008 + FrameIndex + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1064 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkCaptain], 1088 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.FrozenGolem:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FrozenGolem], 264 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FrozenGolem], 296 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FrozenGolem], 344 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FrozenGolem], 408 + FrameIndex + (int)Direction * 12, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.IcePhantom:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 320 + FrameIndex + (int)Direction * 4, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 352 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 400 + FrameIndex + (int)Direction * 9, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 472 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 472 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 536 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.IcePhantom], 560 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;

                case Monster.HornedMage:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (384 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Walking:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (416 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (464 + FrameIndex + (int)Direction * 8), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.AttackRange1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (528 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.AttackRange2:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (600 + FrameIndex + (int)Direction * 8), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Struck:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (664 + FrameIndex + (int)Direction * 3), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Die:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedMage], (688 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.White, true);
                                break;
                        }
                    }
                    break;
                case Monster.Kirin:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (392 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Walking:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (496 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (544 + FrameIndex + (int)Direction * 7), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack2:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (600 + FrameIndex + (int)Direction * 12), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack3:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (696 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Struck:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (744 + FrameIndex + (int)Direction * 3), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Die:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.Kirin], (744 + FrameIndex + (int)Direction * 1) - 1, DrawLocation, Mir2Color.White, true);
                                break;
                        }
                    }
                    break;
                case Monster.DarkWraith:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (360 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Walking:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (392 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (440 + FrameIndex + (int)Direction * 8), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack2:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (504 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack3:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (584 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Struck:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (616 + FrameIndex + (int)Direction * 3), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Die:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkWraith], (640 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.White, true);
                                break;
                        }
                        break;
                    }
                case Monster.DarkSpirit:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkSpirit], (256 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Walking:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkSpirit], (288 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.AttackRange1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkSpirit], (336 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Struck:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkSpirit], (408 + FrameIndex + (int)Direction * 3), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Die:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkSpirit], (432 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.White, true);
                                break;
                        }
                        break;
                    }
                case Monster.LightningBead:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.LightningBead], 30 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.LightningBead], 37 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.LightningBead], 43 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.LightningBead], 50 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Appear:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.LightningBead], 58 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.HealingBead:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HealingBead], 30 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HealingBead], 37 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HealingBead], 43 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HealingBead], 46 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Appear:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HealingBead], 54 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.PowerUpBead:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PowerUpBead], 30 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PowerUpBead], 37 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PowerUpBead], 43 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PowerUpBead], 49 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Appear:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.PowerUpBead], 58 + FrameIndex, DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.DarkOmaKing:
                    switch (CurrentAction)
                    {
                        case MirAction.Standing:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (784 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Walking:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (864 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (912 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack2:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (984 + FrameIndex + (int)Direction * 34), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack3:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (1256 + FrameIndex + (int)Direction * 8), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Attack4:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (1320 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.AttackRange1:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (1392 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Struck:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (1464 + FrameIndex + (int)Direction * 3), DrawLocation, Mir2Color.White, true);
                            break;
                        case MirAction.Die:
                            Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.DarkOmaKing], (1488 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.White, true);
                            break;
                    }
                    break;
                case Monster.HornedWarrior:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Standing:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (376 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Walking:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (408 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (456 + FrameIndex + (int)Direction * 8), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack2:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (520 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack3:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (592 + FrameIndex + (int)Direction * 8), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Struck:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (656 + FrameIndex + (int)Direction * 3), DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Die:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.HornedWarrior], (680 + FrameIndex + (int)Direction * 9), DrawLocation, Mir2Color.White, true);
                                break;
                        }
                        break;
                    }
                case Monster.FloatingRock:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.AttackRange1:
                                if (FrameIndex <= 6)
                                {
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.FloatingRock], 160 + (FrameIndex) + (int)Direction * 7, DrawLocation, Mir2Color.White, true);
                                }
                                break;
                        }
                        break;
                    }
                case Monster.FrostTiger:
                    {
                        if (Effect == 1)
                            switch (CurrentAction)
                            {
                                case MirAction.Standing:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (528 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.Gray, true);
                                    break;
                                case MirAction.Walking:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (560 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.Gray, true);
                                    break;
                                case MirAction.Attack1:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (608 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.Gray, true);
                                    break;
                                case MirAction.Struck:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (656 + FrameIndex + (int)Direction * 2), DrawLocation, Mir2Color.Gray, true);
                                    break;
                                case MirAction.Die:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (672 + FrameIndex + (int)Direction * 10), DrawLocation, Mir2Color.Gray, true);
                                    break;
                                case MirAction.AttackRange1:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (752 + FrameIndex + (int)Direction * 6), DrawLocation, Mir2Color.Gray, true);
                                    break;
                                case MirAction.SitDown:
                                    Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.ManTree], (800 + FrameIndex + (int)Direction * 4), DrawLocation, Mir2Color.Gray, true);
                                    break;
                            }
                        break;
                    }
                case Monster.WaterDragon:
                    {
                        switch (CurrentAction)
                        {
                            case MirAction.Show:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 400 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Standing:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 464 + FrameIndex + (int)Direction * 6, DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.AttackRange1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 512 + FrameIndex + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Attack1:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 576 + FrameIndex + (int)Direction * 10, DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Struck:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 656 + FrameIndex + (int)Direction * 3, DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Die:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 680 + FrameIndex + (int)Direction * 15, DrawLocation, Mir2Color.White, true);
                                break;
                            case MirAction.Hide:
                                Mir2Res.DrawBlend(mMonsterSpriteRenderer, Mir2Res.Monsters[(ushort)Monster.WaterDragon], 407 + (FrameIndex * -1) + (int)Direction * 8, DrawLocation, Mir2Color.White, true);
                                break;
                        }
                        break;
                    }

            } //END OF DRAW EFFECTS
        }

        public override void DrawName()
        {
            if (!Name.Contains("_"))
            {
                base.DrawName();
                return;
            }

            string[] splitName = Name.Split('_');

            //IntelligentCreature
            int yOffset = 0;
            switch (BaseImage)
            {
                case Monster.Chick:
                    yOffset = -10;
                    break;
                case Monster.BabyPig:
                case Monster.Kitten:
                case Monster.BabySkeleton:
                case Monster.Baekdon:
                case Monster.Wimaen:
                case Monster.BlackKitten:
                case Monster.BabyDragon:
                case Monster.OlympicFlame:
                case Monster.BabySnowMan:
                case Monster.Frog:
                case Monster.BabyMonkey:
                case Monster.AngryBird:
                case Monster.Foxey:
                case Monster.MedicalRat:
                    yOffset = -20;
                    break;
            }

            for (int s = 0; s < splitName.Count(); s++)
            {
                CreateMonsterLabel(splitName[s], s);

                TempLabel.text = splitName[s];
                TempLabel.transform.localPosition = new Vector3(DisplayRectangle.x + (48 - TempLabel.rectTransform.sizeDelta.x) / 2, DisplayRectangle.y - (32 - TempLabel.rectTransform.sizeDelta.y / 2) + (Dead ? 35 : 8) - (((splitName.Count() - 1) * 10) / 2) + (s * 12) + yOffset);
            }
        }

        public void CreateMonsterLabel(string word, int wordOrder)
        {
            TempLabel = null;

            //for (int i = 0; i < LabelList.Count; i++)
            //{
            //    if (LabelList[i].Text != word) continue;
            //    TempLabel = LabelList[i];
            //    break;
            //}

            //if (TempLabel != null && !TempLabel.IsDestroyed() && NameColour == OldNameColor) return;

            //OldNameColor = NameColour;

            //TempLabel = new MirLabel
            //{
            //    AutoSize = true,
            //    BackColour = Color.Transparent,
            //    ForeColour = NameColour,
            //    OutLine = true,
            //    OutLineColour = Color.Black,
            //    Text = word,
            //};

            //TempLabel.
            //TempLabel.Disposing += (o, e) => LabelList.Remove(TempLabel);
            //LabelList.Add(TempLabel);
        }

        public override void DrawChat()
        {
            //if (ChatLabel == null || ChatLabel.IsDisposed) return;

            //if (CMain.Time > ChatTime)
            //{
            //    ChatLabel.Dispose();
            //    ChatLabel = null;
            //    return;
            //}

            ////IntelligentCreature
            //int yOffset = 0;
            //switch (BaseImage)
            //{
            //    case Monster.Chick:
            //        yOffset = 30;
            //        break;
            //    case Monster.BabyPig:
            //    case Monster.Kitten:
            //    case Monster.BabySkeleton:
            //    case Monster.Baekdon:
            //    case Monster.Wimaen:
            //    case Monster.BlackKitten:
            //    case Monster.BabyDragon:
            //    case Monster.OlympicFlame:
            //    case Monster.BabySnowMan:
            //    case Monster.Frog:
            //    case Monster.BabyMonkey:
            //    case Monster.AngryBird:
            //    case Monster.Foxey:
            //    case Monster.MedicalRat:
            //        yOffset = 20;
            //        break;
            //}

            //ChatLabel.ForeColour = Dead ? Color.Gray : Color.White;
            //ChatLabel.Location = new Point(DisplayRectangle.X + (48 - ChatLabel.Size.Width) / 2, DisplayRectangle.Y - (60 + ChatLabel.Size.Height) - (Dead ? 35 : 0) + yOffset);
            //ChatLabel.Draw();
        }
    }
}
