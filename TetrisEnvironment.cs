using System;
using System.Collections.Generic;

public class TetrisEnvironment
{
    #region SRS
    static Vector2[] ZeroToOne = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2) };
    static Vector2[] OneToZero = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2) };
    static Vector2[] OneToTwo = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, -1), new Vector2(0, 2), new Vector2(1, 2) };
    static Vector2[] TwoToOne = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(0, -2), new Vector2(-1, -2) };
    static Vector2[] TwoToThree = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2) };
    static Vector2[] ThreeToTwo = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2) };
    static Vector2[] ThreeToZero = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(-1, -1), new Vector2(0, 2), new Vector2(-1, 2) };
    static Vector2[] ZeroToThree = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, -2), new Vector2(1, -2) };

    static Vector2[] IZeroToOne = { new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2) };
    static Vector2[] IOneToZero = { new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, 1), new Vector2(-1, -2) };
    static Vector2[] IOneToTwo = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, 2), new Vector2(2, -1) };
    static Vector2[] ITwoToOne = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1) };
    static Vector2[] ITwoToThree = { new Vector2(0, 0), new Vector2(2, 0), new Vector2(-1, 0), new Vector2(2, 1), new Vector2(-1, -2) };
    static Vector2[] IThreeToTwo = { new Vector2(0, 0), new Vector2(-2, 0), new Vector2(1, 0), new Vector2(-2, -1), new Vector2(1, 2) };
    static Vector2[] IThreeToZero = { new Vector2(0, 0), new Vector2(1, 0), new Vector2(-2, 0), new Vector2(1, -2), new Vector2(-2, 1) };
    static Vector2[] IZeroToThree = { new Vector2(0, 0), new Vector2(-1, 0), new Vector2(2, 0), new Vector2(-1, 2), new Vector2(2, -1) };
    #endregion

    #region 型定義
    /// <summary>
    /// 座標、回転、種類の情報を含むテトリミノの構造体
    /// </summary>
    public struct Mino
    {
        public Mino(Vector2[] positions, Rotate rotation, MinoKind kind)
        {
            this.positions = positions;
            this.rotation = rotation;
            this.kind = kind;
        }

        static public Mino Clone(Mino mino)
        {
            Mino mino_clone = new Mino();
            mino_clone.kind = mino.kind;
            mino_clone.positions = (Vector2[])mino.positions.Clone();
            mino_clone.rotation = mino.rotation;

            return mino_clone;
        }

        public Vector2[] positions;
        public Rotate rotation;
        public MinoKind kind;
    }
    /// <summary>
    /// ミノの種類
    /// </summary>
    public enum MinoKind
    {
        S = 0,
        Z = 1,
        L = 2,
        J = 3,
        O = 4,
        I = 5,
        T = 6,
        Ojama = 7,
        Empty = 8,
    }

    /// <summary>
    /// 2次元の座標を表す構造体
    /// </summary>
    public struct Vector2

    {
        public Vector2(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Vector2 operator +(Vector2 z, Vector2 w)
        {
            return new Vector2((z.x + w.x), (z.y + w.y));
        }

        public static Vector2 X2 = new Vector2(2, 0);
        public static Vector2 X1 = new Vector2(1, 0);
        public static Vector2 Y1 = new Vector2(0, 1);
        public static Vector2 MinusX1 = new Vector2(-1, 0);
        public static Vector2 MinusX2 = new Vector2(-2, 0);
        public static Vector2 Zero = new Vector2(0, 0);

        public int x;
        public int y;
    }

    /// <summary>
    /// 複数の情報を持つミノ設置パターンの構造体です
    /// </summary>
    public struct Way
    {
        public Way(string way, float eval, Vector2[] mino_positions, int moveCount, string hash)
        {
            this.way = way;
            this.eval = eval;
            this.mino_positions = mino_positions;
            this.moveCount = moveCount;
            this.hash = hash;
        }

        public string way;
        public string hash;
        public float eval;
        public Vector2[] mino_positions;
        public int moveCount;
    }


    /// <summary>
    /// ミノを動かすときの行動
    /// </summary>
    public enum Move
    {
        Right = 0,
        Left = 1,
        SoftDrop = 2,
        RightRotate = 3,
        LeftRotate = 4,
        HardDrop = 5,
        Hold = 6,
        Down = 7,
        Null = 8


    }

    /// <summary>
    /// ミノの回転状態
    /// </summary>
    public enum Rotate
    {
        Zero = 0,
        Right = 1,
        Turn = 2,
        Left = 3
    }

    #endregion
    static List<int> completeLineHeight = new List<int>();

    static public void Print(int[,] field, Vector2[] positions, bool wait)
    {
        Console.Clear();

        int[,] field_clone = (int[,])field.Clone();

        foreach (Vector2 pos in positions)
        {
            field_clone[pos.x, pos.y] = 1;
        }

        for (int y = 24; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                if (field_clone[x, y] == 0)
                    Console.Write("・");
                else
                    Console.Write("■");
            }
            Console.Write("\r\n");
        }

        if (wait)
            Console.ReadKey();
    }

    static public void Print(int[,] field, bool wait)
    {
        int[,] field_clone = (int[,])field.Clone();

        for (int y = 24; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                if (field_clone[x, y] == 0)
                    Console.Write("・");
                else
                    Console.Write("■");
            }
            Console.Write("\r\n");
        }

        if (wait)
            Console.ReadKey();
    }

    /// <summary>
    /// ミノを生成する
    /// </summary>
    /// <param name="kind">ミノの種類</param>
    /// <returns>Minoオブジェクト</returns>
    static public Mino CreateMino(MinoKind kind)
    {
        Mino mino = new Mino();
        mino.kind = kind;
        mino.rotation = Rotate.Zero;
        mino.positions = new Vector2[4];

        switch (kind)
        {
            case MinoKind.I:
                mino.positions[0] = new Vector2(3, 19);
                mino.positions[1] = new Vector2(4, 19);
                mino.positions[2] = new Vector2(5, 19);
                mino.positions[3] = new Vector2(6, 19);
                break;
            case MinoKind.O:
                mino.positions[0] = new Vector2(4, 19);
                mino.positions[1] = new Vector2(5, 19);
                mino.positions[2] = new Vector2(4, 20);
                mino.positions[3] = new Vector2(5, 20);
                break;
            case MinoKind.T:
                mino.positions[0] = new Vector2(4, 20);
                mino.positions[1] = new Vector2(3, 19);
                mino.positions[2] = new Vector2(4, 19);
                mino.positions[3] = new Vector2(5, 19);
                break;
            case MinoKind.S:
                mino.positions[0] = new Vector2(4, 20);
                mino.positions[1] = new Vector2(5, 20);
                mino.positions[2] = new Vector2(3, 19);
                mino.positions[3] = new Vector2(4, 19);
                break;
            case MinoKind.Z:
                mino.positions[0] = new Vector2(3, 20);
                mino.positions[1] = new Vector2(4, 20);
                mino.positions[2] = new Vector2(4, 19);
                mino.positions[3] = new Vector2(5, 19);
                break;
            case MinoKind.L:
                mino.positions[0] = new Vector2(5, 20);
                mino.positions[1] = new Vector2(3, 19);
                mino.positions[2] = new Vector2(4, 19);
                mino.positions[3] = new Vector2(5, 19);
                break;
            case MinoKind.J:
                mino.positions[0] = new Vector2(3, 20);
                mino.positions[1] = new Vector2(3, 19);
                mino.positions[2] = new Vector2(4, 19);
                mino.positions[3] = new Vector2(5, 19);
                break;
            case MinoKind.Empty:
                throw new Exception();
        }

        return mino;
    }

    /// <summary>
    /// ミノを回転する
    /// </summary>
    /// <param name="rotate">回転する方向</param>
    /// <param name="mino">回転させるミノ</param>
    /// <param name="field">盤面</param>
    /// <returns>回転できるかどうか</returns>
    static public bool RotateMino(Move rotate, ref Mino mino, int[,] field)
    {
        if (mino.kind == MinoKind.O)
            return false;
        Rotate afterRotate;

        afterRotate = GetNextRotation(rotate, mino);

        SimpleRotate(afterRotate, ref mino);

        switch (mino.rotation)
        {
            case Rotate.Zero:
                switch (afterRotate)
                {
                    case Rotate.Right:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(IZeroToOne, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(ZeroToOne, ref mino, field, afterRotate);

                    case Rotate.Left:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(IZeroToThree, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(ZeroToThree, ref mino, field, afterRotate);

                }
                break;

            case Rotate.Right:
                switch (afterRotate)
                {
                    case Rotate.Turn:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(IOneToTwo, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(OneToTwo, ref mino, field, afterRotate);


                    case Rotate.Zero:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(IOneToZero, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(OneToZero, ref mino, field, afterRotate);

                }
                break;

            case Rotate.Left:
                switch (afterRotate)
                {
                    case Rotate.Turn:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(IThreeToTwo, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(ThreeToTwo, ref mino, field, afterRotate);


                    case Rotate.Zero:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(IThreeToZero, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(ThreeToZero, ref mino, field, afterRotate);

                }
                break;

            case Rotate.Turn:
                switch (afterRotate)
                {
                    case Rotate.Right:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(ITwoToOne, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(TwoToOne, ref mino, field, afterRotate);


                    case Rotate.Left:
                        if (mino.kind == MinoKind.I)
                            return TryMovetoValidPosition(ITwoToThree, ref mino, field, afterRotate);
                        else
                            return TryMovetoValidPosition(TwoToThree, ref mino, field, afterRotate);

                }
                break;

        }

        throw new System.Exception();
    }

    /// <summary>
    /// その場で回転する
    /// </summary>
    /// <param name="after">回転した後の状態</param>
    /// <param name="mino">現在のミノ</param>
    static private void SimpleRotate(Rotate after, ref Mino mino)
    {
        switch (mino.rotation)
        {
            case Rotate.Zero:
                switch (after)
                {
                    case Rotate.Right:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += -1;
                                mino.positions[3].y += -2;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += 1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += -2;
                                mino.positions[2].y += 1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;
                        }

                        break;

                    case Rotate.Left:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += -1;
                                mino.positions[3].x += -2;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += -2;
                                mino.positions[2].x += 1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += -1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;
                        }
                        break;
                }
                break;

            case Rotate.Right:
                switch (after)
                {
                    case Rotate.Turn:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += -1;
                                mino.positions[3].x += -2;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += -2;
                                mino.positions[2].x += 1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += -1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;
                        }

                        break;

                    case Rotate.Zero:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += 1;
                                mino.positions[3].y += 2;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += -1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 2;
                                mino.positions[2].y += -1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;
                        }

                        break;
                }
                break;

            case Rotate.Left:
                switch (after)
                {
                    case Rotate.Turn:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += -1;
                                mino.positions[3].y += -2;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += 1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += -2;
                                mino.positions[2].y += 1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += -2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += -1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;
                        }

                        break;

                    case Rotate.Zero:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += 1;
                                mino.positions[3].x += 2;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 2;
                                mino.positions[2].x += -1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += 1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;
                        }
                        break;
                }
                break;

            case Rotate.Turn:
                switch (after)
                {
                    case Rotate.Right:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += 1;
                                mino.positions[3].x += 2;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += 1;
                                mino.positions[1].x += 2;
                                mino.positions[2].x += -1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += 1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += 1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += 2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += -1;
                                break;
                        }
                        break;

                    case Rotate.Left:
                        switch (mino.kind)
                        {
                            case MinoKind.I:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += -1;
                                mino.positions[1].y += 0;
                                mino.positions[2].y += 1;
                                mino.positions[3].y += 2;
                                break;

                            case MinoKind.T:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.S:
                                mino.positions[0].x += -1;
                                mino.positions[1].x += 0;
                                mino.positions[2].x += -1;
                                mino.positions[3].x += 0;

                                mino.positions[0].y += 1;
                                mino.positions[1].y += 2;
                                mino.positions[2].y += -1;
                                mino.positions[3].y += 0;
                                break;

                            case MinoKind.Z:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += 1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.L:
                                mino.positions[0].x += 0;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 2;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;

                            case MinoKind.J:
                                mino.positions[0].x += -2;
                                mino.positions[1].x += -1;
                                mino.positions[2].x += 0;
                                mino.positions[3].x += 1;

                                mino.positions[0].y += 0;
                                mino.positions[1].y += -1;
                                mino.positions[2].y += 0;
                                mino.positions[3].y += 1;
                                break;
                        }
                        break;
                }
                break;

        }
    }

    static Rotate GetNextRotation(Move rotate, Mino mino)
    {
        int value;
        if (rotate == Move.RightRotate)
        {
            value = ((int)mino.rotation);
            value++;
            if (value == 4)
                value = 0;
        }
        else if (rotate == Move.LeftRotate)
        {
            value = ((int)mino.rotation);
            value--;
            if (value == -1)
                value = 3;
        }
        else
            throw new Exception();

        return (Rotate)value;
    }

    /// <summary>
    /// 移動可能かどうか
    /// </summary>
    /// <param name="field">判定する盤面</param>
    /// <param name="position">現在の位置</param>
    /// <param name="movePos">移動する座標</param>
    /// <returns></returns>
    static public bool CheckValidPosition(int[,] field, Vector2[] position, Vector2 movePos)
    {
        foreach (Vector2 pos in position)
        {
            if (pos.x + movePos.x >= 0 && pos.x + movePos.x < 10 &&
                pos.y + movePos.y >= 0)
            {
                if (field[pos.x + movePos.x, pos.y + movePos.y] == 1)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 回転したあとキックテーブルのテスト
    /// </summary>
    /// <param name="tryValue">キックテーブル</param>
    /// <param name="mino">回転させるミノ</param>
    /// <param name="field">盤面</param>
    /// <param name="afterRotate">回転後のrotatio算出出用</param>
    /// <returns></returns>
    static private bool TryMovetoValidPosition(Vector2[] tryValue, ref Mino mino, int[,] field, Rotate afterRotate)
    {

        Rotate temp = mino.rotation;
        mino.rotation = afterRotate;
        foreach (Vector2 pos in tryValue)
        {
            if (CheckValidPosition(field, mino.positions, pos))
            {
                mino.positions[0].x += pos.x;
                mino.positions[1].x += pos.x;
                mino.positions[2].x += pos.x;
                mino.positions[3].x += pos.x;

                mino.positions[0].y += pos.y;
                mino.positions[1].y += pos.y;
                mino.positions[2].y += pos.y;
                mino.positions[3].y += pos.y;
                return true;
            }
        }
        //回転できなかった場合元に戻す処理
        SimpleRotate(temp, ref mino);
        mino.rotation = temp;
        return false;
    }

    /// <summary>
    /// 指定した位置のミノが空いてるか
    /// </summary>
    /// <param name="field">盤面</param>
    /// <param name="position">ミノの位置</param>
    /// <returns></returns>
    static private bool CheckValidOnlyPosition(byte[,] field, Vector2 position)
    {
        if (position.x >= 0 && position.x < 10 &&
            position.y >= 0)
        {
            if (field[position.x, position.y] == 1)
            {
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    static public void CheckLine(int[,] field)
    {
        completeLineHeight.Clear();

        for (int y = 0; y < 25; y++)
        {
            int temp = 0;
            for (int x = 0; x < 10; x++)
            {
                if (field[x, y] == 1)
                    temp++;
            }

            if (temp == 10)
                completeLineHeight.Add(y);
        }

        foreach (int y in completeLineHeight)
        {
            DownLine(field, y);
        }
    }

    static public void DownLine(int[,] field, int y)
    {
        for (; y < 25 - 1; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                if (y == 23)
                    field[x, y] = 0;
                else
                    field[x, y] = field[x, y + 1];
            }

        }
    }

}