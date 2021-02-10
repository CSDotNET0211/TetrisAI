using System;
using System.Collections;
using System.Collections.Generic;

public class NewEvaluation
{
    static List<int> completeLineHeight = new List<int>();
    static List<TetrisEnvironment.Vector2> holeYs = new List<TetrisEnvironment.Vector2>();
    static float[] holeup = new float[4];
    static int[] maxHeights = new int[10];

    static public float h_sumofheight;
    static public float h_holeCount;
    static public float h_dekoboko;
    static public float h_line1;
    static public float h_line2;
    static public float h_line3;
    static public float h_line4;
    static public float h_holeup1;
    static public float h_holeup2;
    static public float h_holeup3;
    static public float h_holeup4;


    static public float Evaluate(int[,] field, TetrisEnvironment.Vector2[] minopositions, float beforeEval, bool arrayApply, bool test = false)
    {
        holeYs.Clear();



        int[,] field_clone;
        if (arrayApply)
        {
            field_clone = field;
        }
        else
        {
            field_clone = (int[,])field.Clone();

        }

        int completeLine = 0;
        foreach (TetrisEnvironment.Vector2 pos in minopositions)
        {
            field_clone[pos.x, pos.y] = 1;
        }
        CheckLine(field_clone, ref completeLine);





        int sumofheight = 0;

        //高さ合計計算
        for (int x = 0; x < 10; x++)
        {
            for (int y = 25 - 1; y >= 0; y--)
            {
                if (field_clone[x, y] == 1)
                {
                    maxHeights[x] = y;
                    sumofheight += y;
                    continue;
                }
            }
        }

        int holeCount = 0;
        //穴計算
        for (int x = 0; x < 10; x++)
        {
            for (int y = 0; y < 25 - 1; y++)
            {
                if (field_clone[x, y] == 0 && field_clone[x, y + 1] == 1)
                {
                    holeCount++;
                    holeYs.Add(new TetrisEnvironment.Vector2(x, y));
                }
            }
        }

        for (int y = 25 - 1 - 1; y >= 0; y--)
        {
            for (int x = 0; x < 10; x++)
            {
                if (field_clone[x, y] == 0 && field_clone[x, y + 1] == 1)
                {
                    holeCount++;
                    holeYs.Add(new TetrisEnvironment.Vector2(x, y));
                }
            }
        }





        int holemax;
        if (holeYs.Count < 4)
            holemax = holeYs.Count;
        else
            holemax = 4;
        //穴の上
        for (int i = 0; i < holemax; i++)
        {
            int ad = 1;
            while (true)
            {
                if (holeYs[i].y + ad < 25 && field_clone[holeYs[i].x, holeYs[i].y + ad] == 1)
                    ad++;
                else
                {
                    holeup[i] = ad;
                    break;
                }
            }

        }


        //一番深いのはノーカンにしよう
        int dekoboko = 0;
        for (int i = 0; i < maxHeights.Length - 1; i++)
            dekoboko += Math.Abs(maxHeights[i] - maxHeights[i + 1]);

        float ajust = 0;
        switch (completeLine)
        {
            case 1:
                ajust += h_line1;
                break;
            case 2:
                ajust += h_line2;
                break;
            case 3:
                ajust += h_line3;
                break;
            case 4:
                ajust += h_line4;
                break;
        }



        if (test)
        {
            TetrisEnvironment.Print(field_clone, false);
            Console.WriteLine($"でこぼこ:{ dekoboko * -0.184 }");
            Console.WriteLine($"高さ合計:{ sumofheight * -0.51f}");
            Console.WriteLine($"穴:{ holeCount * -0.6f }");
            Console.WriteLine($"クリアライン:{ completeLine * 0.76 }");
            Console.WriteLine($"合計:{(float)(sumofheight * -0.51f + completeLine * 0.76 + holeCount * -0.356f + dekoboko * -0.184)}");
            Console.ReadKey();
        }

        return (float)(sumofheight * h_sumofheight +/* completeLine * 0.76+*/  holeCount * h_holeCount + dekoboko * h_dekoboko + beforeEval * 0.6f + ajust + holeup[0] * h_holeup1 + holeup[1] * h_holeup2 + holeup[2] * h_holeup3 + holeup[3] * h_holeup4);

    }

    static void CheckLine(int[,] field, ref int completeLineCount)
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

        completeLineCount = completeLineHeight.Count;

        foreach (int y in completeLineHeight)
        {
            DownLine(field, y);
        }
    }

    static void DownLine(int[,] field, int y)
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
