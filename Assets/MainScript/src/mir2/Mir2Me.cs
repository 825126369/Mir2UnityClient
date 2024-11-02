using System.Drawing;
using UnityEngine;

public class Mir2Me : SingleTonMonoBehaviour<Mir2Me>
{
    public float fSpeed = 100f;
    readonly TimeOutGenerator mTimeOut_MouseDown = TimeOutGenerator.New(0.5f);

    public const int CellWidth = 48;
    public const int CellHeight = 32;

    public static int OffSetX;
    public static int OffSetY;

    public static int ViewRangeX;
    public static int ViewRangeY;

    bool bAtuoRun = false;

    public Point CurrentLocation;
    public Point MapLocation;
    public Point Movement;
    public Point OffSetMove;
    public MirDirection Direction;
    public MirAction CurrentAction;

    public MirGender Gender;
    public MirClass Class;
    public byte Hair;
    public ushort Level;

    public int FrameIndex;

    public void Init()
    {
        OffSetX = Screen.width / 2 / CellWidth;
        OffSetY = Screen.height / 2 / CellHeight - 1;
        ViewRangeX = OffSetX + 6;
        ViewRangeY = OffSetY + 6;
    }

    private void Update()
    {
        if (mTimeOut_MouseDown.orTimeOut())
        {
            if (Input.GetMouseButtonDown(0))
            {
                bAtuoRun = false;
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector3 dir = Vector3.Normalize(transform.position - pos);
                Direction = GetDirection(dir);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                bAtuoRun = true;
            }
        }

        UpdateFrame();
        OnMove();
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

    private void UpdateFrame()
    {
        FrameIndex++;
        if(FrameIndex > 2)
        {
            FrameIndex = 0;
        }
    }

    private void OnMove()
    {
        switch (CurrentAction)
        {
            case MirAction.Walking:
            case MirAction.Running:
            case MirAction.MountWalking:
            case MirAction.MountRunning:
            case MirAction.Pushed:
            case MirAction.DashL:
            case MirAction.DashR:
            case MirAction.Sneek:
            case MirAction.Jump:
            case MirAction.DashAttack:

                int i = 0;
                if (CurrentAction == MirAction.Running)
                {
                    i = 2;
                }
                else
                {
                    i = 1;
                }
                
                int count = 3;
                int index = FrameIndex;

                Movement = Functions.PointMove(CurrentLocation, Direction, CurrentAction == MirAction.Pushed ? 0 : -i);
                switch (Direction)
                {
                    case MirDirection.Up:
                        OffSetMove = new Point(0, (int)((Map.CellHeight * i / (float)(count)) * (index + 1)));
                        break;
                    case MirDirection.UpRight:
                        OffSetMove = new Point((int)((-Map.CellWidth * i / (float)(count)) * (index + 1)), (int)((Map.CellHeight * i / (float)(count)) * (index + 1)));
                        break;
                    case MirDirection.Right:
                        OffSetMove = new Point((int)((-Map.CellWidth * i / (float)(count)) * (index + 1)), 0);
                        break;
                    case MirDirection.DownRight:
                        OffSetMove = new Point((int)((-Map.CellWidth * i / (float)(count)) * (index + 1)), (int)((-Map.CellHeight * i / (float)(count)) * (index + 1)));
                        break;
                    case MirDirection.Down:
                        OffSetMove = new Point(0, (int)((-Map.CellHeight * i / (float)(count)) * (index + 1)));
                        break;
                    case MirDirection.DownLeft:
                        OffSetMove = new Point((int)((Map.CellWidth * i / (float)(count)) * (index + 1)), (int)((-Map.CellHeight * i / (float)(count)) * (index + 1)));
                        break;
                    case MirDirection.Left:
                        OffSetMove = new Point((int)((Map.CellWidth * i / (float)(count)) * (index + 1)), 0);
                        break;
                    case MirDirection.UpLeft:
                        OffSetMove = new Point((int)((Map.CellWidth * i / (float)(count)) * (index + 1)), (int)((Map.CellHeight * i / (float)(count)) * (index + 1)));
                        break;
                }

                OffSetMove = new Point(OffSetMove.X % 2 + OffSetMove.X, OffSetMove.Y % 2 + OffSetMove.Y);
                break;
            default:
                OffSetMove = Point.Empty;
                Movement = CurrentLocation;
                break;
        }

    }
}

