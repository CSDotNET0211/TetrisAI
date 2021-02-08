using System;
using System.Collections;
using System.Collections.Generic;

public class NewTetrisAI
{
    static TetrisEnvironment.Way? _finalWays = new TetrisEnvironment.Way();

    static public TetrisEnvironment.Way Find(int[,] field, int nowmino, int[] nexts, int ojama, int? hold, bool canHold)
    {
        Init();

        //通常検索
        Search(field, nowmino, nexts, ojama, 0);

        //ホールド
        if (canHold)
        {
            if (hold == null)
            {
                int newmino = nexts[0];

                for (int i = 0; i < nexts.Length - 1; i++)
                {
                    nexts[i] = nexts[i + 1];
                }
                nexts[nexts.Length - 1] = (int)TetrisEnvironment.MinoKind.Empty;

                Search(field, newmino, nexts, ojama, 0, ((int)TetrisEnvironment.Move.Hold).ToString());
            }
            else
            {
                Search(field, (int)hold, nexts, ojama, 0, ((int)TetrisEnvironment.Move.Hold).ToString());

            }
        }


        return (TetrisEnvironment.Way)_finalWays;


    }

    static void Init()
    {
        _finalWays = null;
    }

    /// <summary>
    /// 利用可能なネクストかあるかチェック
    /// </summary>
    /// <param name="nexts">ネクスト</param>>
    /// <returns>空ならtrue</returns>
    static bool CheckNextIsEmpty(int[] nexts)
    {
        for (int i = nexts.Length - 1; i >= 0; i--)
        {
            if (nexts[i] != (int)TetrisEnvironment.MinoKind.Empty)
            {
                return false;
            }
        }

        return true;
    }

    static private void Search(int[,] field, int nowmino, int[] nexts, int ojama, float beforeEval, string moveajust = "")
    {
        TetrisEnvironment.Mino mino = TetrisEnvironment.CreateMino((TetrisEnvironment.MinoKind)nowmino);

        Dictionary<string, TetrisEnvironment.Way> ways = new Dictionary<string, TetrisEnvironment.Way>();
        SearchPattern(mino, field, moveajust, ways, 0, TetrisEnvironment.Move.Null, beforeEval);

        var minoclone = TetrisEnvironment.Mino.Clone(mino);

        //右回転
        if (TetrisEnvironment.RotateMino(TetrisEnvironment.Move.RightRotate, ref mino, field))
        {
            SearchPattern(mino, field, moveajust + "3", ways, 1, TetrisEnvironment.Move.Null, beforeEval);


            //180回転
            if (TetrisEnvironment.RotateMino(TetrisEnvironment.Move.RightRotate, ref mino, field))
            {
                SearchPattern(mino, field, moveajust + "33", ways, 2, TetrisEnvironment.Move.Null, beforeEval);



            }
        }

        //左回転
        if (TetrisEnvironment.RotateMino(TetrisEnvironment.Move.LeftRotate, ref minoclone, field))
        {
            SearchPattern(minoclone, field, moveajust + "4", ways, 1, TetrisEnvironment.Move.Null, beforeEval);

        }


        //ネクストが残っていたら
        if (!CheckNextIsEmpty(nexts))
        {
            var nexts_clone = (int[])nexts.Clone();

            int newmino = nexts_clone[0];
            for (int i = 0; i < nexts_clone.Length - 1; i++)
            {
                nexts_clone[i] = nexts_clone[i + 1];
            }
            nexts_clone[nexts.Length - 1] = (int)TetrisEnvironment.MinoKind.Empty;

            foreach (var value in ways.Values)
            {
                var field_clone = (int[,])field.Clone();
                float eval = NewEvaluation.Evaluate(field_clone, value.mino_positions, beforeEval, true);

                Search(field_clone, newmino, nexts_clone, ojama, eval, value.way);

            }
        }
        //ネクストなし
        else
        {
            TetrisEnvironment.Way bestkouho = new TetrisEnvironment.Way();
            bool first = true;
            foreach (var value in ways.Values)
            {
                //TetrisEnvironment.Print(field, value.mino_positions, true);
                if (first)
                {
                    bestkouho = value;
                    first = false;
                }

                if (bestkouho.eval < value.eval)
                    bestkouho = value;
            }

            if (_finalWays == null)
            {
                _finalWays = bestkouho;
            }
            else
            {
                if (_finalWays.Value.eval < bestkouho.eval)
                    _finalWays = bestkouho;
            }

        }


    }
    static private void SearchPattern(
        TetrisEnvironment.Mino mino,
        int[,] field,
 string move,
 Dictionary<string, TetrisEnvironment.Way> hashset,
 int movecount,
 TetrisEnvironment.Move beforeHorizontal,
 float beforeEval
        )
    {
        TetrisEnvironment.Mino mino_clone;
        //ハードドロップ
        {
            mino_clone = TetrisEnvironment.Mino.Clone(mino);

            int count = 0;
            while (true)
            {
                if (TetrisEnvironment.CheckValidPosition(field, mino_clone.positions, new TetrisEnvironment.Vector2(0, count)))
                    count--;
                else
                { count++; break; }
            }

            mino_clone.positions[0] += new TetrisEnvironment.Vector2(0, count);
            mino_clone.positions[1] += new TetrisEnvironment.Vector2(0, count);
            mino_clone.positions[2] += new TetrisEnvironment.Vector2(0, count);
            mino_clone.positions[3] += new TetrisEnvironment.Vector2(0, count);

            //順番を整列
            for (int i = 1; i < 4; i++)
            {
                if (mino_clone.positions[i].x < mino_clone.positions[i - 1].x)
                {
                    TetrisEnvironment.Vector2 temp = mino_clone.positions[i - 1];
                    mino_clone.positions[i - 1] = mino_clone.positions[i];
                    mino_clone.positions[i] = temp;
                }
            }

            for (int j = 1; j < 4; j++)
            {
                int num = 0;
                if (mino_clone.positions[j].x == mino_clone.positions[j - 1].x)
                {
                    while (j - 1 - num >= 0 && mino_clone.positions[j - num].y < mino_clone.positions[j - 1 - num].y)
                    {
                        TetrisEnvironment.Vector2 temp = mino_clone.positions[j - 1 - num];
                        mino_clone.positions[j - 1 - num] = mino_clone.positions[j - num];
                        mino_clone.positions[j - num] = temp;
                        num++;
                    }
                }
            }

            string hash = mino_clone.positions[0].x.ToString();
            hash += mino_clone.positions[0].y.ToString();
            hash += mino_clone.positions[1].x.ToString();
            hash += mino_clone.positions[1].y.ToString();
            hash += mino_clone.positions[2].x.ToString();
            hash += mino_clone.positions[2].y.ToString();
            hash += mino_clone.positions[3].x.ToString();
            hash += mino_clone.positions[3].y.ToString();

            TetrisEnvironment.Way output;


            if (hashset.TryGetValue(hash, out output))
            {
                if (output.moveCount > movecount)
                {
                    hashset.Remove(hash);
                    hashset.Add(hash, new TetrisEnvironment.Way(move + (int)TetrisEnvironment.Move.HardDrop, NewEvaluation.Evaluate(field, mino_clone.positions, beforeEval, false), mino_clone.positions, movecount + 1, hash));
                }
            }
            else
                hashset.Add(hash, new TetrisEnvironment.Way(move + (int)TetrisEnvironment.Move.HardDrop, NewEvaluation.Evaluate(field, mino_clone.positions, beforeEval, false), mino_clone.positions, movecount + 1, hash));

        }

        //右移動
        if (beforeHorizontal != TetrisEnvironment.Move.Left && TetrisEnvironment.CheckValidPosition(field, mino.positions, new TetrisEnvironment.Vector2(1, 0)))
        {
            mino_clone = TetrisEnvironment.Mino.Clone(mino);

            mino_clone.positions[0] += TetrisEnvironment.Vector2.X1;
            mino_clone.positions[1] += TetrisEnvironment.Vector2.X1;
            mino_clone.positions[2] += TetrisEnvironment.Vector2.X1;
            mino_clone.positions[3] += TetrisEnvironment.Vector2.X1;

            SearchPattern(mino_clone, field, move + (int)TetrisEnvironment.Move.Right, hashset, movecount + 1, TetrisEnvironment.Move.Right, beforeEval);
        }

        //左移動
        if (beforeHorizontal != TetrisEnvironment.Move.Right && TetrisEnvironment.CheckValidPosition(field, mino.positions, new TetrisEnvironment.Vector2(-1, 0)))
        {
            mino_clone = TetrisEnvironment.Mino.Clone(mino);

            mino_clone.positions[0] += TetrisEnvironment.Vector2.MinusX1;
            mino_clone.positions[1] += TetrisEnvironment.Vector2.MinusX1;
            mino_clone.positions[2] += TetrisEnvironment.Vector2.MinusX1;
            mino_clone.positions[3] += TetrisEnvironment.Vector2.MinusX1;

            SearchPattern(mino_clone, field, move + (int)TetrisEnvironment.Move.Left, hashset, movecount + 1, TetrisEnvironment.Move.Left, beforeEval);
        }

    }

}





